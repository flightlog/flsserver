using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Articles;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for article entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
    [RoutePrefix("api/v1/articles")]
    public class ArticlesController : ApiController
    {
        private readonly ArticleService _articleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlesController"/> class.
        /// </summary>
        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Gets the article overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<ArticleOverview>))]
        public IHttpActionResult GetArticleOverviews()
        {
            var articles = _articleService.GetArticleOverviews();
            return Ok(articles);
        }

        /// <summary>
        /// Gets the article overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<ArticleOverview>))]
        public IHttpActionResult GetPagedArticleOverview([FromBody]PageableSearchFilter<ArticleOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var articles = _articleService.GetPagedArticleOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(articles);
        }

        /// <summary>
        /// Gets the article details.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{articleId}")]
        [ResponseType(typeof(ArticleDetails))]
        public IHttpActionResult GetArticleDetails(Guid articleId)
        {
            var articleDetails = _articleService.GetArticleDetails(articleId);
            return Ok(articleDetails);
        }

        /// <summary>
        /// Inserts the specified article details.
        /// </summary>
        /// <param name="articleDetails">The article details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(ArticleDetails))]
        public IHttpActionResult Insert([FromBody] ArticleDetails articleDetails)
        {
            _articleService.InsertArticleDetails(articleDetails);
            return Ok(articleDetails);
        }

        /// <summary>
        /// Updates the specified article identifier.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="articleDetails">The article details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{articleId}")]
        [ResponseType(typeof(ArticleDetails))]
        public IHttpActionResult Update(Guid articleId, [FromBody]ArticleDetails articleDetails)
        {
            _articleService.UpdateArticleDetails(articleDetails);
            return Ok(articleDetails);
        }

        /// <summary>
        /// Deletes the specified article identifier.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{articleId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid articleId)
        {
            _articleService.DeleteArticle(articleId);
            return Ok();
        }

        /// <summary>
        /// Gets the article synchronisation datetime stamp
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lastSync")]
        [ResponseType(typeof(Nullable<DateTime>))]
        public IHttpActionResult GetLastArticleSynchronisationOn()
        {
            var lastArticleSynchronisationOn = _articleService.GetLastArticleSynchronisationOn();
            return Ok(lastArticleSynchronisationOn);
        }

        /// <summary>
        /// Updates the article synchronisation datetime stamp
        /// </summary>
        /// <param name="lastArticleSynchronisationOn">The last article synchronisation date time.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("lastSync")]
        [ResponseType(typeof(Nullable<DateTime>))]
        public IHttpActionResult SetLastArticleSynchronisationOn([FromBody]Nullable<DateTime> lastArticleSynchronisationOn)
        {
            _articleService.SetLastArticleSynchronisationOn(lastArticleSynchronisationOn);
            return Ok(lastArticleSynchronisationOn);
        }
    }
}
