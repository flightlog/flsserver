using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FLS.Common.Exceptions;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using LinqKit;
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

        #region FlightCrewType
        internal List<FlightCrewTypeListItem> GetFlightCrewTypeListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightCrewTypeListItems =
                    context.FlightCrewTypes.OrderBy(x => x.FlightCrewTypeName).Select(x => new FlightCrewTypeListItem()
                    {
                        FlightCrewTypeId = x.FlightCrewTypeId,
                        FlightCrewTypeName = x.FlightCrewTypeName
                    }).ToList();

                return flightCrewTypeListItems;
            }
        }
        #endregion FlightCrewType

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
            var filter = new PageableSearchFilter<FlightOverviewSearchFilter>();

            var flights = GetPagedFlightOverview(0, 10000, filter, false);
            return flights.Items;
        }

        public List<FlightOverview> GetFlightOverviewsWithinToday()
        {
            return GetFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<FlightOverview> GetFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var filter = new PageableSearchFilter<FlightOverviewSearchFilter>()
            {
                SearchFilter = new FlightOverviewSearchFilter()
                {
                    FlightDate = new DateTimeFilter()
                    {
                        From = fromDate,
                        To = toDate
                    }
                }
            };

            var flights = GetPagedFlightOverview(0, 10000, filter, false);
            return flights.Items;
        }
        
        public PagedList<FlightOverview> GetPagedFlightOverview(int? pageStart, int? pageSize, PageableSearchFilter<FlightOverviewSearchFilter> pageableSearchFilter, bool motorFlightsOnly)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<FlightOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new FlightOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
            }
            else if (pageableSearchFilter.Sorting.Count == 1 && pageableSearchFilter.Sorting.ContainsKey("FlightDate"))
            {
                //when sorting for flight date only, we sort for StartDateTime as second to get more valuable result
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var includedFlightCrewTypes = new int[]
                {
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator
                };

                var flightCrews = context
                    .FlightCrews
                    .Include("Person")
                    .Where(fc => includedFlightCrewTypes.Contains(fc.FlightCrewTypeId))
                    .GroupBy(fc => fc.FlightId)
                    .Select(fc => new
                    {
                        FlightId = fc.Key,
                        Pilot = fc.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent),
                        SecondCrew = fc.OrderBy(ffc => ffc.FlightCrewTypeId).FirstOrDefault(ffc =>
                                                        ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger),
                        WinchOperator = fc.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator)
                    });

                var flights = context.Flights
                    .Include(x => x.FlightType)
                    .Where(f => f.OwnerId == CurrentAuthenticatedFLSUserClubId);

                if (motorFlightsOnly)
                {
                    flights =
                        flights.Where(flight => flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight);
                }

                var filter = pageableSearchFilter.SearchFilter;
                flights = flights.WhereIf(filter.Immatriculation,
                        flight => flight.Aircraft.Immatriculation.Contains(filter.Immatriculation));
                flights = flights.WhereIf(filter.PilotName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Firstname)
                    .Contains(filter.PilotName));
                flights = flights.WhereIf(filter.SecondCrewName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger).Person.Firstname).Contains(filter.SecondCrewName));

                flights = flights.WhereIf(filter.FlightComment,
                    flight => flight.Comment.Contains(filter.FlightComment));

                if (filter.AirStates != null && filter.AirStates.Any()) flights = flights.Where(flight => filter.AirStates.Contains(flight.AirStateId));
                if (filter.ProcessStates != null && filter.ProcessStates.Any()) flights = flights.Where(flight => filter.ProcessStates.Contains(flight.ProcessStateId));

                flights = flights.WhereIf(filter.FlightCode,
                    flight => flight.FlightType.FlightCode.Contains(filter.FlightCode));

                if (filter.FlightDate != null)
                {
                    var dateTimeFilter = filter.FlightDate;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        flights = flights.Where(flight => flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >= DbFunctions.TruncateTime(from)
                            && DbFunctions.TruncateTime(flight.FlightDate) <= DbFunctions.TruncateTime(to));
                    }
                }

                //if (filter.LdgDateTime != null)
                //{
                //    var dateTimeFilter = filter.LdgDateTime;

                //    if (dateTimeFilter.Fixed.HasValue)
                //    {
                //        flights = flights.Where(flight => flight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.LdgDateTime) == DbFunctions.TruncateTime(dateTimeFilter.Fixed.Value));
                //    }
                //    else if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                //    {
                //        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                //        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                //        flights = flights.Where(flight => flight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.LdgDateTime) >= DbFunctions.TruncateTime(from)
                //            && DbFunctions.TruncateTime(flight.LdgDateTime) <= DbFunctions.TruncateTime(to));
                //    }
                //}

                if (filter.IsSoloFlight.HasValue)
                    flights = flights.Where(flight => flight.IsSoloFlight == filter.IsSoloFlight.Value);

                flights = flights.WhereIf(filter.StartType,
                    flight => flight.StartType.StartTypeName.Contains(filter.StartType));
                flights = flights.WhereIf(filter.StartLocation,
                    flight => flight.StartLocation.LocationName.Contains(filter.StartLocation));
                flights = flights.WhereIf(filter.LdgLocation,
                    flight => flight.LdgLocation.LocationName.Contains(filter.LdgLocation));

                //TODO: Search for flight duration
                //flights = flights.WhereIf(filter.GliderFlightDuration,
                //    flight => flight.FlightDuration.LocationName.Contains(filter.GliderFlightDuration));

                var flightsAndFlightCrews = flights.GroupJoin(flightCrews, f => f.FlightId, fcp => fcp.FlightId,
                        (f, fcp) => new { f, fcp })
                    .SelectMany(x => x.fcp.DefaultIfEmpty(), (f, fcp) => new FlightOverview
                    {
                        FlightId = f.f.FlightId,
                        FlightComment = f.f.Comment,
                        IsSoloFlight = f.f.IsSoloFlight,
                        PilotName = fcp.Pilot != null ? fcp.Pilot.Person.Lastname + " " + fcp.Pilot.Person.Firstname : null,
                        SecondCrewName =
                            fcp.SecondCrew != null
                                ? fcp.SecondCrew.Person.Lastname + " " + fcp.SecondCrew.Person.Firstname
                                : null,
                        FlightDurationInSeconds = DbFunctions.DiffSeconds(f.f.StartDateTime, f.f.LdgDateTime),
                        FlightCode = f.f.FlightType.FlightCode,
                        AirState = f.f.AirStateId,
                        ProcessState = f.f.ProcessStateId,
                        Immatriculation = f.f.Aircraft.Immatriculation,
                        StartType = f.f.StartTypeId,
                        FlightDate = f.f.FlightDate,
                        StartDateTime = f.f.StartDateTime,
                        LdgDateTime = f.f.LdgDateTime,
                        StartLocation = f.f.StartLocation.LocationName,
                        LdgLocation = f.f.LdgLocation.LocationName
                    }).OrderByPropertyNames(pageableSearchFilter.Sorting);

                var pagedQuery = new PagedQuery<FlightOverview>(flightsAndFlightCrews, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList();

                SetFlightOverviewSecurity(overviewList);

                var pagedList = new PagedList<FlightOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        #region MotorFlights
        public List<FlightOverview> GetMotorFlightOverviews()
        {
            var filter = new PageableSearchFilter<FlightOverviewSearchFilter>();

            var flights = GetPagedFlightOverview(0, 10000, filter, true);
            return flights.Items;
        }

        public List<FlightOverview> GetMotorFlightOverviewsWithinToday()
        {
            return GetMotorFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<FlightOverview> GetMotorFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var filter = new PageableSearchFilter<FlightOverviewSearchFilter>()
            {
                SearchFilter = new FlightOverviewSearchFilter()
                {
                    FlightDate = new DateTimeFilter()
                    {
                        From = fromDate,
                        To = toDate
                    }
                }
            };

            var flights = GetPagedFlightOverview(0, 10000, filter, true);
            return flights.Items;
        }
        #endregion MotorFlights
        
        #region GliderFlights
        public List<GliderFlightOverview> GetGliderFlightOverviews()
        {
            var filter = new PageableSearchFilter<GliderFlightOverviewSearchFilter>();

            var flights = GetPagedGliderFlightOverview(0, 10000, filter);
            return flights.Items;
        }

        public List<GliderFlightOverview> GetGliderFlightOverviewsWithinToday()
        {
            return GetGliderFlightOverviews(DateTime.Today, DateTime.Today);
        }

        public List<GliderFlightOverview> GetGliderFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var filter = new PageableSearchFilter<GliderFlightOverviewSearchFilter>()
            {
                SearchFilter = new GliderFlightOverviewSearchFilter() { 
                    FlightDate = new DateTimeFilter()
                    {
                        From = fromDate,
                        To = toDate
                    }
                }
            };

            var flights = GetPagedGliderFlightOverview(0, 10000, filter);
            return flights.Items;
        }
        
        public PagedList<GliderFlightOverview> GetPagedGliderFlightOverview(int? pageStart, int? pageSize, PageableSearchFilter<GliderFlightOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<GliderFlightOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new GliderFlightOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
            }
            else if (pageableSearchFilter.Sorting.Count == 1 && pageableSearchFilter.Sorting.ContainsKey("FlightDate"))
            {
                //when sorting for flight date only, we sort for StartDateTime as second to get more valuable result
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var includedFlightCrewTypes = new int[]
                {
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator
                };

                var flightCrews = context
                    .FlightCrews
                    .Include("Person")
                    .Where(fc => includedFlightCrewTypes.Contains(fc.FlightCrewTypeId))
                    .GroupBy(fc => fc.FlightId)
                    .Select(fc => new
                    {
                        FlightId = fc.Key,
                        Pilot = fc.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent),
                        SecondCrew = fc.OrderBy(ffc => ffc.FlightCrewTypeId).FirstOrDefault(ffc => 
                                                        ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger),
                        WinchOperator = fc.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator)
                    });

                var flights = context.Flights
                    .Include(x => x.FlightType)
                    .Where(f => f.OwnerId == CurrentAuthenticatedFLSUserClubId
                                && f.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight);


                var filter = pageableSearchFilter.SearchFilter;
                flights = flights.WhereIf(filter.Immatriculation,
                        flight => flight.Aircraft.Immatriculation.Contains(filter.Immatriculation));
                flights = flights.WhereIf(filter.PilotName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Firstname).Contains(filter.PilotName));
                flights = flights.WhereIf(filter.SecondCrewName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer).Person.Firstname).Contains(filter.SecondCrewName)
                    || (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger).Person.Firstname).Contains(filter.SecondCrewName));


                flights = flights.WhereIf(filter.FlightComment,
                    flight => flight.Comment.Contains(filter.FlightComment));

                if (filter.AirStates != null && filter.AirStates.Any()) flights = flights.Where(flight => filter.AirStates.Contains(flight.AirStateId));
                if (filter.ProcessStates != null && filter.ProcessStates.Any()) flights = flights.Where(flight => filter.ProcessStates.Contains(flight.ProcessStateId));
                
                flights = flights.WhereIf(filter.FlightCode,
                    flight => flight.FlightType.FlightCode.Contains(filter.FlightCode));

                if (filter.FlightDate != null)
                {
                    var dateTimeFilter = filter.FlightDate;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        flights = flights.Where(flight => flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >= DbFunctions.TruncateTime(from)
                            && DbFunctions.TruncateTime(flight.FlightDate) <= DbFunctions.TruncateTime(to));
                    }
                }

                //if (filter.LdgDateTime != null)
                //{
                //    var dateTimeFilter = filter.LdgDateTime;

                //    if (dateTimeFilter.Fixed.HasValue)
                //    {
                //        flights = flights.Where(flight => flight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.LdgDateTime) == DbFunctions.TruncateTime(dateTimeFilter.Fixed.Value));
                //    }
                //    else if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                //    {
                //        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                //        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                //        flights = flights.Where(flight => flight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.LdgDateTime) >= DbFunctions.TruncateTime(from)
                //            && DbFunctions.TruncateTime(flight.LdgDateTime) <= DbFunctions.TruncateTime(to));
                //    }
                //}

                if (filter.IsSoloFlight.HasValue)
                    flights = flights.Where(flight => flight.IsSoloFlight == filter.IsSoloFlight.Value);

                flights = flights.WhereIf(filter.StartType,
                    flight => flight.StartType.StartTypeName.Contains(filter.StartType));
                flights = flights.WhereIf(filter.StartLocation,
                    flight => flight.StartLocation.LocationName.Contains(filter.StartLocation));
                flights = flights.WhereIf(filter.LdgLocation,
                    flight => flight.LdgLocation.LocationName.Contains(filter.LdgLocation));

                //TODO: Search for flight duration
                //flights = flights.WhereIf(filter.GliderFlightDuration,
                //    flight => flight.FlightDuration.LocationName.Contains(filter.GliderFlightDuration));

                flights = flights.WhereIf(filter.WinchOperatorName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator).Person.Firstname).Contains(filter.WinchOperatorName));


                //TowFlight filtering
                flights = flights.WhereIf(filter.TowAircraftImmatriculation,
                        flight => flight.TowFlight.Aircraft.Immatriculation.Contains(filter.TowAircraftImmatriculation));
                flights = flights.WhereIf(filter.TowPilotName,
                    flight => (flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Lastname + " " + flight.FlightCrews.FirstOrDefault(x => x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Firstname).Contains(filter.TowPilotName));

                if (filter.TowFlightAirStates != null && filter.TowFlightAirStates.Any()) flights = flights.Where(flight => filter.TowFlightAirStates.Contains(flight.TowFlight.AirStateId));
                if (filter.TowFlightProcessStates != null && filter.TowFlightProcessStates.Any()) flights = flights.Where(flight => filter.TowFlightProcessStates.Contains(flight.TowFlight.ProcessStateId));
                
                //if (filter.TowFlightStartDateTime != null)
                //{
                //    var dateTimeFilter = filter.TowFlightStartDateTime;

                //    if (dateTimeFilter.Fixed.HasValue)
                //    {
                //        flights = flights.Where(flight => flight.TowFlight.StartDateTime.HasValue && DbFunctions.TruncateTime(flight.TowFlight.StartDateTime) == DbFunctions.TruncateTime(dateTimeFilter.Fixed.Value));
                //    }
                //    else if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                //    {
                //        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                //        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                //        flights = flights.Where(flight => flight.TowFlight.StartDateTime.HasValue && DbFunctions.TruncateTime(flight.TowFlight.StartDateTime) >= DbFunctions.TruncateTime(from)
                //            && DbFunctions.TruncateTime(flight.TowFlight.StartDateTime) <= DbFunctions.TruncateTime(to));
                //    }
                //}

                //if (filter.TowFlightLdgDateTime != null)
                //{
                //    var dateTimeFilter = filter.TowFlightLdgDateTime;

                //    if (dateTimeFilter.Fixed.HasValue)
                //    {
                //        flights = flights.Where(flight => flight.TowFlight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.TowFlight.LdgDateTime) == DbFunctions.TruncateTime(dateTimeFilter.Fixed.Value));
                //    }
                //    else if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                //    {
                //        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                //        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                //        flights = flights.Where(flight => flight.TowFlight.LdgDateTime.HasValue && DbFunctions.TruncateTime(flight.TowFlight.LdgDateTime) >= DbFunctions.TruncateTime(from)
                //            && DbFunctions.TruncateTime(flight.TowFlight.LdgDateTime) <= DbFunctions.TruncateTime(to));
                //    }
                //}

                flights = flights.WhereIf(filter.TowFlightStartLocation,
                    flight => flight.TowFlight.StartLocation.LocationName.Contains(filter.TowFlightStartLocation));
                flights = flights.WhereIf(filter.TowFlightLdgLocation,
                    flight => flight.TowFlight.LdgLocation.LocationName.Contains(filter.TowFlightLdgLocation));

                //TODO: Search for flight duration
                //flights = flights.WhereIf(filter.TowFlightDuration,
                //    flight => flight.TowFlight.FlightDuration.LocationName.Contains(filter.TowFlightDuration));


                var flightsAndFlightCrews = flights.GroupJoin(flightCrews, f => f.FlightId, fcp => fcp.FlightId,
                        (f, fcp) => new {f, fcp})
                    .SelectMany(x => x.fcp.DefaultIfEmpty(), (f, fcp) => new GliderFlightOverview
                    {
                        FlightId = f.f.FlightId,
                        FlightComment = f.f.Comment,
                        IsSoloFlight = f.f.IsSoloFlight,
                        PilotName = fcp.Pilot != null ? fcp.Pilot.Person.Lastname + " " + fcp.Pilot.Person.Firstname : null,
                        SecondCrewName =
                            fcp.SecondCrew != null
                                ? fcp.SecondCrew.Person.Lastname + " " + fcp.SecondCrew.Person.Firstname
                                : null,
                        FlightCode = f.f.FlightType.FlightCode,
                        AirState = f.f.AirStateId,
                        ProcessState = f.f.ProcessStateId,
                        Immatriculation = f.f.Aircraft.Immatriculation,
                        StartType = f.f.StartTypeId,
                        FlightDate = f.f.FlightDate,
                        StartDateTime = f.f.StartDateTime,
                        LdgDateTime = f.f.LdgDateTime,
                        GliderFlightDurationInSeconds = DbFunctions.DiffSeconds(f.f.StartDateTime, f.f.LdgDateTime),
                        TowFlightId = f.f.TowFlightId,
                        TowAircraftImmatriculation = f.f.TowFlight.Aircraft.Immatriculation,
                        TowFlightStartDateTime = f.f.TowFlight.StartDateTime,
                        TowFlightLdgDateTime = f.f.TowFlight.LdgDateTime,
                        TowFlightDurationInSeconds = DbFunctions.DiffSeconds(f.f.TowFlight.StartDateTime, f.f.TowFlight.LdgDateTime),
                        TowFlightStartLocation = f.f.TowFlight.StartLocation.LocationName,
                        TowFlightLdgLocation = f.f.TowFlight.LdgLocation.LocationName,
                        TowFlightAirState = f.f.TowFlight.AirStateId,
                        TowFlightProcessState = f.f.TowFlight.ProcessStateId,
                        TowPilotName = f.f.TowFlight.FlightCrews.Any(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent) ?
                                f.f.TowFlight.FlightCrews.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Lastname
                                + " " + f.f.TowFlight.FlightCrews.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Firstname : null,
                        WinchOperatorName = fcp.WinchOperator != null ? fcp.WinchOperator.Person.Lastname + " " + fcp.WinchOperator.Person.Firstname : null,
                        StartLocation = f.f.StartLocation.LocationName,
                        LdgLocation = f.f.LdgLocation.LocationName
                    }).OrderByPropertyNames(pageableSearchFilter.Sorting);

                var pagedQuery = new PagedQuery<GliderFlightOverview>(flightsAndFlightCrews, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList();

                SetGliderFlightOverviewSecurity(overviewList);

                var pagedList = new PagedList<GliderFlightOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
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
        /// Gets the flights which have been created today or have been modified since the flight was sent last time to the pilot.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        internal List<Flight> GetFlightsCreatedTodayOrModifiedSinceLastReportSentOn(Guid clubId)
        {
            DateTime today = DateTime.Now.Date;

            var flights = GetFlights(flight => DbFunctions.TruncateTime(flight.CreatedOn) == today.Date
                                                && (flight.FlightReportSentOn.HasValue == false
                                                    || (flight.ModifiedOn.HasValue 
                                                        && flight.FlightReportSentOn.Value < flight.ModifiedOn.Value))
                                                && flight.OwnerId == clubId 
                                                && (flight.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight
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
                //Flights which are not validated or are invalid and have been modified since last validation date
                var flightsToValidate = GetFlights(flight => flight.OwnerId == clubId 
                    && (flight.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.NotProcessed
                    || (flight.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.Invalid && flight.ModifiedOn.HasValue && flight.ValidatedOn.HasValue && (flight.ModifiedOn >= flight.ValidatedOn))));

                using (var context = _dataAccessService.CreateDbContext())
                {
                    var flightValidationStates = context.FlightProcessStates.ToList();

                    foreach (var flight in flightsToValidate)
                    {
                        context.Flights.Attach(flight);

                        flight.ValidateFlight();
                        flight.DoNotUpdateMetaData = true;

                        try
                        {
                            Logger.Info(
                            $"The currently validated flight {flight} has now the following Process-State: {flight.ProcessStateId} ({flightValidationStates.First(q => q.FlightProcessStateId == flight.ProcessStateId).FlightProcessStateName})");
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
                        flight => flight.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.Valid
                                    && flight.ProcessStateId < (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked
                                    && flight.OwnerId == clubId
                                   && (forceLockNow || DbFunctions.TruncateTime(flight.CreatedOn) <= lockingDate.Date));

                using (var context = _dataAccessService.CreateDbContext())
                {
                    foreach (var flight in flights)
                    {
                        context.Flights.Attach(flight);

                        flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked;
                        flight.DoNotUpdateMetaData = true;
                        Logger.Info($"The valid flight {flight} has now been locked.");
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
        

        public List<FlightExchangeData> GetFlightsModifiedSince(DateTime modifiedSince)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flights = context.Flights.AsNoTracking()
                            .Include(Constants.Aircraft).AsNoTracking()
                            .Include(Constants.FlightType).AsNoTracking()
                            .Include(Constants.FlightCrews).AsNoTracking()
                            .Include(Constants.FlightCrews + "." + Constants.Person).AsNoTracking()
                            .Include(Constants.FlightCrews + "." + Constants.Person + "." + Constants.PersonClubs).AsNoTracking()
                            .Include(Constants.StartType).AsNoTracking()
                            .Include(Constants.StartLocation).AsNoTracking()
                            .Include(Constants.LdgLocation).AsNoTracking()
                            .Include(Constants.TowFlight).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.Aircraft).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.FlightType).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.StartLocation).AsNoTracking()
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation).AsNoTracking()
                    .Where(flight => flight.OwnerId == CurrentAuthenticatedFLSUserClubId &&
                            (flight.CreatedOn >= modifiedSince
                                                || (flight.ModifiedOn.HasValue &&
                                                flight.ModifiedOn.Value >= modifiedSince))
                                               &&
                                              (flight.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight
                                               || flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight))
                    .OrderByDescending(c => c.StartDateTime)
                    .ToList();
                
                var flightExchangeDatas = flights.Select(x => x.ToFlightExchangeData(CurrentAuthenticatedFLSUserClubId)).ToList();

                return flightExchangeDatas;
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

        public void SetFlightReportSent(List<Flight> flights, DateTime flightReportSentOn)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var flight in flights)
                {
                    context.Flights.Attach(flight);
                    flight.FlightReportSentOn = flightReportSentOn;
                    flight.DoNotUpdateMetaData = true;
                }

                context.SaveChanges();
            }
        }
    }
}
