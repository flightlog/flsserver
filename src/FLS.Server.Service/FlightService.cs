using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FLS.Common.Exceptions;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using NLog;

namespace FLS.Server.Service
{
    public class FlightService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly AircraftService _aircraftService;
        private readonly ClubService _clubService;

        public FlightService(DataAccessService dataAccessService, AircraftService aircraftService,
            ClubService clubService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _aircraftService = aircraftService;
            _clubService = clubService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region EngineOperatingCounter
        public AircraftOperatingCounterResult GetAircraftOperatingCounterResult(AircraftOperatingCounterRequest request)
        {
            request.ArgumentNotNull("request");

            var result = new AircraftOperatingCounterResult()
            {
                AircraftId = request.AircraftId,
                AtDateTime = request.AtDateTime,
                AircraftHasNoEngine = false
            };

            var until = DateTime.UtcNow;
            if (request.AtDateTime.HasValue) until = request.AtDateTime.Value;

            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraft = context.Aircrafts.FirstOrDefault(a => a.AircraftId == request.AircraftId);
                var counterUnitTypes = context.CounterUnitTypes.ToList();

                if (aircraft == null)
                {
                    throw new EntityNotFoundException("Aircraft", request.AircraftId);
                }

                if (aircraft.HasEngine == false)
                {
                    result.AircraftHasNoEngine = true;
                    return result;
                }

                if (aircraft.EngineOperatingCounterUnitTypeId.HasValue)
                {
                    var counterUnitType =
                        counterUnitTypes.FirstOrDefault(c => c.CounterUnitTypeId == aircraft.EngineOperatingCounterUnitTypeId.Value);

                    counterUnitType.NotNull("CounterUnitType");

                    result.EngineOperatingCounterUnitTypeKeyName = counterUnitType.CounterUnitTypeKeyName;
                }
                else
                {
                    var counterUnitType =
                        counterUnitTypes.FirstOrDefault(c => c.CounterUnitTypeKeyName.ToLower() == "min");

                    counterUnitType.NotNull("CounterUnitType");

                    result.EngineOperatingCounterUnitTypeKeyName = counterUnitType.CounterUnitTypeKeyName;
                }

                var aircraftOperatingCounter = context.AircraftOperatingCounters
                    .Where(q => q.AtDateTime <= until && q.AircraftId == request.AircraftId)
                    .OrderByDescending(o => o.AtDateTime)
                    .FirstOrDefault();
                
                bool foundAnyOperatingCounterValue = false;

                if (aircraftOperatingCounter == null)
                {
                    aircraftOperatingCounter = new AircraftOperatingCounter()
                    {
                        AtDateTime = DateTime.MinValue,
                        AircraftId = request.AircraftId,
                        EngineOperatingCounterInSeconds = 0,
                        FlightOperatingCounterInSeconds = 0
                    };
                }
                else
                {
                    foundAnyOperatingCounterValue = true;
                }
                
                //get flights between last aircraft operating counter value (if available) and requested date time
                var flights = context.Flights
                    .Where(f => f.AircraftId == request.AircraftId
                                && f.StartDateTime >= aircraftOperatingCounter.AtDateTime
                                && (f.LdgDateTime.HasValue == false || f.LdgDateTime <= until)).ToList();

                long operatingCounterValue = aircraftOperatingCounter.EngineOperatingCounterInSeconds.GetValueOrDefault(0);

                foreach (var flight in flights.OrderBy(o => o.LdgDateTime))
                {
                    if (flight.EngineEndOperatingCounterInSeconds.HasValue)
                    {
                        operatingCounterValue = flight.EngineEndOperatingCounterInSeconds.Value;
                        foundAnyOperatingCounterValue = true;
                        continue;
                    }

                    if (flight.EngineStartOperatingCounterInSeconds.HasValue)
                    {
                        operatingCounterValue = flight.EngineStartOperatingCounterInSeconds.Value;
                        foundAnyOperatingCounterValue = true;
                    }

                    var counterUnitType =
                        counterUnitTypes.FirstOrDefault(
                            q => q.CounterUnitTypeKeyName.ToLower() == result.EngineOperatingCounterUnitTypeKeyName.ToLower());

                    counterUnitType.NotNull("CounterUnitType");

                    operatingCounterValue += Convert.ToInt64(flight.FlightDurationZeroBased.TotalSeconds);
                }

                if (foundAnyOperatingCounterValue)
                {
                    result.EngineOperatingCounterInSeconds = operatingCounterValue;
                }

                return result;
            }
        }
        #endregion EngineOperatingCounter

        #region FlightState
        public List<FlightStateListItem> GetFlightAirStateListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightStates = context.FlightAirStates.OrderBy(a => a.FlightAirStateId);

                var flightStateListItems = flightStates.Select(flightState => new FlightStateListItem()
                {
                    FlightStateId = flightState.FlightAirStateId,
                    FlightState = flightState.FlightAirStateName
                }).ToList();

                return flightStateListItems;
            }
        }
        
        public List<FlightStateListItem> GetFlightValidationStateListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightStates = context.FlightValidationStates.OrderBy(a => a.FlightValidationStateId);

                var flightStateListItems = flightStates.Select(flightState => new FlightStateListItem()
                {
                    FlightStateId = flightState.FlightValidationStateId,
                    FlightState = flightState.FlightValidationStateName
                }).ToList();

                return flightStateListItems;
            }
        }

        public List<FlightStateListItem> GetFlightProcessStateListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightStates = context.FlightProcessStates.OrderBy(a => a.FlightProcessStateId);

                var flightStateListItems = flightStates.Select(flightState => new FlightStateListItem()
                {
                    FlightStateId = flightState.FlightProcessStateId,
                    FlightState = flightState.FlightProcessStateName
                }).ToList();

                return flightStateListItems;
            }
        }
        #endregion FlightState

        #region StartType
        public List<StartTypeListItem> GetStartTypeListItems()
        {
            var startTypes = GetStartTypes();

            var startTypeListItems = startTypes.Select(startType => startType.ToStartTypeListItem()).ToList();

            return startTypeListItems;
        }

        public List<StartTypeListItem> GetGliderStartTypeListItems()
        {
            var startTypes = GetStartTypes(q => q.IsForMotorFlights == false);

            var startTypeListItems = startTypes.Select(startType => startType.ToStartTypeListItem()).ToList();

            return startTypeListItems;
        }

        public List<StartTypeListItem> GetMotorStartTypeListItems()
        {
            var startTypes = GetStartTypes(q => q.IsForMotorFlights);

            var startTypeListItems = startTypes.Select(startType => startType.ToStartTypeListItem()).ToList();

            return startTypeListItems;
        }

        internal List<StartType> GetStartTypes(Expression<Func<StartType, bool>> startTypeFilter)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var startTypes = context.StartTypes
                    .Where(startTypeFilter)
                    .OrderBy(x => x.StartTypeId)
                    .ToList();

                return startTypes;
            }
        }

        internal List<StartType> GetStartTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var startTypes = context.StartTypes.OrderBy(x => x.StartTypeId).ToList();

                return startTypes;
            }
        }
        #endregion StartType

        #region FlightCostBalanceType
        public List<FlightCostBalanceTypeListItem> GetFlightCostBalanceTypeListItems()
        {
            var entities = GetFlightCostBalanceTypes();

            var items = entities.Select(f => f.ToFlightCostBalanceTypeListItem()).ToList();

            return items;
        }

        internal List<FLS.Server.Data.DbEntities.FlightCostBalanceType> GetFlightCostBalanceTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightCostBalanceTypes = context.FlightCostBalanceTypes.OrderBy(x => x.FlightCostBalanceTypeId).ToList();

                return flightCostBalanceTypes;
            }
        }
        #endregion FlightCostBalanceType

        #region Flight
        public List<FlightOverview> GetFlightOverviews()
        {
            var flights = GetFlights(flight => flight.OwnerId == CurrentAuthenticatedFLSUserClubId,
                includeTowFlight: false);

            var preparedFlights = PrepareFlightOverviews(flights);

            return preparedFlights;
        }

        public List<FlightOverview> GetFlightOverviewsWithinToday()
        {
            return GetFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<FlightOverview> GetFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            //needed for flights without start time (null values in StartDateTime)
            DateTime fromDateTime = fromDate.Date;
            DateTime toDateTime;

            if (toDate.Date < DateTime.MaxValue.Date)
            {
                toDateTime = toDate.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                toDateTime = DateTime.MaxValue;
            }

            bool isTodayIncluded = fromDate.Date <= DateTime.Now.Date && toDate.Date >= DateTime.Now.Date;

            var flights = GetFlights(flight => (flight.StartDateTime == null && isTodayIncluded
                                               ||
                                               flight.StartDateTime.Value >= fromDateTime &&
                                               flight.StartDateTime.Value <= toDateTime
                                               || flight.LdgDateTime == null && isTodayIncluded
                                               ||
                                               flight.LdgDateTime.Value >= fromDateTime &&
                                               flight.LdgDateTime.Value <= toDateTime)
                                              && flight.OwnerId == CurrentAuthenticatedFLSUser.ClubId, 
                                              includeTowFlight:false);

            return PrepareFlightOverviews(flights);
        }

        public List<FlightOverview> GetMotorFlightOverviews()
        {
            var flights = GetFlights(flight => flight.OwnerId == CurrentAuthenticatedFLSUserClubId 
            && flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight,
                includeTowFlight: false);

            var preparedFlights = PrepareFlightOverviews(flights);

            return preparedFlights;
        }

        public List<FlightOverview> GetMotorFlightOverviewsWithinToday()
        {
            return GetMotorFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<FlightOverview> GetMotorFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            //needed for flights without start time (null values in StartDateTime)
            DateTime fromDateTime = fromDate.Date;
            DateTime toDateTime;

            if (toDate.Date < DateTime.MaxValue.Date)
            {
                toDateTime = toDate.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                toDateTime = DateTime.MaxValue;
            }

            bool isTodayIncluded = fromDate.Date <= DateTime.Now.Date && toDate.Date >= DateTime.Now.Date;

            var flights = GetFlights(flight => (flight.StartDateTime == null && isTodayIncluded
                                               ||
                                               flight.StartDateTime.Value >= fromDateTime &&
                                               flight.StartDateTime.Value <= toDateTime
                                               || flight.LdgDateTime == null && isTodayIncluded
                                               ||
                                               flight.LdgDateTime.Value >= fromDateTime &&
                                               flight.LdgDateTime.Value <= toDateTime)
                                              && flight.OwnerId == CurrentAuthenticatedFLSUser.ClubId
                                                && flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight,
                                              includeTowFlight: false);

            return PrepareFlightOverviews(flights);
        }

        private List<FlightOverview> PrepareFlightOverviews(List<Flight> flights)
        {
            var flightOverviewList = flights.Select(e => e.ToFlightOverview()).ToList();
            SetFlightOverviewSecurity(flightOverviewList);

            return flightOverviewList.ToList();
        }

        #region GliderFlights
        public List<GliderFlightOverview> GetGliderFlightOverviews()
        {
            var flights = GetFlights(flight => flight.OwnerId == CurrentAuthenticatedFLSUserClubId
                                              && flight.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight);

            var preparedFlights = PrepareGliderFlightOverviews(flights);

            return preparedFlights;
        }

        public List<GliderFlightOverview> GetGliderFlightOverviewsWithinToday()
        {
            return GetGliderFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<GliderFlightOverview> GetGliderFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            //needed for flights without start time (null values in StartDateTime)
            DateTime fromDateTime = fromDate.Date;
            DateTime toDateTime;

            if (toDate.Date < DateTime.MaxValue.Date)
            {
                toDateTime = toDate.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                toDateTime = DateTime.MaxValue;
            }

            bool isTodayIncluded = fromDate.Date <= DateTime.Now.Date && toDate.Date >= DateTime.Now.Date;

            var flights = GetFlights(flight => (flight.StartDateTime == null && isTodayIncluded
                                               ||
                                               flight.StartDateTime.Value >= fromDateTime &&
                                               flight.StartDateTime.Value <= toDateTime
                                               || flight.LdgDateTime == null && isTodayIncluded
                                               ||
                                               flight.LdgDateTime.Value >= fromDateTime &&
                                               flight.LdgDateTime.Value <= toDateTime)
                                              && flight.OwnerId == CurrentAuthenticatedFLSUser.ClubId
                                              && flight.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight);

            return PrepareGliderFlightOverviews(flights);
        }

        private List<GliderFlightOverview> PrepareGliderFlightOverviews(List<Flight> flights)
        {
            var flightOverviewList = flights.Select(e => e.ToGliderFlightOverview()).ToList();
            SetGliderFlightOverviewSecurity(flightOverviewList);

            return flightOverviewList.ToList();
        }
        #endregion GliderFlights

        public FlightDetails GetFlightDetails(Guid flightId)
        {
            var flight = GetFlight(flightId);

            var flightDetails = flight.ToFlightDetails();
            SetFlightDetailsSecurity(flightDetails, flight);

            return flightDetails;
        }
        
        /// <summary>
        /// Gets the flights which have been created or validated today.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        internal List<Flight> GetFlightsCreatedOrValidatedToday(Guid clubId)
        {
            DateTime today = DateTime.Now.Date;

            var flights = GetFlights(flight => (DbFunctions.TruncateTime(flight.CreatedOn) == today.Date
                                                || (flight.ValidatedOn.HasValue && 
                                                DbFunctions.TruncateTime(flight.ValidatedOn.Value) == today.Date))
                                               && flight.OwnerId == clubId 
                                               &&
                                              (flight.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight
                                               || flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight));

            return flights;
        }

        public void ValidateFlights()
        {
            ValidateFlights(CurrentAuthenticatedFLSUserClubId);
        }

        public void ValidateFlights(Guid clubId)
        {
            try
            {
                //Flights which are not validated or are invalid, or have been modified since last validation date
                var flightsToValidate = GetFlights(flight => flight.OwnerId == clubId 
                    && (flight.ValidationStateId < (int)FLS.Data.WebApi.Flight.FlightValidationState.Valid
                    || (flight.ModifiedOn.HasValue && flight.ValidatedOn.HasValue && (flight.ModifiedOn >= flight.ValidatedOn))));

                using (var context = _dataAccessService.CreateDbContext())
                {
                    var flightValidationStates = context.FlightValidationStates.ToList();

                    foreach (var flight in flightsToValidate)
                    {
                        context.Flights.Attach(flight);

                        flight.ValidateFlight();
                        flight.DoNotUpdateMetaData = true;

                        try
                        {
                            Logger.Info(
                            $"The currently validated flight {flight} has now the following Flight-State: {flight.ValidationStateId} ({flightValidationStates.First(q => q.FlightValidationStateId == flight.ValidationStateId).FlightValidationStateName})");
                        }
                        catch (Exception exception)
                        {
                            Logger.Error(exception);
                        }
                    }

                    context.SaveChanges();
                }

                Logger.Info(string.Format("Saved the validated flights of today to database."));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing today flights for validating. Error: {ex.Message}");
            }
        }

        public void LockFlights(bool forceLockNow = false)
        {
            LockFlights(CurrentAuthenticatedFLSUserClubId, forceLockNow);
        }


        public void LockFlights(Guid clubId, bool forceLockNow = false)
        {
            if (forceLockNow && IsCurrentUserInRoleSystemAdministrator == false &&
                IsCurrentUserInRoleClubAdministrator == false)
            {
                //user can not force the lock if he is not club admin or system admin
                Logger.Info("User can not force the lock of the flight if he is not club admin or system admin");
                throw new MethodAccessException("User can not force the lock of the flight if he is not club admin or system admin");
            }

            try
            {
                DateTime lockingDate = DateTime.Today.AddDays(-2).AddTicks(-1);

                var flights =
                    GetFlights(
                        flight => flight.ValidationStateId == (int)FLS.Data.WebApi.Flight.FlightValidationState.Valid
                                    && flight.ProcessStateId < (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked
                                    && flight.OwnerId == clubId
                                   && (forceLockNow || DbFunctions.TruncateTime(flight.CreatedOn) <= lockingDate.Date));

                using (var context = _dataAccessService.CreateDbContext())
                {
                    foreach (var flight in flights)
                    {
                        context.Flights.Attach(flight);

                        flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked;

                        Logger.Info(string.Format("The valid flight {0} has now been locked.", flight));
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing flights for locking. Error: {ex.Message}");
            }
        }
        
        internal List<Flight> GetFlights(Expression<Func<Flight, bool>> flightFilter, bool includeTowFlight = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var query = context.Flights
                    .Include(Constants.Aircraft)
                    .Include(Constants.FlightType)
                    .Include(Constants.FlightCrews)
                    .Include(Constants.FlightCrews + "." + Constants.Person)
                    .Include(Constants.StartType)
                    .Include(Constants.StartLocation)
                    .Include(Constants.LdgLocation);
                    
                if (includeTowFlight)
                {
                    query = query.Include(Constants.TowFlight)
                        .Include(Constants.TowFlight + "." + Constants.Aircraft)
                        .Include(Constants.TowFlight + "." + Constants.FlightType)
                        .Include(Constants.TowFlight + "." + Constants.FlightCrews)
                        .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person)
                        .Include(Constants.TowFlight + "." + Constants.StartLocation)
                        .Include(Constants.TowFlight + "." + Constants.LdgLocation);
                }

                var flights = query.OrderByDescending(c => c.StartDateTime)
                    .Where(flightFilter)
                    .ToList();
                
                return flights;
            }
        }
        
        internal Flight GetFlight(Guid flightId, bool includeTowFlight = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var query = context.Flights
                    .Include(Constants.Aircraft)
                    .Include(Constants.FlightType)
                    .Include(Constants.FlightCrews)
                    .Include(Constants.FlightCrews + "." + Constants.Person)
                    .Include(Constants.StartType)
                    .Include(Constants.StartLocation)
                    .Include(Constants.LdgLocation);

                if (includeTowFlight)
                {
                    query = query.Include(Constants.TowFlight)
                        .Include(Constants.TowFlight + "." + Constants.Aircraft)
                        .Include(Constants.TowFlight + "." + Constants.FlightType)
                        .Include(Constants.TowFlight + "." + Constants.FlightCrews)
                        .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person)
                        .Include(Constants.TowFlight + "." + Constants.StartLocation)
                        .Include(Constants.TowFlight + "." + Constants.LdgLocation);
                }

                var flight = query.OrderByDescending(c => c.StartDateTime)
                    .FirstOrDefault(a => a.FlightId == flightId);

                return flight;
            }
        }

        public void InsertFlightDetails(FlightDetails flightDetails)
        {
            flightDetails.ArgumentNotNull("flightDetails");

            var relatedAircrafts = GetRelatedAircrafts(flightDetails);
            var relatedFlightTypes = GetRelatedFlightTypes(flightDetails);

            var flight = flightDetails.ToFlight(null, relatedAircrafts, relatedFlightTypes);

            flight.EntityNotNull("Flight");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Flights.Add(flight);
                context.SaveChanges();
            }

            //Map it back to details
            flight.ToFlightDetails(flightDetails);
        }
        
        public void UpdateFlightDetails(FlightDetails currentFlightDetails)
        {
            currentFlightDetails.ArgumentNotNull("currentFlightDetails");
            var originalFlight = GetFlight(currentFlightDetails.FlightId);
            originalFlight.EntityNotNull("Flight", currentFlightDetails.FlightId);

            if (originalFlight.ProcessStateId > (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked)
            {
                var message = $"Flight with Id: {originalFlight.Id} has already been invoiced and can not be updated!";
                Logger.Warn(message);
                throw new LockedFlightException(message);
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Flights.Attach(originalFlight);

                var relatedAircrafts = GetRelatedAircrafts(currentFlightDetails);
                var relatedFlightTypes = GetRelatedFlightTypes(currentFlightDetails);

                currentFlightDetails.ToFlight(originalFlight, relatedAircrafts, relatedFlightTypes);
                
                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to the flight details (FlightId, Metadata, FlightState, etc.)
                    originalFlight.ToFlightDetails(currentFlightDetails);
                }
            }
        }

        public void DeleteFlight(Guid flightId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Flights.Include(Constants.TowFlight).FirstOrDefault(l => l.FlightId == flightId);
                original.EntityNotNull("Flight", flightId);

                //manual cascade on delete as SQL Server does not support cascade delete with parent/child relation to the same entity
                if (original.TowFlight != null)
                {
                    context.Flights.Remove(original.TowFlight);
                }

                context.Flights.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Flight

        #region ReportingData
        public AircraftFlightReport GetAircraftFlightReport(AircraftFlightReportFilterCriteria filterCriteria)
        {
            filterCriteria.ArgumentNotNull("filterCriteria");

            var aircraftFlightReport = new AircraftFlightReport
                {
                    ReportingDateTime = DateTime.UtcNow,
                    FilterCriteria = filterCriteria,
                    AircraftFlightReportData = new List<AircraftFlightReportData>(filterCriteria.AircraftIds.Count)
                };

            var flights = GetAircraftFlights(filterCriteria);

            foreach (var aircraftId in filterCriteria.AircraftIds)
            {
                var aircraft = _aircraftService.GetAircraft(aircraftId);
                if (aircraft == null) continue;

                var aircraftFlightReportData = new AircraftFlightReportData
                {
                    AircraftId = aircraftId,
                    AircraftImmatriculation = aircraft.Immatriculation
                };

                if (flights != null && flights.Any())
                {
                    var id = aircraftId;
                    foreach (var flight in flights.Where(x => x.AircraftId == id))
                    {
                        aircraftFlightReportData.FlightDuration += flight.FlightDurationZeroBased;

                        if (flight.NrOfLdgs.HasValue)
                        {
                            aircraftFlightReportData.NumberOfAllStarts += flight.NrOfLdgs.Value;

                            if (flight.StartTypeId == (int)AircraftStartType.TowingByAircraft)
                            {
                                aircraftFlightReportData.NumberOfTowingStarts += flight.NrOfLdgs.Value;
                            }
                            else if (flight.StartTypeId == (int) AircraftStartType.WinchLaunch)
                            {
                                aircraftFlightReportData.NumberOfWinchStarts += flight.NrOfLdgs.Value;
                            }
                            else if (flight.StartTypeId == (int)AircraftStartType.SelfStart)
                            {
                                aircraftFlightReportData.NumberOfSelfStarts += flight.NrOfLdgs.Value;
                            }
                            else if (flight.StartTypeId == (int)AircraftStartType.MotorFlightStart)
                            {
                                aircraftFlightReportData.NumberOfMotorflightStarts += flight.NrOfLdgs.Value;
                            }
                            else
                            {
                                aircraftFlightReportData.NumberOfStartsOfUnknownType += flight.NrOfLdgs.Value;
                            }
                        }
                    }
                }

                aircraftFlightReport.AircraftFlightReportData.Add(aircraftFlightReportData);

            }

            return aircraftFlightReport;
        }

        public List<Flight> GetAircraftFlights(AircraftFlightReportFilterCriteria filterCriteria)
        {
            filterCriteria.ArgumentNotNull("filterCriteria");
            //needed for flights without start time (null values in StartDateTime)
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flights =
                    context.Flights
                                   .Include(Constants.Aircraft)
                                   .OrderBy(c => c.StartDateTime)
                                   .Where(flight => (flight.StartDateTime.Value >= filterCriteria.StatisticStartDateTime &&
                                                     flight.StartDateTime.Value <= filterCriteria.StatisticEndDateTime)
                                                    &&
                                                    ((filterCriteria.TakeWinchStarts && (flight.StartTypeId == (int)AircraftStartType.WinchLaunch))
                                                    || (filterCriteria.TakeTowingStarts && (flight.StartTypeId == (int)AircraftStartType.TowingByAircraft))
                                                    || (filterCriteria.TakeSelfStarts && (flight.StartTypeId == (int)AircraftStartType.SelfStart))
                                                    || (filterCriteria.TakeMotorFlightStarts && (flight.StartTypeId == (int)AircraftStartType.MotorFlightStart))
                                                    || (filterCriteria.TakeExternalStarts && (flight.StartTypeId == (int)AircraftStartType.ExternalStart)))
                                                    &&
                                                    flight.AirStateId >= (int)FLS.Data.WebApi.Flight.FlightAirState.Landed);
                
                if (filterCriteria.AircraftIds != null && filterCriteria.AircraftIds.Count > 0)
                {
                    flights = flights.Where(flight => filterCriteria.AircraftIds.Contains(flight.AircraftId));
                }

                if (filterCriteria.FlownByPersonIds != null && filterCriteria.FlownByPersonIds.Count > 0)
                {
                    flights = flights.Where(flight => flight.FlightCrews.Any(x => filterCriteria.FlownByPersonIds.Contains(x.PersonId)));
                }

                return flights.ToList();
            }
        }
        #endregion ReportingData

        #region Helper Methods
        private List<Aircraft> GetRelatedAircrafts(FlightDetails flightDetails)
        {
            var relatedAircrafts = new List<Aircraft>();

            if (flightDetails.GliderFlightDetailsData != null &&
                flightDetails.GliderFlightDetailsData.AircraftId.IsValid())
            {
                relatedAircrafts.Add(_aircraftService.GetAircraft(flightDetails.GliderFlightDetailsData.AircraftId));
            }

            if (flightDetails.TowFlightDetailsData != null &&
                flightDetails.TowFlightDetailsData.AircraftId.IsValid())
            {
                relatedAircrafts.Add(_aircraftService.GetAircraft(flightDetails.TowFlightDetailsData.AircraftId));
            }

            if (flightDetails.MotorFlightDetailsData != null &&
                flightDetails.MotorFlightDetailsData.AircraftId.IsValid())
            {
                relatedAircrafts.Add(_aircraftService.GetAircraft(flightDetails.MotorFlightDetailsData.AircraftId));
            }

            return relatedAircrafts;
        }

        private List<FlightType> GetRelatedFlightTypes(FlightDetails flightDetails)
        {
            var relatedFlightTypes = new List<FlightType>();

            if (flightDetails.GliderFlightDetailsData != null &&
                flightDetails.GliderFlightDetailsData.FlightTypeId.HasValue
                && flightDetails.GliderFlightDetailsData.FlightTypeId.Value.IsValid())
            {
                relatedFlightTypes.Add(_clubService.GetFlightType(flightDetails.GliderFlightDetailsData.FlightTypeId.Value));
            }

            if (flightDetails.TowFlightDetailsData != null &&
                flightDetails.TowFlightDetailsData.FlightTypeId.HasValue
                && flightDetails.TowFlightDetailsData.FlightTypeId.Value.IsValid())
            {
                relatedFlightTypes.Add(_clubService.GetFlightType(flightDetails.TowFlightDetailsData.FlightTypeId.Value));
            }

            if (flightDetails.MotorFlightDetailsData != null &&
                flightDetails.MotorFlightDetailsData.FlightTypeId.HasValue
                && flightDetails.MotorFlightDetailsData.FlightTypeId.Value.IsValid())
            {
                relatedFlightTypes.Add(_clubService.GetFlightType(flightDetails.MotorFlightDetailsData.FlightTypeId.Value));
            }

            return relatedFlightTypes;
        }
        #endregion Helper Methods

        #region Security
        private void SetFlightOverviewSecurity(IEnumerable<FlightOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var flightOverview in list)
                {
                    if (flightOverview.ProcessState < (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked
                        || IsCurrentUserInRoleClubAdministrator)
                    {
                        flightOverview.CanUpdateRecord = true;
                        flightOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        flightOverview.CanUpdateRecord = false;
                        flightOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetGliderFlightOverviewSecurity(IEnumerable<GliderFlightOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var flightOverview in list)
                {
                    if (flightOverview.ProcessState < (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked 
                        || IsCurrentUserInRoleClubAdministrator)
                    {
                        flightOverview.CanUpdateRecord = true;
                        flightOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        flightOverview.CanUpdateRecord = false;
                        flightOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetFlightDetailsSecurity(FlightDetails details, Flight flight)
        {
            if (details == null)
            {
                Logger.Error(string.Format("FlightDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (flight.ProcessStateId < (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked
                || IsCurrentUserInRoleClubAdministrator)
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;

                if (details.GliderFlightDetailsData != null)
                {
                    details.GliderFlightDetailsData.CanUpdateRecord = true;
                    details.GliderFlightDetailsData.CanDeleteRecord = true;
                }

                if (details.TowFlightDetailsData != null)
                {
                    details.TowFlightDetailsData.CanUpdateRecord = true;
                    details.TowFlightDetailsData.CanDeleteRecord = true;
                }

                if (details.MotorFlightDetailsData != null)
                {
                    details.MotorFlightDetailsData.CanUpdateRecord = true;
                    details.MotorFlightDetailsData.CanDeleteRecord = true;
                }
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }
        #endregion Security
    }
}
