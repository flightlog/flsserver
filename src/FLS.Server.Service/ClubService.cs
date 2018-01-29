using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Articles;
using FLS.Data.WebApi.Club;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using NLog;
using UserAccountState = FLS.Data.WebApi.User.UserAccountState;

namespace FLS.Server.Service
{
    public class ClubService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public ClubService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Club
        public List<ClubOverview> GetClubOverviews()
        {
            var clubs = GetClubs();

            var clubOverviewResult = clubs.Select(e => e.ToClubOverview()).ToList();

            SetClubOverviewSecurity(clubOverviewResult);

            return clubOverviewResult;
        }

        public PagedList<ClubOverview> GetPagedClubOverview(int? pageStart, int? pageSize, PageableSearchFilter<ClubOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<ClubOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new ClubOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "CountryName")
                {
                    pageableSearchFilter.Sorting.Add("Country.CountryName", pageableSearchFilter.Sorting[sort]);
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
                var clubs = context.Clubs.Include(Constants.Country)
                        .OrderByPropertyNames(pageableSearchFilter.Sorting);

                if (IsCurrentUserInRoleSystemAdministrator == false)
                {
                    //don't return system club to normal users, workflows, etc. as it is just used for the system admin user
                    clubs = clubs.Where(q => q.ClubStateId != (int) FLS.Data.WebApi.Club.ClubState.System);
                }

                var filter = pageableSearchFilter.SearchFilter;
                clubs = clubs.WhereIf(filter.ClubName,
                        club => club.Clubname.Contains(filter.ClubName));
                clubs = clubs.WhereIf(filter.Address,
                        club => club.Address.Contains(filter.Address));
                clubs = clubs.WhereIf(filter.City,
                        club => club.City.Contains(filter.City));
                clubs = clubs.WhereIf(filter.CountryName,
                        club => club.Country.CountryName.Contains(filter.CountryName));
                clubs = clubs.WhereIf(filter.EmailAddress,
                        club => club.Email.Contains(filter.EmailAddress));
                clubs = clubs.WhereIf(filter.PhoneNumber,
                        club => club.Phone.Contains(filter.PhoneNumber));
                clubs = clubs.WhereIf(filter.ZipCode,
                        club => club.Zip.Contains(filter.ZipCode));
                clubs = clubs.WhereIf(filter.HomebaseName,
                        club => club.Homebase.LocationName.Contains(filter.HomebaseName));


                var pagedQuery = new PagedQuery<Club>(clubs, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(e => e.ToClubOverview()).ToList();

                SetClubOverviewSecurity(overviewList);

                var pagedList = new PagedList<ClubOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public ClubDetails GetMyClubDetails()
        {
            return GetClubDetails(CurrentAuthenticatedFLSUser.ClubId);
        }

        public ClubDetails GetClubDetails(Guid clubId)
        {
            var club = GetClub(clubId, false);

            var clubDetails = club.ToClubDetails();
            SetClubDetailsSecurity(clubDetails, club);

            return clubDetails;
        }

        internal List<Club> GetClubs(bool includeRelatedEntities = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                IOrderedQueryable<Club> clubs = null;
                if (includeRelatedEntities)
                {
                    clubs = context.Clubs.Include(Constants.Country)
                        //.Include("Location")
                        .OrderBy(name => name.Clubname);
                }
                else
                {
                    clubs = context.Clubs.OrderBy(name => name.Clubname);
                }

                if (IsCurrentUserInRoleSystemAdministrator == false)
                {
                    //don't return system club to normal users, workflows, etc. as it is just used for the system admin user
                    clubs = clubs.Where(q => q.ClubStateId != (int) FLS.Data.WebApi.Club.ClubState.System)
                        .OrderBy(name => name.Clubname);
                }

                return clubs.ToList();
            }
        }

        internal Club GetClub(Guid clubId, bool includeRelatedEntities = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                Club club = null;
                if (includeRelatedEntities)
                {
                    club = context.Clubs
                        .Include(Constants.Country)
                        .Include("Homebase")
                        .Include("DefaultGliderFlightType")
                        .Include("DefaultMotorFlightType")
                        .Include("DefaultStartType")
                        .Include("DefaultTowFlightType")
                        .FirstOrDefault(a => a.ClubId == clubId);
                }
                else
                {
                    club = context.Clubs.FirstOrDefault(a => a.ClubId == clubId);
                }
                
                return club;
            }
        }

        public void InsertClubDetails(ClubDetails clubDetails)
        {
            var club = clubDetails.ToClub();
            club.EntityNotNull("Club", Guid.Empty);

            InsertClub(club);

            //Map it back to details
            club.ToClubDetails(clubDetails);
        }

        internal void InsertClub(Club club)
        {
            club.ArgumentNotNull("club");

            if (IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleSystemAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Clubs.Add(club);

                var planningDayAssignmentType = new PlanningDayAssignmentType()
                {
                    AssignmentTypeName = "Segelflugleiter",
                    Club = club,
                    RequiredNrOfPlanningDayAssignments = 1
                };
                context.PlanningDayAssignmentTypes.Add(planningDayAssignmentType);

                planningDayAssignmentType = new PlanningDayAssignmentType()
                {
                    AssignmentTypeName = "Schlepppilot",
                    Club = club,
                    RequiredNrOfPlanningDayAssignments = 1
                };
                context.PlanningDayAssignmentTypes.Add(planningDayAssignmentType);

                planningDayAssignmentType = new PlanningDayAssignmentType()
                {
                    AssignmentTypeName = "Fluglehrer",
                    Club = club,
                    RequiredNrOfPlanningDayAssignments = 1
                };
                context.PlanningDayAssignmentTypes.Add(planningDayAssignmentType);

                context.SaveChanges();
            }
        }

        public void UpdateClubDetails(ClubDetails currentClubDetails)
        {
            currentClubDetails.ArgumentNotNull("currentClubDetails");
            var original = GetClub(currentClubDetails.ClubId);
            original.EntityNotNull("Club", currentClubDetails.ClubId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Clubs.Attach(original);
                currentClubDetails.ToClub(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToClubDetails(currentClubDetails);
                }
            }
        }

        public void DeleteClub(Guid clubId)
        {
            if (IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleSystemAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = GetClub(clubId, false);
                original.EntityNotNull("Club", clubId);
                context.Clubs.Attach(original);

                if (context.Users.Any(u => u.AccountState == (int)UserAccountState.Active && u.ClubId == clubId))
                {
                    throw new DeleteEntityException(ErrorMessage.CantDeleteClubDuoToActiveUsers);
                }

                context.Clubs.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Club

        #region FlightType
        public List<FlightTypeOverview> GetFlightTypeOverviews()
        {
            return GetFlightTypeOverviews(CurrentAuthenticatedFLSUserClubId);
        }

        public List<FlightTypeOverview> GetGliderFlightTypeOverviews()
        {
            return GetGliderFlightTypeOverviews(CurrentAuthenticatedFLSUserClubId);
        }

        public List<FlightTypeOverview> GetTowingFlightTypeOverviews()
        {
            return GetTowingFlightTypeOverviews(CurrentAuthenticatedFLSUserClubId);
        }

        public List<FlightTypeOverview> GetMotorFlightTypeOverviews()
        {
            return GetMotorFlightTypeOverviews(CurrentAuthenticatedFLSUserClubId);
        }

        public List<FlightTypeOverview> GetFlightTypeOverviews(Guid clubId)
        {
            var flightTypes = GetFlightTypes(clubId);

            return PrepareFlightTypeOverviews(flightTypes);
        }

        private List<FlightTypeOverview> PrepareFlightTypeOverviews(List<FlightType> flightTypes)
        {
            var flightTypeOverviewList = flightTypes.Select(e => e.ToFlightTypeOverview()).ToList();
            SetFlightTypeOverviewSecurity(flightTypeOverviewList);

            return flightTypeOverviewList;
        }

        public List<FlightTypeOverview> GetGliderFlightTypeOverviews(Guid clubId)
        {
            var flightTypes = GetFlightTypes(clubId, ft => ft.IsForGliderFlights);

            return PrepareFlightTypeOverviews(flightTypes);
        }

        public List<FlightTypeOverview> GetTowingFlightTypeOverviews(Guid clubId)
        {
            var flightTypes = GetFlightTypes(clubId, ft => ft.IsForTowFlights);

            return PrepareFlightTypeOverviews(flightTypes);
        }

        public List<FlightTypeOverview> GetMotorFlightTypeOverviews(Guid clubId)
        {
            var flightTypes = GetFlightTypes(clubId, ft => ft.IsForMotorFlights);

            return PrepareFlightTypeOverviews(flightTypes);
        }

        public PagedList<FlightTypeOverview> GetPagedFlightTypeOverview(int? pageStart, int? pageSize, PageableSearchFilter<FlightTypeOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<FlightTypeOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new FlightTypeOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightTypes = context.FlightTypes
                    .Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);
                
                var filter = pageableSearchFilter.SearchFilter;
                flightTypes = flightTypes.WhereIf(filter.FlightTypeName,
                        club => club.FlightTypeName.Contains(filter.FlightTypeName));
                flightTypes = flightTypes.WhereIf(filter.FlightCode,
                        club => club.FlightCode.Contains(filter.FlightCode));
                flightTypes = flightTypes.WhereIf(filter.MinNrOfAircraftSeatsRequired,
                        club => club.MinNrOfAircraftSeatsRequired.ToString().Contains(filter.MinNrOfAircraftSeatsRequired));

                if (filter.InstructorRequired.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.InstructorRequired.Value,
                        club => club.InstructorRequired == filter.InstructorRequired.Value);
                
                if (filter.ObserverPilotOrInstructorRequired.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.ObserverPilotOrInstructorRequired.Value,
                        club => club.ObserverPilotOrInstructorRequired == filter.ObserverPilotOrInstructorRequired.Value);

                if (filter.IsCheckFlight.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsCheckFlight.Value,
                        club => club.IsCheckFlight == filter.IsCheckFlight.Value);

                if (filter.IsPassengerFlight.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsPassengerFlight.Value,
                        club => club.IsPassengerFlight == filter.IsPassengerFlight.Value);

                if (filter.IsForGliderFlights.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsForGliderFlights.Value,
                        club => club.IsForGliderFlights == filter.IsForGliderFlights.Value);

                if (filter.IsForTowFlights.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsForTowFlights.Value,
                        club => club.IsForTowFlights == filter.IsForTowFlights.Value);

                if (filter.IsForMotorFlights.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsForMotorFlights.Value,
                        club => club.IsForMotorFlights == filter.IsForMotorFlights.Value);

                if (filter.IsFlightCostBalanceSelectable.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsFlightCostBalanceSelectable.Value,
                        club => club.IsFlightCostBalanceSelectable == filter.IsFlightCostBalanceSelectable.Value);

                if (filter.IsSoloFlight.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsSoloFlight.Value,
                        club => club.IsSoloFlight == filter.IsSoloFlight.Value);

                if (filter.IsCouponNumberRequired.HasValue)
                    flightTypes = flightTypes.WhereIf(filter.IsCouponNumberRequired.Value,
                        club => club.IsCouponNumberRequired == filter.IsCouponNumberRequired.Value);

                var pagedQuery = new PagedQuery<FlightType>(flightTypes, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(e => e.ToFlightTypeOverview()).ToList();

                SetFlightTypeOverviewSecurity(overviewList);

                var pagedList = new PagedList<FlightTypeOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public FlightTypeDetails GetFlightTypeDetails(Guid flightTypeId)
        {
            var flightType = GetFlightType(flightTypeId);

            var flightTypeDetails = flightType.ToFlightTypeDetails();
            SetFlightTypeDetailsSecurity(flightTypeDetails, flightType);

            return flightTypeDetails;
        }

        internal List<FlightType> GetFlightTypes(Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<FlightType> flightTypes = null;

                if (IsCurrentUserInClub(clubId) == false)
                {
                    Logger.Warn("User is not in club and is not allowed to call this webservice method!");

                    throw new UnauthorizedAccessException(ErrorMessage.NotInSameClub);
                }
                else
                {
                    flightTypes = context.FlightTypes.Where(c => c.ClubId == clubId).OrderBy(t => t.FlightTypeName).ToList();
                }

                return flightTypes;
            }
        }

        internal List<FlightType> GetFlightTypes(Guid clubId, Expression<Func<FlightType, bool>> flightTypeFilter)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<FlightType> flightTypes = null;

                if (IsCurrentUserInClub(clubId)
                    || IsCurrentUserInRoleSystemAdministrator)
                {
                    flightTypes = context.FlightTypes.Where(c => c.ClubId == clubId).Where(flightTypeFilter).OrderBy(t => t.FlightTypeName).ToList();
                }
                else
                {
                    throw new UnauthorizedAccessException(ErrorMessage.NotInSameClub);
                }

                return flightTypes;
            }
        }

        internal FlightType GetFlightType(Guid flightTypeId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var flightType = context.FlightTypes
                    .FirstOrDefault(a => a.FlightTypeId == flightTypeId);

                return flightType;
            }
        }

        public void InsertFlightTypeDetails(FlightTypeDetails flightTypeDetails)
        {
            CurrentAuthenticatedFLSUserClubId.NotNullOrEmptyGuid("CurrentAuthenticatedFLSUserClubId");
            var flightType = flightTypeDetails.ToFlightType(CurrentAuthenticatedFLSUserClubId);
            flightType.EntityNotNull("FlightType", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.FlightTypes.Add(flightType);
                context.SaveChanges();
            }

            //Map it back to details
            flightType.ToFlightTypeDetails(flightTypeDetails);
        }
        
        public void UpdateFlightTypeDetails(FlightTypeDetails currentFlightTypeDetails)
        {
            currentFlightTypeDetails.ArgumentNotNull("currentFlightTypeDetails");
            var original = GetFlightType(currentFlightTypeDetails.FlightTypeId);
            original.EntityNotNull("FlightType", currentFlightTypeDetails.FlightTypeId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.FlightTypes.Attach(original);
                currentFlightTypeDetails.ToFlightType(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToFlightTypeDetails(currentFlightTypeDetails);
                }
            }
        }

        public void DeleteFlightType(Guid flightTypeId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.FlightTypes.ToList().FirstOrDefault(l => l.FlightTypeId == flightTypeId);
                original.EntityNotNull("FlightType", flightTypeId);

                context.FlightTypes.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion FlightType

        #region MemberState
        public List<MemberStateOverview> GetMemberStateOverviews()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var memberStates = context.MemberStates
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderBy(name => name.MemberStateName).ToList();

                var memberStateOverviewResult = memberStates.Select(e => e.ToMemberStateOverview()).ToList();

                SetMemberStateOverviewSecurity(memberStateOverviewResult);

                return memberStateOverviewResult;
            }
        }

        public PagedList<MemberStateOverview> GetPagedMemberStateOverview(int? pageStart, int? pageSize, PageableSearchFilter<MemberStateOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<MemberStateOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new MemberStateOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var memberStates = context.MemberStates
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                memberStates = memberStates.WhereIf(filter.MemberStateName,
                        club => club.MemberStateName.Contains(filter.MemberStateName));

                var pagedQuery = new PagedQuery<MemberState>(memberStates, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(e => e.ToMemberStateOverview()).ToList();

                SetMemberStateOverviewSecurity(overviewList);

                var pagedList = new PagedList<MemberStateOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public MemberStateDetails GetMemberStateDetails(Guid memberStateId)
        {
            var memberState = GetMemberState(memberStateId);

            var memberStateDetails = memberState.ToMemberStateDetails();
            SetMemberStateDetailsSecurity(memberStateDetails);

            return memberStateDetails;
        }
        
        internal MemberState GetMemberState(Guid memberStateId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                MemberState memberState = context.MemberStates
                        .FirstOrDefault(a => a.MemberStateId == memberStateId);

                return memberState;
            }
        }

        public void InsertMemberStateDetails(MemberStateDetails memberStateDetails)
        {
            var memberState = memberStateDetails.ToMemberState(CurrentAuthenticatedFLSUserClubId);
            memberState.EntityNotNull("MemberState", Guid.Empty);

            InsertMemberState(memberState);

            //Map it back to details
            memberState.ToMemberStateDetails(memberStateDetails);
        }

        internal void InsertMemberState(MemberState memberState)
        {
            memberState.ArgumentNotNull("memberState");

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.MemberStates.Add(memberState);
                context.SaveChanges();
            }
        }

        public void UpdateMemberStateDetails(MemberStateDetails currentMemberStateDetails)
        {
            currentMemberStateDetails.ArgumentNotNull("currentMemberStateDetails");
            var original = GetMemberState(currentMemberStateDetails.MemberStateId);
            original.EntityNotNull("MemberState", currentMemberStateDetails.MemberStateId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.MemberStates.Attach(original);
                currentMemberStateDetails.ToMemberState(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToMemberStateDetails(currentMemberStateDetails);
                }
            }
        }

        public void DeleteMemberState(Guid memberStateId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = GetMemberState(memberStateId);
                original.EntityNotNull("MemberState", memberStateId);
                context.MemberStates.Attach(original);
                
                context.MemberStates.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion MemberState

        #region PersonCategory
        public List<PersonCategoryOverview> GetPersonCategoryOverviews()
        {
            var personCategories = GetPersonCategories();

            var personCategoryOverviewResult = personCategories.Select(e => e.ToPersonCategoryOverview()).ToList();

            SetPersonCategoryOverviewSecurity(personCategoryOverviewResult);

            return personCategoryOverviewResult;
        }

        public PagedList<PersonCategoryOverview> GetPagedPersonCategoryOverview(int? pageStart, int? pageSize, PageableSearchFilter<PersonCategoryOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<PersonCategoryOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new PersonCategoryOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var personCategories = context.PersonCategories
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                personCategories = personCategories.WhereIf(filter.CategoryName,
                        club => club.CategoryName.Contains(filter.CategoryName));

                var pagedQuery = new PagedQuery<PersonCategory>(personCategories, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(e => e.ToPersonCategoryOverview()).ToList();

                SetPersonCategoryOverviewSecurity(overviewList);

                var pagedList = new PagedList<PersonCategoryOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public PersonCategoryDetails GetPersonCategoryDetails(Guid personCategoryId)
        {
            var personCategory = GetPersonCategory(personCategoryId);

            var personCategoryDetails = personCategory.ToPersonCategoryDetails();
            SetPersonCategoryDetailsSecurity(personCategoryDetails);

            return personCategoryDetails;
        }

        internal List<PersonCategory> GetPersonCategories()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var personCategories = context.PersonCategories
                    .Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId)
                        .OrderBy(name => name.CategoryName).ToList();
                return personCategories;
            }
        }

        internal PersonCategory GetPersonCategory(Guid personCategoryId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                PersonCategory personCategory = context.PersonCategories
                        .FirstOrDefault(a => a.PersonCategoryId == personCategoryId);

                return personCategory;
            }
        }

        public void InsertPersonCategoryDetails(PersonCategoryDetails personCategoryDetails)
        {
            var personCategory = personCategoryDetails.ToPersonCategory(CurrentAuthenticatedFLSUserClubId);
            personCategory.EntityNotNull("PersonCategory", Guid.Empty);

            InsertPersonCategory(personCategory);

            //Map it back to details
            personCategory.ToPersonCategoryDetails(personCategoryDetails);
        }

        internal void InsertPersonCategory(PersonCategory personCategory)
        {
            personCategory.ArgumentNotNull("personCategory");

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.PersonCategories.Add(personCategory);
                context.SaveChanges();
            }
        }

        public void UpdatePersonCategoryDetails(PersonCategoryDetails currentPersonCategoryDetails)
        {
            currentPersonCategoryDetails.ArgumentNotNull("currentPersonCategoryDetails");
            var original = GetPersonCategory(currentPersonCategoryDetails.PersonCategoryId);
            original.EntityNotNull("PersonCategory", currentPersonCategoryDetails.PersonCategoryId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.PersonCategories.Attach(original);
                currentPersonCategoryDetails.ToPersonCategory(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToPersonCategoryDetails(currentPersonCategoryDetails);
                }
            }
        }

        public void DeletePersonCategory(Guid personCategoryId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = GetPersonCategory(personCategoryId);
                original.EntityNotNull("PersonCategory", personCategoryId);
                context.PersonCategories.Attach(original);

                context.PersonCategories.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion PersonCategory

        #region Security
        private void SetClubOverviewSecurity(IEnumerable<ClubOverview> clubList)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in clubList)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var clubOverview in clubList)
                {
                    if (IsCurrentUserInRoleSystemAdministrator)
                    {
                        clubOverview.CanUpdateRecord = true;
                        clubOverview.CanDeleteRecord = true;
                    }
                    else if (IsCurrentUserInRoleClubAdministrator && IsCurrentUserInClub(clubOverview.ClubId))
                    {
                        clubOverview.CanUpdateRecord = true;
                        clubOverview.CanDeleteRecord = false;
                    }
                    else if (IsOwner(context.Clubs.First(a => a.ClubId == clubOverview.ClubId)))
                    {
                        clubOverview.CanUpdateRecord = true;
                        clubOverview.CanDeleteRecord = false;
                    }
                    else
                    {
                        clubOverview.CanUpdateRecord = false;
                        clubOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetClubDetailsSecurity(ClubDetails details, Club club)
        {
            if (details == null)
            {
                Logger.Error(string.Format("ClubDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
            }
            else if (IsCurrentUserInRoleClubAdministrator && IsCurrentUserInClub(club.ClubId))
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = false;
            }
            else if (IsOwner(club))
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = false;
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }

        private void SetFlightTypeOverviewSecurity(IEnumerable<FlightTypeOverview> flightTypeList)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in flightTypeList)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var flightTypeOverview in flightTypeList)
                {
                    if (IsCurrentUserInRoleClubAdministrator)
                    {
                        flightTypeOverview.CanUpdateRecord = true;
                        flightTypeOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        flightTypeOverview.CanUpdateRecord = false;
                        flightTypeOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetFlightTypeDetailsSecurity(FlightTypeDetails details, FlightType flightType)
        {
            if (details == null)
            {
                Logger.Error(string.Format("FlightTypeDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator)
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

        private void SetMemberStateOverviewSecurity(List<MemberStateOverview> memberStateOverviewResult)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in memberStateOverviewResult)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in memberStateOverviewResult)
            {
                if (IsCurrentUserInRoleClubAdministrator)
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

        private void SetMemberStateDetailsSecurity(MemberStateDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("MemberStateDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator)
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

        private void SetPersonCategoryOverviewSecurity(List<PersonCategoryOverview> overviewResult)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in overviewResult)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in overviewResult)
            {
                if (IsCurrentUserInRoleClubAdministrator)
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

        private void SetPersonCategoryDetailsSecurity(PersonCategoryDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("PersonCategoryDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator)
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
