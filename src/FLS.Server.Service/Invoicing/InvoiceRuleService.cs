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
        private readonly InvoiceMappingFactory _invoiceMappingFactory;
        private readonly IPersonService _personService;
        private readonly IExtensionService _extensionService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;

        public InvoiceRuleService(DataAccessService dataAccessService, IdentityService identityService, InvoiceMappingFactory invoiceMappingFactory, IPersonService personService, 
            IExtensionService extensionService, IAircraftService aircraftService, ILocationService locationService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _invoiceMappingFactory = invoiceMappingFactory;
            _personService = personService;
            _extensionService = extensionService;
            _aircraftService = aircraftService;
            _locationService = locationService;
        }

        #region InvoiceLineRuleFilter
        //public List<InvoiceRuleFilterOverview> GetInvoiceRuleFilterOverviews()
        //{
        //    using (var context = _dataAccessService.CreateDbContext())
        //    {
        //        List<Article> articles = context.Articles.Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId).OrderBy(t => t.ArticleNumber).ToList();

        //        var overviewList = articles.Select(x => new ArticleOverview()
        //        {
        //            ArticleId = x.ArticleId,
        //            ArticleNumber = x.ArticleNumber,
        //            ArticleName = x.ArticleName,
        //            IsActive = x.IsActive
        //        }).ToList();

        //        SetArticleOverviewSecurity(overviewList);
        //        return overviewList;
        //    }
        //}

        public List<InvoiceLineRuleFilterDetails> GetInvoiceLineRuleFilterDetailsList(Guid clubId)
        {
            var aicrafts = _aircraftService.GetAircraftListItems();
            var locations = _locationService.GetLocationListItems(airfieldsOnly:true);

            List<InvoiceLineRuleFilterDetails> filters = new List<InvoiceLineRuleFilterDetails>();

            using (var context = _dataAccessService.CreateDbContext())
            {
                var invoiceRuleFilters = context.InvoiceRuleFilters.Where(q => q.ClubId == clubId);

                foreach (var invoiceRuleFilter in invoiceRuleFilters)
                {
                    var filter = invoiceRuleFilter.ToInvoiceLineRuleFilterDetails(aicrafts, locations);
                    filters.Add(filter);
                }
            }

            return filters;
        }

        //public InvoiceRuleFilterDetails GetInvoiceRuleFilterDetails(Guid articleId)
        //{
        //    using (var context = _dataAccessService.CreateDbContext())
        //    {
        //        var article = context.Articles.FirstOrDefault(c => c.ArticleId == articleId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

        //        var articleDetails = article.ToInvoiceRuleFilterDetails();
        //        SetInvoiceRuleFilterDetailsSecurity(articleDetails);
        //        return articleDetails;
        //    }
        //}

        public void InsertInvoiceRuleFilterDetails(BaseInvoiceRuleFilterDetails articleDetails)
        {
            var article = articleDetails.ToArticle(CurrentAuthenticatedFLSUserClubId);
            article.EntityNotNull("Article", Guid.Empty);

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to insert a new article!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Articles.Add(article);
                context.SaveChanges();
            }

            //Map it back to details
            article.ToInvoiceRuleFilterDetails(articleDetails);
        }
        public void UpdateInvoiceRuleFilterDetails(BaseInvoiceRuleFilterDetails currentInvoiceRuleFilterDetails)
        {
            currentInvoiceRuleFilterDetails.ArgumentNotNull("currentInvoiceRuleFilterDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Articles.FirstOrDefault(x => x.ArticleId == currentInvoiceRuleFilterDetails.ArticleId);
                original.EntityNotNull("Article", currentInvoiceRuleFilterDetails.ArticleId);

                currentInvoiceRuleFilterDetails.ToArticle(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToInvoiceRuleFilterDetails(currentInvoiceRuleFilterDetails);
                }
            }
        }

        public void DeleteArticle(Guid articleId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to delete a article!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Articles.FirstOrDefault(x => x.ArticleId == articleId);
                original.EntityNotNull("Article", articleId);

                context.Articles.Remove(original);
                context.SaveChanges();
            }
        }

        public Nullable<DateTime> GetLastArticleSynchronisationOn()
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to set the last article synchronisation datetime value!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var club = context.Clubs.FirstOrDefault(c => c.ClubId == CurrentAuthenticatedFLSUserClubId);
                club.EntityNotNull("Club");

                return club.LastArticleSynchronisationOn;
            }
        }

        #endregion InvoiceLineRuleFilter

        private void SetArticleOverviewSecurity(List<ArticleOverview> articleOverviewResult)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in articleOverviewResult)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in articleOverviewResult)
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

        private void SetInvoiceRuleFilterDetailsSecurity(BaseInvoiceRuleFilterDetails details)
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
