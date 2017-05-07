using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.System;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Interfaces;
using Newtonsoft.Json;

namespace FLS.Server.Service.Accounting
{
    public class AccountingRuleService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IPersonService _personService;
        private readonly IExtensionService _extensionService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;

        public AccountingRuleService(DataAccessService dataAccessService, IdentityService identityService, IPersonService personService, 
            IExtensionService extensionService, IAircraftService aircraftService, ILocationService locationService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _personService = personService;
            _extensionService = extensionService;
            _aircraftService = aircraftService;
            _locationService = locationService;
        }

        #region AccountingRuleFilterType
        public List<AccountingRuleFilterTypeListItem> GetAccountingRuleFilterTypeListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var entities = context.AccountingRuleFilterTypes.OrderBy(l => l.AccountingRuleFilterTypeName).ToList();

                var items = entities.Select(x => new AccountingRuleFilterTypeListItem()
                {
                    AccountingRuleFilterTypeId = x.AccountingRuleFilterTypeId,
                    AccountingRuleFilterTypeName = x.AccountingRuleFilterTypeName
                }).ToList();

                return items;
            }
        }
        #endregion AccountingRuleFilterType

        #region AccountingRuleFilter
        public List<AccountingRuleFilterOverview> GetAccountingRuleFilterOverview()
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly:true);

            var filters = new List<AccountingRuleFilterOverview>();

            using (var context = _dataAccessService.CreateDbContext())
            {
                var accountingRuleFilters = context.AccountingRuleFilters.Include("AccountingRuleFilterType")
                    .Where(q => q.ClubId == CurrentAuthenticatedFLSUserClubId);

                foreach (var accountingRuleFilter in accountingRuleFilters)
                {
                    var filter = accountingRuleFilter.ToAccountingRuleFilterOverview(aicrafts, locations);
                    filters.Add(filter);
                }
            }

            SetAccountingRuleFilterOverviewSecurity(filters);

            return filters;
        }

        public PagedList<AccountingRuleFilterOverview> GetPagedAccountingRuleFilterOverview(int? pageStart, int? pageSize, PageableSearchFilter<AccountingRuleFilterOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<AccountingRuleFilterOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new AccountingRuleFilterOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("SortIndicator", "asc");
            }
            
            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "AccountingRuleFilterTypeName")
                {
                    pageableSearchFilter.Sorting.Add("AccountingRuleFilterType.AccountingRuleFilterTypeName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "Target")
                {
                    pageableSearchFilter.Sorting.Add("RecipientTarget", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Add("ArticleTarget", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("SortIndicator", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var accountingRuleFilters = context.AccountingRuleFilters.Include("AccountingRuleFilterType")
                    .Where(q => q.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                accountingRuleFilters = accountingRuleFilters.WhereIf(filter.AccountingRuleFilterTypeName,
                        accountingRuleFilter => accountingRuleFilter.AccountingRuleFilterType.AccountingRuleFilterTypeName.Contains(filter.AccountingRuleFilterTypeName));
                accountingRuleFilters = accountingRuleFilters.WhereIf(filter.Description,
                        accountingRuleFilter => accountingRuleFilter.Description.Contains(filter.Description));
                accountingRuleFilters = accountingRuleFilters.WhereIf(filter.RuleFilterName,
                        accountingRuleFilter => accountingRuleFilter.RuleFilterName.Contains(filter.RuleFilterName));
                accountingRuleFilters = accountingRuleFilters.WhereIf(filter.SortIndicator,
                        accountingRuleFilter => accountingRuleFilter.SortIndicator.ToString().Contains(filter.SortIndicator));
                accountingRuleFilters = accountingRuleFilters.WhereIf(filter.Target,
                        accountingRuleFilter => accountingRuleFilter.RecipientTarget.Contains(filter.Target)
                            || accountingRuleFilter.ArticleTarget.Contains(filter.Target));

                var pagedQuery = new PagedQuery<AccountingRuleFilter>(accountingRuleFilters, pageStart, pageSize);

                var aicrafts = _aircraftService.GetAircraftListItems();
                var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

                var overviewList = pagedQuery.Items.ToList().Select(x => x.ToAccountingRuleFilterOverview(aicrafts, locations))
                .Where(obj => obj != null)
                .ToList();

                var pagedList = new PagedList<AccountingRuleFilterOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        internal List<AccountingRuleFilterDetails> GetAccountingRuleFilterDetailsListByClubId(Guid clubId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var filters = new List<AccountingRuleFilterDetails>();

            using (var context = _dataAccessService.CreateDbContext())
            {
                var accountingRuleFilters = context.AccountingRuleFilters.Where(q => q.ClubId == clubId);

                foreach (var accountingRuleFilter in accountingRuleFilters)
                {
                    var filter = accountingRuleFilter.ToAccountingRuleFilterDetails(aicrafts, locations);
                    filters.Add(filter);
                }
            }

            return filters;
        }

        internal List<RuleBasedAccountingRuleFilterDetails> GetRuleBasedAccountingRuleFilterDetailsListByClubId(Guid clubId)
        {
            var aircrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var filters = GetAccountingRuleFilterDetailsListByClubId(clubId);
            var ruleBasedFilters = new List<RuleBasedAccountingRuleFilterDetails>();

            foreach (var filter in filters)
            {
                var ruleBasedFilter = new RuleBasedAccountingRuleFilterDetails(filter);
                ruleBasedFilter = RemapAircraftIds(ruleBasedFilter, aircrafts);
                ruleBasedFilter = RemapLocationIds(ruleBasedFilter, locations);
                ruleBasedFilters.Add(ruleBasedFilter);
            }

            return ruleBasedFilters;
        }

        
        public RuleBasedAccountingRuleFilterDetails RemapLocationIds(RuleBasedAccountingRuleFilterDetails filter, List<LocationListItem> locationListItems)
        {
            filter.MatchedStartLocationIds = new List<Guid>();
            filter.MatchedLdgLocationIds = new List<Guid>();

            foreach (var icaoCode in filter.MatchedLdgLocations)
            {
                var location = locationListItems.FirstOrDefault(q => q.IcaoCode != null && q.IcaoCode.ToUpper() == icaoCode.ToUpper());

                if (location == null)
                {
                    Logger.Warn($"Location ICAO code {icaoCode} for accounting rule {filter.RuleFilterName} not found!");
                }
                else
                {
                    filter.MatchedLdgLocationIds.Add(location.LocationId);
                }
            }

            foreach (var icaoCode in filter.MatchedStartLocations)
            {
                var location = locationListItems.FirstOrDefault(q => q.IcaoCode != null && q.IcaoCode.ToUpper() == icaoCode.ToUpper());

                if (location == null)
                {
                    Logger.Warn($"Location ICAO code {icaoCode} for accounting rule {filter.RuleFilterName} not found!");
                }
                else
                {
                    filter.MatchedStartLocationIds.Add(location.LocationId);
                }
            }

            return filter;
        }

        public RuleBasedAccountingRuleFilterDetails RemapAircraftIds(RuleBasedAccountingRuleFilterDetails filter, List<AircraftListItem> aircraftListItems)
        {
            filter.MatchedAircraftIds = new List<Guid>();

            foreach (var immatriculation in filter.MatchedAircraftImmatriculations)
            {
                var aircraft =
                    aircraftListItems.FirstOrDefault(q => q.Immatriculation.ToUpper() == immatriculation.ToUpper());

                if (aircraft == null)
                {
                    Logger.Warn($"Aircraft immatriculation {immatriculation} for accounting rule {filter.RuleFilterName} not found!");
                }
                else
                {
                    filter.MatchedAircraftIds.Add(aircraft.AircraftId);
                }
            }

            return filter;
        }

        public AccountingRuleFilterDetails GetAccountingRuleFilterDetails(Guid accountingRuleFilterId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            using (var context = _dataAccessService.CreateDbContext())
            {
                var accountingRuleFilter = context.AccountingRuleFilters.FirstOrDefault(c => c.AccountingRuleFilterId == accountingRuleFilterId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var accountingRuleFilterDetails = accountingRuleFilter.ToAccountingRuleFilterDetails(aicrafts, locations);
                SetAccountingRuleFilterDetailsSecurity(accountingRuleFilterDetails);
                return accountingRuleFilterDetails;
            }
        }

        /// <summary>
        /// Used only for initial insert of FGZO rules from AccountingMappingFactory
        /// </summary>
        /// <param name="accountingRuleFilterDetails"></param>
        /// <param name="clubId"></param>
        internal void InsertAccountingRuleFilterDetails(AccountingRuleFilterDetails accountingRuleFilterDetails, Guid clubId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var accountingRuleFilter = accountingRuleFilterDetails.ToAccountingRuleFilter(clubId, aicrafts, locations);
            accountingRuleFilter.EntityNotNull("AccountingRuleFilter", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.AccountingRuleFilters.Add(accountingRuleFilter);
                context.SaveChanges();
            }

            //Map it back to details
            accountingRuleFilter.ToAccountingRuleFilterDetails(aicrafts, locations, accountingRuleFilterDetails);
        }


        public void InsertAccountingRuleFilterDetails(AccountingRuleFilterDetails accountingRuleFilterDetails)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to insert a new accounting rule filter!");
            }

            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var accountingRuleFilter = accountingRuleFilterDetails.ToAccountingRuleFilter(CurrentAuthenticatedFLSUserClubId, aicrafts, locations);
            accountingRuleFilter.EntityNotNull("AccountingRuleFilter", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.AccountingRuleFilters.Add(accountingRuleFilter);
                context.SaveChanges();
            }

            //Map it back to details
            accountingRuleFilter.ToAccountingRuleFilterDetails(aicrafts, locations, accountingRuleFilterDetails);
        }

        public void UpdateAccountingRuleFilterDetails(AccountingRuleFilterDetails currentAccountingRuleFilterDetails)
        {
            currentAccountingRuleFilterDetails.ArgumentNotNull("currentAccountingRuleFilterDetails");
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.AccountingRuleFilters.FirstOrDefault(x => x.AccountingRuleFilterId == currentAccountingRuleFilterDetails.AccountingRuleFilterId);
                original.EntityNotNull("AccountingRuleFilter", currentAccountingRuleFilterDetails.AccountingRuleFilterId);

                currentAccountingRuleFilterDetails.ToAccountingRuleFilter(CurrentAuthenticatedFLSUserClubId, aicrafts, locations, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToAccountingRuleFilterDetails(aicrafts, locations, currentAccountingRuleFilterDetails);
                }
            }
        }

        public void DeleteAccountingRuleFilter(Guid accountingRuleFilterId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to delete an accounting rule filter!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.AccountingRuleFilters.FirstOrDefault(x => x.AccountingRuleFilterId == accountingRuleFilterId);
                original.EntityNotNull("AccountingRuleFilter", accountingRuleFilterId);

                context.AccountingRuleFilters.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion AccountingRuleFilter

        private void SetAccountingRuleFilterOverviewSecurity(List<AccountingRuleFilterOverview> accountingRuleFilterOverview)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in accountingRuleFilterOverview)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in accountingRuleFilterOverview)
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

        private void SetAccountingRuleFilterDetailsSecurity(AccountingRuleFilterDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("AccountingRuleFilterDetails is null while trying to set security properties"));
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
    }

}
