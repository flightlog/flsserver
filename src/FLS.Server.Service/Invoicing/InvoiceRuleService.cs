using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Invoicing.RuleEngines;
using Newtonsoft.Json;

namespace FLS.Server.Service.Invoicing
{
    public class InvoiceRuleService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IPersonService _personService;
        private readonly IExtensionService _extensionService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;

        public InvoiceRuleService(DataAccessService dataAccessService, IdentityService identityService, IPersonService personService, 
            IExtensionService extensionService, IAircraftService aircraftService, ILocationService locationService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _personService = personService;
            _extensionService = extensionService;
            _aircraftService = aircraftService;
            _locationService = locationService;
        }

        #region InvoiceRuleFilter
        public List<InvoiceRuleFilterOverview> GetInvoiceRuleFilterOverview()
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly:true);

            var filters = new List<InvoiceRuleFilterOverview>();

            using (var context = _dataAccessService.CreateDbContext())
            {
                var invoiceRuleFilters = context.InvoiceRuleFilters.Where(q => q.ClubId == CurrentAuthenticatedFLSUserClubId);

                foreach (var invoiceRuleFilter in invoiceRuleFilters)
                {
                    var filter = invoiceRuleFilter.ToInvoiceRuleFilterOverview(aicrafts, locations);
                    filters.Add(filter);
                }
            }

            return filters;
        }

        internal List<InvoiceRuleFilterDetails> GetInvoiceRuleFilterDetailsListByClubId(Guid clubId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var filters = new List<InvoiceRuleFilterDetails>();

            using (var context = _dataAccessService.CreateDbContext())
            {
                var invoiceRuleFilters = context.InvoiceRuleFilters.Where(q => q.ClubId == clubId);

                foreach (var invoiceRuleFilter in invoiceRuleFilters)
                {
                    var filter = invoiceRuleFilter.ToInvoiceRuleFilterDetails(aicrafts, locations);
                    filters.Add(filter);
                }
            }

            return filters;
        }

        public InvoiceRuleFilterDetails GetInvoiceRuleFilterDetails(Guid invoiceRuleFilterId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            using (var context = _dataAccessService.CreateDbContext())
            {
                var invoiceRuleFilter = context.InvoiceRuleFilters.FirstOrDefault(c => c.InvoiceRuleFilterId == invoiceRuleFilterId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var invoiceRuleFilterDetails = invoiceRuleFilter.ToInvoiceRuleFilterDetails(aicrafts, locations);
                SetInvoiceRuleFilterDetailsSecurity(invoiceRuleFilterDetails);
                return invoiceRuleFilterDetails;
            }
        }

        /// <summary>
        /// Used only for initial insert of FGZO rules from InvoiceMappingFactory
        /// </summary>
        /// <param name="invoieRuleFilterDetails"></param>
        /// <param name="clubId"></param>
        internal void InsertInvoiceRuleFilterDetails(InvoiceRuleFilterDetails invoieRuleFilterDetails, Guid clubId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var invoiceRuleFilter = invoieRuleFilterDetails.ToInvoiceRuleFilter(clubId, aicrafts, locations);
            invoiceRuleFilter.EntityNotNull("InvoiceRuleFilter", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.InvoiceRuleFilters.Add(invoiceRuleFilter);
                context.SaveChanges();
            }

            //Map it back to details
            invoiceRuleFilter.ToInvoiceRuleFilterDetails(aicrafts, locations, invoieRuleFilterDetails);
        }


        public void InsertInvoiceRuleFilterDetails(InvoiceRuleFilterDetails invoieRuleFilterDetails)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to insert a new invoice rule filter!");
            }

            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            var invoiceRuleFilter = invoieRuleFilterDetails.ToInvoiceRuleFilter(CurrentAuthenticatedFLSUserClubId, aicrafts, locations);
            invoiceRuleFilter.EntityNotNull("InvoiceRuleFilter", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.InvoiceRuleFilters.Add(invoiceRuleFilter);
                context.SaveChanges();
            }

            //Map it back to details
            invoiceRuleFilter.ToInvoiceRuleFilterDetails(aicrafts, locations, invoieRuleFilterDetails);
        }

        public void UpdateInvoiceRuleFilterDetails(InvoiceRuleFilterDetails currentInvoiceRuleFilterDetails)
        {
            currentInvoiceRuleFilterDetails.ArgumentNotNull("currentInvoiceRuleFilterDetails");
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly: true);

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.InvoiceRuleFilters.FirstOrDefault(x => x.InvoiceRuleFilterId == currentInvoiceRuleFilterDetails.InvoiceRuleFilterId);
                original.EntityNotNull("InvoiceRuleFilter", currentInvoiceRuleFilterDetails.InvoiceRuleFilterId);

                currentInvoiceRuleFilterDetails.ToInvoiceRuleFilter(CurrentAuthenticatedFLSUserClubId, aicrafts, locations, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToInvoiceRuleFilterDetails(aicrafts, locations, currentInvoiceRuleFilterDetails);
                }
            }
        }

        public void DeleteInvoiceRuleFilter(Guid invoiceRuleFilterId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to delete an invoice rule filter!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.InvoiceRuleFilters.FirstOrDefault(x => x.InvoiceRuleFilterId == invoiceRuleFilterId);
                original.EntityNotNull("InvoiceRuleFilter", invoiceRuleFilterId);

                context.InvoiceRuleFilters.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion InvoiceRuleFilter

        private void SetInvoiceRuleFilterOverviewSecurity(List<InvoiceRuleFilterOverview> invoiceRuleFilterOverview)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in invoiceRuleFilterOverview)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in invoiceRuleFilterOverview)
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

        private void SetInvoiceRuleFilterDetailsSecurity(InvoiceRuleFilterDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("InvoiceRuleFilterDetails is null while trying to set security properties"));
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
