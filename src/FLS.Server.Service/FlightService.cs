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
        public AircraftEngineOperatingCounterResult GetAircraftOperatingCounterResult(AircraftEngineOperatingCounterRequest request)
        {
            request.ArgumentNotNull("request");

            var result = new AircraftEngineOperatingCounterResult()
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

                if (aircraft == null)
                {
                    throw new EntityNotFoundException("Aircraft", request.AircraftId);
                }

                if (aircraft.HasEngine == false)
                {
                    result.AircraftHasNoEngine = true;
                    return result;
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
                        EngineOperatingCounterInMinutes = 0,
                        FlightOperatingCounterInMinutes = 0
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

                decimal operatingCounterValue = aircraftOperatingCounter.EngineOperatingCounterInMinutes.GetValueOrDefault(0);

                foreach (var flight in flights.OrderBy(o => o.LdgDateTime))
                {
                    if (flight.EngineEndOperatingCounterInMinutes.HasValue)
                    {
                        operatingCounterValue = flight.EngineEndOperatingCounterInMinutes.Value;
                        foundAnyOperatingCounterValue = true;
                        continue;
                    }

                    if (flight.EngineStartOperatingCounterInMinutes.HasValue)
                    {
                        operatingCounterValue = flight.EngineStartOperatingCounterInMinutes.Value;
                        foundAnyOperatingCounterValue = true;
                    }

                    if (flight.EngineTime.HasValue)
                    {
                        operatingCounterValue +=
                            Convert.ToDecimal(TimeSpan.FromTicks(flight.EngineTime.Value.Ticks).TotalMinutes);
                    }
                    else
                    {
                        operatingCounterValue += Convert.ToDecimal(flight.Duration.TotalMinutes);
                    }
                }

                if (foundAnyOperatingCounterValue)
                {
                    result.EngineOperatingCounterInMinutes = operatingCounterValue;
                }

                return result;
            }
        }
        #endregion EngineOperatingCounter

        #region FlightState
        public List<FlightStateListItem> GetFlightStateListItems()
        {
            var flightStates = GetFlightStates();

            var flightStateListItems = flightStates.Select(flightState => flightState.ToFlightStateListItem()).ToList();

            return flightStateListItems;
        }

        internal List<FLS.Server.Data.DbEntities.FlightState> GetFlightStates()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightStates = context.FlightStates.OrderBy(a => a.FlightStateId).ToList();

                return flightStates;
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
        
        /// <summary>
        /// Gets the flights which needs to be validated.
        /// </summary>
        /// <returns>All flights which have a flight state of invalid or less (started, landed, new).</returns>
        internal List<Flight> GetFlightsForValidating(Guid clubId)
        {
            var flights = GetFlights(
                flight => flight.OwnerId == clubId  
                            && flight.FlightStateId < (int)FLS.Data.WebApi.Flight.FlightState.Valid);

            return flights;
        }

        /// <summary>
        /// Gets the valid flights which the landing time and the created date is more than 2 days ago,
        /// so that these flights can be locked (no longer editable).
        /// </summary>
        /// <returns>List of flights which have a landing date and created date of 2 days ago and is in valid flight state.</returns>
        internal List<Flight> GetValidFlightsForLocking(Guid clubId)
        {
            DateTime lockingDate = DateTime.Today.AddDays(-2).AddTicks(-1);

            var flights =
                GetFlights(
                    flight => flight.FlightStateId == (int)FLS.Data.WebApi.Flight.FlightState.Valid
                                && flight.OwnerId == clubId  
                               && DbFunctions.TruncateTime(flight.CreatedOn) <= lockingDate.Date);
            return flights;
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

            flight.EntityNotNull("Flight", Guid.Empty);

            InsertFlight(flight);

            //Map it back to details
            flight.ToFlightDetails(flightDetails);
        }
        
        internal void InsertFlight(Flight flight)
        {
            flight.ArgumentNotNull("flight");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Flights.Add(flight);
                context.SaveChanges();
            }
        }

        public void UpdateFlightDetails(FlightDetails currentFlightDetails)
        {
            currentFlightDetails.ArgumentNotNull("currentFlightDetails");
            var originalFlight = GetFlight(currentFlightDetails.FlightId);
            originalFlight.EntityNotNull("Flight", currentFlightDetails.FlightId);

            if (originalFlight.FlightStateId > (int)FLS.Data.WebApi.Flight.FlightState.Locked)
            {
                var message = string.Format("Flight with Id: {0} is locked and can not be updated!",
                                            originalFlight.Id);
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
                        aircraftFlightReportData.FlightDuration += flight.Duration;

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
                                                    flight.FlightStateId >= (int)FLS.Data.WebApi.Flight.FlightState.Landed);
                
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
                    if (flightOverview.FlightState < (int)FLS.Data.WebApi.Flight.FlightState.Locked
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
                    if (flightOverview.FlightState < (int)FLS.Data.WebApi.Flight.FlightState.Locked 
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

            if (flight.FlightStateId < (int)FLS.Data.WebApi.Flight.FlightState.Locked
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
