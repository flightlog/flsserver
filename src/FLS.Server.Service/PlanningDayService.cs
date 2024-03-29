﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Mapping;
using LinqKit;
using NLog;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace FLS.Server.Service
{
    public class PlanningDayService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly AircraftReservationService _aircraftReservationService;

        public PlanningDayService(DataAccessService dataAccessService,
            AircraftReservationService aircraftReservationService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _aircraftReservationService = aircraftReservationService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region PlanningDay
        /// <summary>
        /// Gets the planning days from the current date to the future and for the current users club only.
        /// </summary>
        /// <returns></returns>
        public List<PlanningDayOverview> GetPlanningDayOverview()
        {
            return GetPlanningDayOverview(DateTime.MinValue);
        }

        /// <summary>
        /// Gets the planning days from the current date to the future and for the current users club only.
        /// </summary>
        /// <returns></returns>
        public List<PlanningDayOverview> GetPlanningDayOverview(DateTime fromDate, Guid? clubId = null)
        {
            if (clubId.HasValue == false)
            {
                clubId = CurrentAuthenticatedFLSUserClubId;
            }

            var entities = GetPlanningDays(clubId.Value, fromDate);
            var overviewList = entities.Select(e => e.ToPlanningDayOverview()).ToList();

            foreach (var planningDayOverview in overviewList)
            {
                var reservations = _aircraftReservationService.GetAircraftReservationsByPlanningDayId(planningDayOverview.PlanningDayId);
                planningDayOverview.NumberOfAircraftReservations = reservations.Count;
            }

            SetPlanningDayOverviewSecurity(overviewList);
            return overviewList;
        }
        
        public PagedList<PlanningDayOverview> GetPagedPlanningDayOverview(int? pageStart, int? pageSize, PageableSearchFilter<PlanningDayOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<PlanningDayOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new PlanningDayOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("Day", "asc");
            }

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "LocationName")
                {
                    pageableSearchFilter.Sorting.Add("Location.LocationName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "TowingPilotName" || sort == "FlightOperatorName" || sort == "InstructorName" || sort == "NumberOfAircraftReservations" || sort == "OnlyPlanningDaysInFuture")
                {
                    //TODO: Add ability to sort for conditional columns
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("Day", "asc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                // **************************************************************************************************************************
                // SQL Query
                // **************************************************************************************************************************
                //DECLARE @clubId as uniqueidentifier
                //SET @clubId = (SELECT TOP 1 ClubId FROM Clubs WHERE ClubKey = 'FGZO')

                //SELECT PlanningDays.PlanningDayId, PlanningDays.Day, Locations.LocationName, PlanningDays.Remarks, 
                //				Persons.Lastname, Persons.Firstname, 
                //				PlanningDayAssignmentTypes.AssignmentTypeName,                          
                //						 COUNT(AircraftReservations.AircraftId) AS NrOfReservations
                //FROM PlanningDayAssignmentTypes INNER JOIN
                //                         PlanningDayAssignments ON PlanningDayAssignmentTypes.PlanningDayAssignmentTypeId = PlanningDayAssignments.AssignmentTypeId INNER JOIN
                //                         PlanningDays ON PlanningDayAssignments.AssignedPlanningDayId = PlanningDays.PlanningDayId INNER JOIN
                //                         Persons ON PlanningDayAssignments.AssignedPersonId = Persons.PersonId INNER JOIN
                //                         Locations ON PlanningDays.LocationId = Locations.LocationId

                //                         LEFT JOIN AircraftReservations ON AircraftReservations.ClubId = @clubId

                //                            AND AircraftReservations.LocationId = PlanningDays.LocationId

                //                            AND CAST(AircraftReservations.Start AS date) = PlanningDays.Day

                //where PlanningDays.ClubId = @clubId
                //and PlanningDays.Day < '2017-01-01'
                //GROUP by PlanningDays.PlanningDayId, PlanningDays.Day, Locations.LocationName, PlanningDays.Remarks, 
                //				Persons.Lastname, Persons.Firstname, 
                //				PlanningDayAssignmentTypes.AssignmentTypeName
                //HAVING COUNT(AircraftReservations.AircraftId) > 0
                // **************************************************************************************************************************
                // SQL Query END
                // **************************************************************************************************************************

                var planningDays = context.PlanningDays
                    .Include("Location")
                    .Include("PlanningDayAssignments")
                    .Include("PlanningDayAssignments.AssignmentType")
                    .Include("PlanningDayAssignments.AssignedPerson")
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                var filter = pageableSearchFilter.SearchFilter;
                planningDays = planningDays.WhereIf(filter.LocationName,
                    x => x.Location.LocationName.Contains(filter.LocationName));

                planningDays = planningDays.WhereIf(filter.FlightOperatorName,
                    x => x.PlanningDayAssignments.Any(y => (y.AssignedPerson.Lastname.Contains(filter.FlightOperatorName)
                    || y.AssignedPerson.Firstname.Contains(filter.FlightOperatorName))
                    && y.AssignmentType.AssignmentTypeName.ToLower() == "segelflugleiter"));
                planningDays = planningDays.WhereIf(filter.TowingPilotName,
                    x => x.PlanningDayAssignments.Any(y => (y.AssignedPerson.Lastname.Contains(filter.TowingPilotName)
                    || y.AssignedPerson.Firstname.Contains(filter.TowingPilotName))
                    && y.AssignmentType.AssignmentTypeName.ToLower() == "schlepppilot"));
                planningDays = planningDays.WhereIf(filter.InstructorName,
                    x => x.PlanningDayAssignments.Any(y => (y.AssignedPerson.Lastname.Contains(filter.InstructorName)
                    || y.AssignedPerson.Firstname.Contains(filter.InstructorName))
                    && y.AssignmentType.AssignmentTypeName.ToLower() == "fluglehrer"));

                planningDays = planningDays.WhereIf(filter.Remarks,
                    x => x.Remarks.Contains(filter.Remarks));

                if (filter.Day != null)
                {
                    var dateTimeFilter = filter.Day;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        planningDays =
                            planningDays.Where(planningDay => DbFunctions.TruncateTime(planningDay.Day) >= DbFunctions.TruncateTime(from)
                                                              && DbFunctions.TruncateTime(planningDay.Day) <= DbFunctions.TruncateTime(to));
                    }
                }

                planningDays = planningDays.OrderByPropertyNames(pageableSearchFilter.Sorting);

                var planningDayOverviews = planningDays.Select(p => new PlanningDayOverview
                {
                    Day = p.Day,
                    PlanningDayId = p.PlanningDayId,
                    LocationId = p.LocationId,
                    LocationName = p.Location.LocationName,
                    Remarks = p.Remarks,
                    FlightOperatorName =
                        p.PlanningDayAssignments.Where(
                                x =>
                                    x.AssignmentType.AssignmentTypeName.ToLower() == "segelflugleiter" &&
                                    x.AssignedPerson != null)
                            .Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname)
                            .FirstOrDefault(),
                    TowingPilotName =
                        p.PlanningDayAssignments.Where(
                                x =>
                                    x.AssignmentType.AssignmentTypeName.ToLower() == "schlepppilot" &&
                                    x.AssignedPerson != null)
                            .Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname)
                            .FirstOrDefault(),
                    InstructorName =
                        p.PlanningDayAssignments.Where(
                                x =>
                                    x.AssignmentType.AssignmentTypeName.ToLower() == "fluglehrer" &&
                                    x.AssignedPerson != null)
                            .Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname)
                            .FirstOrDefault()
                }).ToList();

                var reservations = context.AircraftReservations.Where(r => r.ClubId == CurrentAuthenticatedFLSUserClubId).GroupBy(r => new { r.LocationId, Day = DbFunctions.TruncateTime(r.Start) }).Select(r => new { r.Key, Count = r.Count() }).ToList();

                foreach (var planningDay in planningDayOverviews)
                {
                    var countInfo = reservations.FirstOrDefault(r => r.Key.Day == planningDay.Day.Date && r.Key.LocationId == planningDay.LocationId);
                    planningDay.NumberOfAircraftReservations = countInfo?.Count ?? 0;
                }

                var planningDayOverviewQuery = planningDayOverviews.AsQueryable();
                planningDayOverviewQuery = planningDayOverviewQuery.WhereIf(filter.NumberOfAircraftReservations,
                    x => x.NumberOfAircraftReservations.ToString().Contains(filter.NumberOfAircraftReservations));

                var pagedQuery = new PagedQuery<PlanningDayOverview>(planningDayOverviewQuery, pageStart, pageSize);
                
                var pagedList = new PagedList<PlanningDayOverview>(pagedQuery.Items.ToList(), pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                SetPlanningDayOverviewSecurity(pagedList.Items);

                return pagedList;
            }
        }
        
        public PlanningDayDetails GetPlanningDayDetails(Guid planningDayId)
        {
            var planningDay = GetPlanningDay(planningDayId);
            var details = planningDay.ToPlanningDayDetails();
            SetPlanningDayDetailsSecurity(details, planningDay);
            return details;
        }

        internal PlanningDay GetPlanningDay(Guid planningDayId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var planningDay = context.PlanningDays
                    .Include("Location")
                    .Include("PlanningDayAssignments")
                    .Include("PlanningDayAssignments.AssignmentType")
                    .Include("PlanningDayAssignments.AssignedPerson")
                    .FirstOrDefault(p => p.PlanningDayId == planningDayId);

                return planningDay;
            }
        }

        /// <summary>
        /// Gets the planning days from the current date to the future and for the current users club only.
        /// </summary>
        /// <returns></returns>
        internal List<PlanningDay> GetPlanningDays(DateTime fromDate)
        {
            return GetPlanningDays(CurrentAuthenticatedFLSUserClubId, fromDate);
        }

        /// <summary>
        /// Gets the planning days from the current date to the future for the club.
        /// </summary>
        /// <returns></returns>
        internal List<PlanningDay> GetPlanningDays(Guid clubId, DateTime fromDate)
        {
            clubId.NotNullOrEmptyGuid("clubId");

            using (var context = _dataAccessService.CreateDbContext())
            {
                List<PlanningDay> entities = null;
                entities = context.PlanningDays
                    .Include("Location")
                    .Include("PlanningDayAssignments")
                    .Include("PlanningDayAssignments.AssignmentType")
                    .Include("PlanningDayAssignments.AssignedPerson")
                    .Where(q => q.ClubId == clubId && DbFunctions.TruncateTime(q.Day) >= fromDate.Date)
                    .OrderBy(pe => pe.Day)
                    .ToList();

                return entities;
            }
        }
        public List<PlanningDayOverview> CreatePlanningDays(PlanningDayCreatorRule planningDayCreatorRule)
        {
            planningDayCreatorRule.ArgumentNotNull("planningDayCreatorRule");

            if (planningDayCreatorRule.EveryMonday == false
                && planningDayCreatorRule.EveryTuesday == false
                && planningDayCreatorRule.EveryWednesday == false
                && planningDayCreatorRule.EveryThursday == false
                && planningDayCreatorRule.EveryFriday == false
                && planningDayCreatorRule.EverySaturday == false
                && planningDayCreatorRule.EverySunday == false)
            {
                //Rule without any selected weekday, nothing to store
                return new List<PlanningDayOverview>();
            }

            var planningDayList = new List<PlanningDay>();

            for (var date = planningDayCreatorRule.StartDate.Date; date.Date <= planningDayCreatorRule.EndDate.Date; date = date.AddDays(1))
            {
                if ((planningDayCreatorRule.EveryMonday && date.DayOfWeek == DayOfWeek.Monday)
                    || (planningDayCreatorRule.EveryTuesday && date.DayOfWeek == DayOfWeek.Tuesday)
                    || (planningDayCreatorRule.EveryWednesday && date.DayOfWeek == DayOfWeek.Wednesday)
                    || (planningDayCreatorRule.EveryThursday && date.DayOfWeek == DayOfWeek.Thursday)
                    || (planningDayCreatorRule.EveryFriday && date.DayOfWeek == DayOfWeek.Friday)
                    || (planningDayCreatorRule.EverySaturday && date.DayOfWeek == DayOfWeek.Saturday)
                    || (planningDayCreatorRule.EverySunday && date.DayOfWeek == DayOfWeek.Sunday))
                {
                    var planningDay = new PlanningDay
                    {
                        LocationId = planningDayCreatorRule.LocationId,
                        Remarks = planningDayCreatorRule.Remarks,
                        Day = date.Date,
                        ClubId = CurrentAuthenticatedFLSUserClubId
                    };

                    planningDayList.Add(planningDay);
                }
            }

            InsertPlanningDays(planningDayList);

            var overviewList = planningDayList.Select(e => e.ToPlanningDayOverview()).ToList();

            SetPlanningDayOverviewSecurity(overviewList);
            return overviewList;
        }

        public List<PlanningDayDetails> CreatePlanningDays(List<PlanningDayDetails> planningDays)
        {
            var planningDayAssignmentTypes = GetPlanningDayAssignmentTypes();

            var planningDayList = planningDays.Select(planningDayDetails => planningDayDetails.ToPlanningDay(CurrentAuthenticatedFLSUserClubId, planningDayAssignmentTypes)).ToList();

            InsertPlanningDays(planningDayList);

            var detailList = new List<PlanningDayDetails>();

            foreach (var planningDay in planningDayList)
            {
                var detail = planningDay.ToPlanningDayDetails();
                SetPlanningDayDetailsSecurity(detail, planningDay);
                detailList.Add(detail);
            }

            return detailList;
        }

        public void InsertPlanningDayDetails(PlanningDayDetails planningDayDetails)
        {
            var planningDayAssignmentTypes = GetPlanningDayAssignmentTypes();
            var planningDay = planningDayDetails.ToPlanningDay(CurrentAuthenticatedFLSUserClubId, planningDayAssignmentTypes);

            InsertPlanningDay(planningDay);

            //Map it back to details
            planningDay.ToPlanningDayDetails(planningDayDetails);
        }

        internal void InsertPlanningDays(List<PlanningDay> planningDays)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                context.PlanningDays.AddRange(planningDays);
                context.SaveChanges();
            }
        }

        internal void InsertPlanningDay(PlanningDay planningDay)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                context.PlanningDays.Add(planningDay);
                context.SaveChanges();
            }
        }

        public void UpdatePlanningDayDetails(PlanningDayDetails currentPlanningDayDetails)
        {
            currentPlanningDayDetails.ArgumentNotNull("currentPlanningDayDetails");
            var original = GetPlanningDay(currentPlanningDayDetails.PlanningDayId);
            original.EntityNotNull("PlanningDay", currentPlanningDayDetails.PlanningDayId);

            var planningDayAssignmentTypes = GetPlanningDayAssignmentTypes();

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.PlanningDays.Attach(original);
                currentPlanningDayDetails.ToPlanningDay(CurrentAuthenticatedFLSUserClubId, planningDayAssignmentTypes,
                                                        original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    
                    //Map it back to details
                    original.ToPlanningDayDetails(currentPlanningDayDetails);
                }
            }
        }

        public void DeletePlanningDayDetails(Guid planningDayId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.PlanningDays.FirstOrDefault(l => l.PlanningDayId == planningDayId);
                original.EntityNotNull("PlanningDay", planningDayId);

                if (IsCurrentUserInRoleClubAdministrator ||
                    IsCreator(original))
                {
                    context.PlanningDays.Remove(original);
                    context.SaveChanges();
                }
                else
                {
                    throw new UnauthorizedAccessException("Current user is not club admin or owner of this record to delete the planning day.");
                }
            }
        }
        #endregion PlanningDay

        #region PlanningDayAssignmentType
        
        /// <summary>
        /// Gets the planning days assignment types from the current users club.
        /// </summary>
        /// <returns></returns>
        internal List<PlanningDayAssignmentType> GetPlanningDayAssignmentTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<PlanningDayAssignmentType> entities = null;
                entities = context.PlanningDayAssignmentTypes.Where(r => r.ClubId == CurrentAuthenticatedFLSUserClubId).OrderBy(pe => pe.AssignmentTypeName).ToList();

                return entities;
            }
        }
        #endregion PlanningDayAssignmentType
        
        #region Security
        private void SetPlanningDayOverviewSecurity(IEnumerable<PlanningDayOverview> list)
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
                foreach (var overview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator 
                        || IsCreator(context.PlanningDays.First(a => a.PlanningDayId == overview.PlanningDayId)))
                    {
                        overview.CanUpdateRecord = true;
                        overview.CanDeleteRecord = true;
                    }
                    else
                    {
                        overview.CanUpdateRecord = false;
                        overview.CanDeleteRecord = false;
                    }

                    if (IsOwner(context.PlanningDays.First(a => a.PlanningDayId == overview.PlanningDayId)))
                    {
                        overview.CanUpdateRecord = true;
                    }
                }
            }
        }

        private void SetPlanningDayDetailsSecurity(PlanningDayDetails details, PlanningDay planningDay)
        {
            if (details == null)
            {
                Logger.Error(string.Format("PlanningDayDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || IsCreator(planningDay))
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }

            if (IsOwner(planningDay))
            {
                details.CanUpdateRecord = true;
            }
        }
        
        #endregion Security
    }
}
