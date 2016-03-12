﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using NLog;

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

                context.PlanningDays.Remove(original);
                context.SaveChanges();
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
                    if (IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.PlanningDays.First(a => a.PlanningDayId == overview.PlanningDayId)))
                    {
                        overview.CanUpdateRecord = true;
                        overview.CanDeleteRecord = true;
                    }
                    else
                    {
                        overview.CanUpdateRecord = false;
                        overview.CanDeleteRecord = false;
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

            if (IsCurrentUserInRoleClubAdministrator || IsOwner(planningDay))
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
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
