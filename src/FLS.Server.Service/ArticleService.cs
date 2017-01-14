using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Articles;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using NLog;

namespace FLS.Server.Service
{
    public class ArticleService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public ArticleService(DataAccessService dataAccessService,  
            IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }
        
        #region Article
        public List<ArticleOverview> GetArticleOverviews()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Article> articles = context.Articles.Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId).OrderBy(t => t.ArticleNumber).ToList();

                var overviewList = articles.Select(x => new ArticleOverview()
                {
                    ArticleId = x.ArticleId,
                    ArticleNumber = x.ArticleNumber,
                    ArticleName = x.ArticleName,
                    IsActive = x.IsActive
                }).ToList();

                SetArticleOverviewSecurity(overviewList);
                return overviewList;
            }
        }

        public PagedList<ArticleOverview> GetPagedArticleOverview(int? pageStart, int? pageSize, PageableSearchFilter<ArticleOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<ArticleOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new ArticleOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("CreatedOn", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var articles = context.Articles.Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                articles = articles.WhereIf(filter.ArticleNumber,
                        reservation => reservation.ArticleNumber.ToLower().Contains(filter.ArticleNumber.ToLower()));
                articles = articles.WhereIf(filter.ArticleName,
                    reservation => reservation.ArticleName.ToLower().Contains(filter.ArticleName.ToLower()));

                if (filter.IsActive.HasValue)
                    articles = articles.WhereIf(filter.IsActive.Value,
                        reservation => reservation.IsActive == filter.IsActive.Value);

                var pagedQuery = new PagedQuery<Article>(articles, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(x => new ArticleOverview()
                {
                    ArticleId = x.ArticleId,
                    ArticleNumber = x.ArticleNumber,
                    ArticleName = x.ArticleName,
                    IsActive = x.IsActive
                })
                .ToList();

                SetArticleOverviewSecurity(overviewList);

                var pagedList = new PagedList<ArticleOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public ArticleDetails GetArticleDetails(Guid articleId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var article = context.Articles.FirstOrDefault(c => c.ArticleId == articleId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var articleDetails = article.ToArticleDetails();
                SetArticleDetailsSecurity(articleDetails);
                return articleDetails;
            }
        }

        public void InsertArticleDetails(ArticleDetails articleDetails)
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
            article.ToArticleDetails(articleDetails);
        }
        public void UpdateArticleDetails(ArticleDetails currentArticleDetails)
        {
            currentArticleDetails.ArgumentNotNull("currentArticleDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Articles.FirstOrDefault(x => x.ArticleId == currentArticleDetails.ArticleId);
                original.EntityNotNull("Article", currentArticleDetails.ArticleId);

                currentArticleDetails.ToArticle(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToArticleDetails(currentArticleDetails);
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

        public void SetLastArticleSynchronisationOn(Nullable<DateTime> lastArticleSynchronisationOn)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to set the last article synchronisation datetime value!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var club = context.Clubs.FirstOrDefault(c => c.ClubId == CurrentAuthenticatedFLSUserClubId);
                club.EntityNotNull("Club");

                club.LastArticleSynchronisationOn = lastArticleSynchronisationOn;
                club.DoNotUpdateMetaData = true;

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }
        }
        #endregion Article

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

        private void SetArticleDetailsSecurity(ArticleDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("ArticleDetails is null while trying to set security properties"));
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



