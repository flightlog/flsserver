﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting.Flights;
using FLS.Server.Data.Enums;
using NLog;

namespace FLS.Server.Service.Reporting
{
    public class FlightReportService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public FlightReportService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }
        
        public FlightReportResult GetPagedFlightReport(int? pageStart, int? pageSize, PageableSearchFilter<FlightReportFilterCriteria> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<FlightReportFilterCriteria>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new FlightReportFilterCriteria();

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "FlightDuration")
                {
                    pageableSearchFilter.Sorting.Add("FlightDurationInSeconds", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "TowFlight.FlightDuration")
                {
                    pageableSearchFilter.Sorting.Add("TowFlight.FlightDurationInSeconds", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
                pageableSearchFilter.Sorting.Add("Immatriculation", "asc");     //better sorting when combining TowFlights and GliderFlights
            }
            else if (pageableSearchFilter.Sorting.Count == 1 && pageableSearchFilter.Sorting.ContainsKey("FlightDate"))
            {
                //when sorting for flight date only, we sort for StartDateTime as second to get more valuable result
                pageableSearchFilter.Sorting.Add("StartDateTime", "asc");
                pageableSearchFilter.Sorting.Add("Immatriculation", "asc");     //better sorting when combining TowFlights and GliderFlights
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var filter = pageableSearchFilter.SearchFilter;

                var includedFlightCrewTypes = new int[]
                {
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger
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
                                                        || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger)
                    });

                var flights = context.Flights
                    .Where(f => (filter.GliderFlights && f.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
                                || (filter.MotorFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight)
                                || (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

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
                
                flights = flights.WhereIf(filter.LocationId.HasValue,
                    flight => flight.StartLocationId == filter.LocationId.Value || flight.LdgLocationId == filter.LocationId.Value);

                //check also if we have to restrict to the club owned data when selecting a location
                if (filter.LocationId.HasValue)
                {
                    var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                    if (club != null 
                        && club.HomebaseId.HasValue
                        && club.HomebaseId.Value != filter.LocationId.Value)
                    {
                        //we have to restrict the flights of other locations to our own club related flight data
                        flights = flights.Where(flight => flight.OwnerId == club.ClubId);
                    }
                }

                //filter only flights where person is Pilot, Copilot or instructor
                flights = flights.WhereIf(filter.FlightCrewPersonId.HasValue,
                    flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                          && (x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent
                                                              || x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot
                                                              || x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor)));

                var flightsAndFlightCrews = flights.GroupJoin(flightCrews, f => f.FlightId, fcp => fcp.FlightId,
                        (f, fcp) => new {f, fcp})
                    .SelectMany(x => x.fcp.DefaultIfEmpty(), (f, fcp) => new FlightReportDataRecord
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
                        FlightTypeName = f.f.FlightType.FlightTypeName,
                        AirState = f.f.AirStateId,
                        ProcessState = f.f.ProcessStateId,
                        Immatriculation = f.f.Aircraft.Immatriculation,
                        StartType = f.f.StartTypeId,
                        FlightDate = f.f.FlightDate,
                        StartDateTime = f.f.StartDateTime,
                        LdgDateTime = f.f.LdgDateTime,
                        StartLocation = f.f.StartLocation.LocationName,
                        LdgLocation = f.f.LdgLocation.LocationName,
                        FlightDurationInSeconds = DbFunctions.DiffSeconds(f.f.StartDateTime, f.f.LdgDateTime),
                        TowFlight = f.f.TowFlightId.HasValue ? new TowFlightReportDataRecord() {  
                            TowFlightId = f.f.TowFlight.FlightId,
                            Immatriculation = f.f.TowFlight.Aircraft.Immatriculation,
                            FlightCode = f.f.TowFlight.FlightType.FlightCode,
                            FlightTypeName = f.f.TowFlight.FlightType.FlightTypeName,
                            StartDateTime = f.f.TowFlight.StartDateTime,
                            LdgDateTime = f.f.TowFlight.LdgDateTime,
                            StartLocation = f.f.TowFlight.StartLocation.LocationName,
                            LdgLocation = f.f.TowFlight.LdgLocation.LocationName,
                            FlightDurationInSeconds = DbFunctions.DiffSeconds(f.f.TowFlight.StartDateTime, f.f.TowFlight.LdgDateTime),
                            AirState = f.f.TowFlight.AirStateId,
                            ProcessState = f.f.TowFlight.ProcessStateId,
                            PilotName = f.f.TowFlight.FlightCrews.Any(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent) ?
                                    f.f.TowFlight.FlightCrews.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Lastname
                                    + " " + f.f.TowFlight.FlightCrews.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent).Person.Firstname : null
                        } : null,
                        FlightCategory = f.f.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight ? FlightCategory.GliderFlight :
                            f.f.FlightAircraftType == (int)FlightCategory.TowFlight ? FlightCategory.TowFlight :
                            f.f.FlightAircraftType == (int)FlightCategory.MotorFlight ? FlightCategory.MotorFlight :
                            f.f.FlightAircraftType == (int)FlightCategory.Other ? FlightCategory.Other : FlightCategory.Unknown,
                        TowedGliderFlightId = f.f.TowedFlights.Any() ? f.f.TowedFlights.FirstOrDefault().FlightId : Guid.Empty
                    }).OrderByPropertyNames(pageableSearchFilter.Sorting);

                var pagedQuery = new PagedQuery<FlightReportDataRecord>(flightsAndFlightCrews, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList();

                var pagedList = new PagedList<FlightReportDataRecord>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                var flightReportSummaries = new List<FlightReportSummary>();

                if (filter.FlightCrewPersonId.HasValue)
                {
                    #region Flight summary for Pilot function

                    var flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.GliderFlights &&
                                     f.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int) FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.PilotOrStudent));

                    var summary = flightSummary.GroupBy(f => new {f.RecordState})
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Pilot (Glider)",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlights = x.Sum(f => 1),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => ((filter.MotorFlights &&
                                        f.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight)));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.PilotOrStudent));

                    summary = flightSummary.GroupBy(f => new { f.RecordState })
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Pilot (Motor)",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.PilotOrStudent));

                    summary = flightSummary.GroupBy(f => new { f.RecordState })
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Pilot (Towing)",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    #endregion Flight summary for Pilot function

                    #region Flight summary for CoPilot function

                    flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.GliderFlights &&
                                     f.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
                                    || (filter.MotorFlights &&
                                        f.FlightAircraftType == (int) FlightAircraftTypeValue.MotorFlight)
                                    || (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot, Copilot or instructor
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int) FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.CoPilot));

                    summary = flightSummary.GroupBy(f => new {f.RecordState})
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Copilot",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlights = x.Sum(f => 1),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    #endregion Flight summary for CoPilot function

                    #region Flight summary for Instructor double seat function

                    flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.GliderFlights &&
                                     f.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
                                    || (filter.MotorFlights &&
                                        f.FlightAircraftType == (int) FlightAircraftTypeValue.MotorFlight)
                                    || (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot, Copilot or instructor
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int) FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.FlightInstructor));

                    //don't count teached solo flights of a trainee in summaries
                    flightSummary = flightSummary.Where(f => f.IsSoloFlight == false);


                    summary = flightSummary.GroupBy(f => new {f.RecordState})
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Instructor",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlights = x.Sum(f => 1),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    #endregion Flight summary for Instructor double seat function

                    #region Flight summary for Instructor function on solo flights

                    flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.GliderFlights &&
                                     f.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight)
                                    || (filter.MotorFlights &&
                                        f.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight)
                                    || (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot, Copilot or instructor
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.FlightInstructor));

                    //count only teached solo flights of a trainee
                    flightSummary = flightSummary.Where(f => f.IsSoloFlight);


                    summary = flightSummary.GroupBy(f => new { f.RecordState })
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = "Instructor (Soloflights)",
                            TotalLdgs = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0)
                                        + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalStarts = x.Sum(f => f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0)
                                          + x.Sum(f => f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0),
                            TotalFlights = x.Sum(f => 1),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        flightReportSummaries.Add(summary.First());
                    }

                    #endregion Flight summary for Instructor function on solo flights

                    var totalSummary = new FlightReportSummary()
                    {
                        GroupBy = "Total",
                        TotalFlightDurationInSeconds = 0,
                        TotalLdgs = 0,
                        TotalStarts = 0,
                        TotalFlights = 0
                    };

                    foreach (var sum in flightReportSummaries)
                    {
                        totalSummary.TotalFlightDurationInSeconds +=
                            sum.TotalFlightDurationInSeconds.GetValueOrDefault(0);
                        totalSummary.TotalLdgs += sum.TotalLdgs;
                        totalSummary.TotalStarts += sum.TotalStarts;
                        totalSummary.TotalFlights += sum.TotalFlights;
                    }

                    flightReportSummaries.Add(totalSummary);
                }
                else if (filter.LocationId.HasValue)
                {
                    #region Flight summary for Location function

                    var flightSummary = context.Flights
                        .Include(x => x.FlightType)
                        .Where(f => (filter.GliderFlights &&
                                     f.FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight)
                                    || (filter.MotorFlights &&
                                        f.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight)
                                    || (filter.TowFlights && f.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight));

                    if (filter.FlightDate != null)
                    {
                        var dateTimeFilter = filter.FlightDate;

                        if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                        {
                            var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                            var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                            flightSummary = flightSummary.Where(flight =>
                                flight.FlightDate.HasValue && DbFunctions.TruncateTime(flight.FlightDate) >=
                                                           DbFunctions.TruncateTime(from)
                                                           && DbFunctions.TruncateTime(flight.FlightDate) <=
                                                           DbFunctions.TruncateTime(to));
                        }
                    }

                    flightSummary = flightSummary.WhereIf(filter.LocationId.HasValue,
                        flight => flight.StartLocationId == filter.LocationId.Value ||
                                  flight.LdgLocationId == filter.LocationId.Value);

                    //check also if we have to restrict to the club owned data when selecting a location
                    if (filter.LocationId.HasValue)
                    {
                        var club = context.Clubs.FirstOrDefault(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                        if (club != null
                            && club.HomebaseId.HasValue
                            && club.HomebaseId.Value != filter.LocationId.Value)
                        {
                            //we have to restrict the flights of other locations to our own club related flight data
                            flightSummary = flightSummary.Where(flight => flight.OwnerId == club.ClubId);
                        }
                    }

                    //filter only flights where person is Pilot, Copilot or instructor
                    flightSummary = flightSummary.WhereIf(filter.FlightCrewPersonId.HasValue,
                        flight => flight.FlightCrews.Any(x => x.PersonId == filter.FlightCrewPersonId.Value
                                                              && x.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight
                                                                  .FlightCrewType.PilotOrStudent));

                    var summary = flightSummary.GroupBy(f => f.FlightType)
                        .Select(x => new FlightReportSummary()
                        {
                            GroupBy = x.Key.FlightTypeName,
                            TotalLdgs = x.Sum(f => f.LdgLocationId == filter.LocationId.Value ? f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoLdgTimeInformation ? 1 : 0 : 0)
                                        + x.Sum(f => f.StartLocationId == filter.LocationId.Value ? f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0 : 0),
                            TotalStarts = x.Sum(f => (f.StartLocationId == filter.LocationId.Value && f.LdgLocationId == filter.LocationId.Value) ? f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : f.NoStartTimeInformation ? 1 : 0 : 0)
                                          + x.Sum(f => (f.StartLocationId != filter.LocationId.Value && f.LdgLocationId == filter.LocationId.Value) ? f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value - 1 : 0 : 0)
                                          + x.Sum(f => (f.StartLocationId == filter.LocationId.Value && f.LdgLocationId != filter.LocationId.Value) ? f.NrOfLdgs.HasValue ? f.NrOfLdgs.Value : 0 : 0)
                                          + x.Sum(f => f.StartLocationId == filter.LocationId.Value ? f.NrOfLdgsOnStartLocation.HasValue ? f.NrOfLdgsOnStartLocation.Value : 0 : 0),
                            TotalFlights = x.Sum(f => 1),
                            TotalFlightDurationInSeconds =
                                x.Sum(f => DbFunctions.DiffSeconds(f.StartDateTime, f.LdgDateTime))
                        }).ToList();

                    if (summary.Any())
                    {
                        var totalSummary = new FlightReportSummary()
                        {
                            GroupBy = "Total",
                            TotalFlightDurationInSeconds = 0,
                            TotalLdgs = 0
                        };

                        foreach (var sum in summary)
                        {
                            totalSummary.TotalFlightDurationInSeconds +=
                                sum.TotalFlightDurationInSeconds.GetValueOrDefault(0);
                            totalSummary.TotalLdgs += sum.TotalLdgs;
                            totalSummary.TotalStarts += sum.TotalStarts;
                            totalSummary.TotalFlights += sum.TotalFlights;
                        }

                        flightReportSummaries.AddRange(summary.OrderBy(x => x.GroupBy));
                        flightReportSummaries.Add(totalSummary);
                    }
                    else
                    {
                        var totalSummary = new FlightReportSummary()
                        {
                            GroupBy = "Total",
                            TotalFlightDurationInSeconds = 0,
                            TotalLdgs = 0,
                            TotalStarts = 0,
                            TotalFlights = 0
                        };

                        flightReportSummaries.Add(totalSummary);
                    }

                    #endregion Flight summary for Location function
                }

                var flightReportResult = new FlightReportResult()
                {
                    FlightReportFilterCriteria = filter,
                    Flights = pagedList,
                    FlightReportSummaries = flightReportSummaries
                };

                return flightReportResult;
            }
        }
    }
}
