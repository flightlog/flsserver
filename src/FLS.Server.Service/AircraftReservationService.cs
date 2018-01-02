using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using NLog;

namespace FLS.Server.Service
{
    public class AircraftReservationService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public AircraftReservationService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region AircraftReservation
        public List<AircraftReservationOverview> GetAircraftReservationOverview()
        {
            var entities = GetAircraftReservations(DateTime.MinValue);
            var overviewList = entities.Select(entity => entity.ToAircraftReservationOverview()).ToList();

            SetAircraftReservationOverviewSecurity(overviewList);
            return overviewList;
        }

        public List<AircraftReservationOverview> GetAircraftReservationOverview(DateTime fromDate)
        {
            var entities = GetAircraftReservations(fromDate);
            var overviewList = entities.Select(entity => entity.ToAircraftReservationOverview()).ToList();
            SetAircraftReservationOverviewSecurity(overviewList);
            return overviewList;
        }

        public List<AircraftReservationOverview> GetAircraftReservationOverviewOfDay(DateTime day)
        {
            var entities = GetAircraftReservationsOfDay(day);
            var overviewList = entities.Select(entity => entity.ToAircraftReservationOverview()).ToList();
            SetAircraftReservationOverviewSecurity(overviewList);
            return overviewList;
        }

        public PagedList<AircraftReservationOverview> GetPagedAircraftReservationOverview(int? pageStart, int? pageSize, PageableSearchFilter<AircraftReservationOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<AircraftReservationOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new AircraftReservationOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "Immatriculation")
                {
                    pageableSearchFilter.Sorting.Add("Aircraft.Immatriculation", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "PilotName")
                {
                    pageableSearchFilter.Sorting.Add("PilotPerson.Lastname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Add("PilotPerson.Firstname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "LocationName")
                {
                    pageableSearchFilter.Sorting.Add("Location.LocationName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "InstructorName")
                {
                    pageableSearchFilter.Sorting.Add("InstructorPerson.Lastname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Add("InstructorPerson.Firstname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "ReservationTypeName")
                {
                    pageableSearchFilter.Sorting.Add("ReservationType.AircraftReservationTypeName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var reservations = context.AircraftReservations
                    .Include(Constants.Aircraft)
                    .Include("PilotPerson")
                    .Include("Location")
                    .Include("InstructorPerson")
                    .Include("ReservationType")
                    .Where(r => r.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                reservations = reservations.WhereIf(filter.Immatriculation,
                        reservation => reservation.Aircraft.Immatriculation.Contains(filter.Immatriculation));

                if (filter.Start != null)
                {
                    var dateTimeFilter = filter.Start;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        reservations = reservations.Where(reservation => DbFunctions.TruncateTime(reservation.Start) >= DbFunctions.TruncateTime(from)
                            && DbFunctions.TruncateTime(reservation.Start) <= DbFunctions.TruncateTime(to));
                    }
                }

                if (filter.End != null)
                {
                    var dateTimeFilter = filter.End;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        reservations = reservations.Where(reservation => DbFunctions.TruncateTime(reservation.End) >= DbFunctions.TruncateTime(from)
                            && DbFunctions.TruncateTime(reservation.End) <= DbFunctions.TruncateTime(to));
                    }
                }
                
                reservations = reservations.WhereIf(filter.LocationName,
                    reservation => reservation.Location.LocationName.Contains(filter.LocationName));
                reservations = reservations.WhereIf(filter.PilotName,
                    reservation => reservation.PilotPerson.Lastname.Contains(filter.PilotName)
                                   || reservation.PilotPerson.Firstname.Contains(filter.PilotName));
                reservations = reservations.WhereIf(filter.SecondCrewName,
                    reservation => reservation.SecondCrewPerson.Lastname.Contains(filter.SecondCrewName)
                                   || reservation.SecondCrewPerson.Firstname.Contains(filter.SecondCrewName));
                reservations = reservations.WhereIf(filter.Remarks,
                    reservation => reservation.Remarks.Contains(filter.Remarks));
                reservations = reservations.WhereIf(filter.ReservationTypeName,
                    reservation => reservation.AircraftReservationType.AircraftReservationTypeName.Contains(filter.ReservationTypeName));
                
                var pagedQuery = new PagedQuery<AircraftReservation>(reservations, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(x => x.ToAircraftReservationOverview())
                .Where(obj => obj != null)
                .ToList();

                SetAircraftReservationOverviewSecurity(overviewList);

                var pagedList = new PagedList<AircraftReservationOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public AircraftReservationDetails GetAircraftReservationDetails(Guid aircraftReservationId)
        {
            var aircraftReservation = GetAircraftReservation(aircraftReservationId);
            var details = aircraftReservation.ToAircraftReservationDetails();
            SetAircraftReservationDetailsSecurity(details, aircraftReservation);
            return details;
        }

        public List<AircraftReservationOverview> GetAircraftReservationsByPlanningDayId(Guid planningDayId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var planningDay = context.PlanningDays.FirstOrDefault(q => q.PlanningDayId == planningDayId);
                planningDay.EntityNotNull("PlanningDay", planningDayId);
                
                var entities = context.AircraftReservations
                    .Include(Constants.Aircraft)
                    .Include("PilotPerson")
                    .Include("Location")
                    .Include("SecondCrewPerson")
                    .Include("FlightType")
                    .Include("AircraftReservationType")
                    .Where(r => r.ClubId == planningDay.ClubId 
                        && DbFunctions.TruncateTime(r.Start) == planningDay.Day.Date
                        && r.LocationId == planningDay.LocationId)
                    .OrderBy(pe => pe.Start)
                    .ToList();

                var overviewList = entities.Select(entity => entity.ToAircraftReservationOverview()).ToList();
                SetAircraftReservationOverviewSecurity(overviewList);
                return overviewList;
            }
        }
        
        internal AircraftReservation GetAircraftReservation(Guid aircraftReservationId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraftReservation = context.AircraftReservations
                    .FirstOrDefault(p => p.AircraftReservationId == aircraftReservationId);

                return aircraftReservation;
            }
        }

        /// <summary>
        /// Gets the aircraft reservations from the current date to the future
        /// </summary>
        /// <returns></returns>
        internal List<AircraftReservation> GetAircraftReservations(DateTime fromDate)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<AircraftReservation> entities = null;
                
                entities = context.AircraftReservations
                    .Include(Constants.Aircraft)
                    .Include("PilotPerson")
                    .Include("Location")
                    .Include("SecondCrewPerson")
                    .Include("FlightType")
                    .Include("AircraftReservationType")
                    .Where(r => r.ClubId == CurrentAuthenticatedFLSUserClubId 
                        && DbFunctions.TruncateTime(r.Start) >= fromDate.Date)
                    .OrderBy(pe => pe.Start).ToList();

                return entities;
            }
        }

        /// <summary>
        /// Gets the aircraft reservations from the current date to the future
        /// </summary>
        /// <returns></returns>
        internal List<AircraftReservation> GetAircraftReservationsOfDay(DateTime day)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<AircraftReservation> entities = null;

                entities = context.AircraftReservations
                    .Include(Constants.Aircraft)
                    .Include("PilotPerson")
                    .Include("Location")
                    .Include("SecondCrewPerson")
                    .Include("FlightType")
                    .Include("AircraftReservationType")
                    .Where(r => r.ClubId == CurrentAuthenticatedFLSUserClubId
                        && DbFunctions.TruncateTime(r.Start) == day.Date)
                    .OrderBy(pe => pe.Start).ToList();

                return entities;
            }
        }

        public void InsertAircraftReservationDetails(AircraftReservationDetails aircraftReservationDetails)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightTypes = context.FlightTypes.ToList();
                var reservationTypes = context.AircraftReservationTypes.ToList();

                var aircraftReservation = aircraftReservationDetails.ToAircraftReservation(reservationTypes, flightTypes);
                aircraftReservation.ClubId = CurrentAuthenticatedFLSUserClubId;

                context.AircraftReservations.Add(aircraftReservation);
                context.SaveChanges();

                //Map it back to details
                aircraftReservation.ToAircraftReservationDetails(aircraftReservationDetails);
            }
        }
        
        public void UpdateAircraftReservationDetails(AircraftReservationDetails currentAircraftReservationDetails)
        {
            currentAircraftReservationDetails.ArgumentNotNull("currentAircraftReservationDetails");
            var original = GetAircraftReservation(currentAircraftReservationDetails.AircraftReservationId);
            original.EntityNotNull("AircraftReservation", currentAircraftReservationDetails.AircraftReservationId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightTypes = context.FlightTypes.ToList();
                var reservationTypes = context.AircraftReservationTypes.ToList();

                context.AircraftReservations.Attach(original);
                currentAircraftReservationDetails.ToAircraftReservation(reservationTypes, flightTypes, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToAircraftReservationDetails(currentAircraftReservationDetails);
                }
            }
        }

        public void DeleteAircraftReservationDetails(Guid aircraftReservationId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.AircraftReservations.FirstOrDefault(l => l.AircraftReservationId == aircraftReservationId);
                original.EntityNotNull("AircraftReservation", aircraftReservationId);

                context.AircraftReservations.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion AircraftReservation

        #region AircraftReservationTypes
        public List<AircraftReservationTypeListItem> GetAircraftReservationTypeListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraftReservationTypes = context.AircraftReservationTypes
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderBy(pe => pe.AircraftReservationTypeName).ToList();

                var flightTypes = context.FlightTypes
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId
                        && x.IsForAircraftReservationType)
                    .OrderBy(pe => pe.FlightTypeName).ToList();

                var listItems = aircraftReservationTypes.Select(x => new AircraftReservationTypeListItem()
                {
                    AircraftReservationTypeId = x.AircraftReservationTypeId,
                    AircraftReservationTypeName = x.AircraftReservationTypeName,
                    Remarks = x.Remarks,
                    IsInstructorRequired = x.IsInstructorRequired,
                    IsObserverPilotOrInstructorRequired = false,
                    IsPassengerRequired = false
                }).ToList();

                listItems.AddRange(flightTypes.Select(x => new AircraftReservationTypeListItem()
                {
                    AircraftReservationTypeId = x.FlightTypeId,
                    AircraftReservationTypeName = x.FlightTypeName,
                    IsInstructorRequired = x.InstructorRequired,
                    IsObserverPilotOrInstructorRequired = x.ObserverPilotOrInstructorRequired,
                    IsPassengerRequired = x.IsPassengerFlight
                }));

                return listItems;
            }
        }
        #endregion AircraftReservationTypes

        #region Security
        private void SetAircraftReservationOverviewSecurity(IEnumerable<AircraftReservationOverview> list)
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
                        IsOwner(context.AircraftReservations.First(a => a.AircraftReservationId == overview.AircraftReservationId)))
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

        private void SetAircraftReservationDetailsSecurity(AircraftReservationDetails details, AircraftReservation aircraftReservation)
        {
            if (details == null)
            {
                Logger.Error(string.Format("AircraftReservationDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || IsOwner(aircraftReservation))
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
