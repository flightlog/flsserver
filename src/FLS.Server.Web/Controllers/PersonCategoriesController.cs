using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for personCategory entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/personcategories")]
    public class PersonCategoriesController : ApiController
    {
        private readonly ClubService _clubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonCategoriesController"/> class.
        /// </summary>
        public PersonCategoriesController(ClubService clubService)
        {
            _clubService = clubService;
        }

        /// <summary>
        /// Gets the personCategory overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<PersonCategoryOverview>))]
        public IHttpActionResult GetPersonCategoryOverviews()
        {
            var personCategories = _clubService.GetPersonCategoryOverviews();
            return Ok(personCategories);
        }

        /// <summary>
        /// Gets the personCategory overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<PersonCategoryOverview>))]
        public IHttpActionResult GetPagedPersonCategoryOverview([FromBody]PageableSearchFilter<PersonCategoryOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var memberStates = _clubService.GetPagedPersonCategoryOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(memberStates);
        }

        /// <summary>
        /// Gets the personCategory details.
        /// </summary>
        /// <param name="personCategoryId">The personCategory identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{personCategoryId}")]
        [ResponseType(typeof(PersonCategoryDetails))]
        public IHttpActionResult GetPersonCategoryDetails(Guid personCategoryId)
        {
            var personCategoryDetails = _clubService.GetPersonCategoryDetails(personCategoryId);
            return Ok(personCategoryDetails);
        }

        /// <summary>
        /// Inserts the specified personCategory details.
        /// </summary>
        /// <param name="personCategoryDetails">The personCategory details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(PersonCategoryDetails))]
        public IHttpActionResult Insert([FromBody] PersonCategoryDetails personCategoryDetails)
        {
            _clubService.InsertPersonCategoryDetails(personCategoryDetails);
            return Ok(personCategoryDetails);
        }

        /// <summary>
        /// Updates the specified personCategory.
        /// </summary>
        /// <param name="personCategoryId">The personCategory identifier.</param>
        /// <param name="personCategoryDetails">The personCategory details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPut]
        [Route("{personCategoryId}")]
        [ResponseType(typeof(PersonCategoryDetails))]
        public IHttpActionResult Update(Guid personCategoryId, [FromBody]PersonCategoryDetails personCategoryDetails)
        {
            _clubService.UpdatePersonCategoryDetails(personCategoryDetails);
            return Ok(personCategoryDetails);
        }

        /// <summary>
        /// Deletes the specified personCategory.
        /// </summary>
        /// <param name="personCategoryId">The personCategory identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpDelete]
        [Route("{personCategoryId}")]
        public IHttpActionResult Delete(Guid personCategoryId)
        {
            _clubService.DeletePersonCategory(personCategoryId);
            return Ok();
        }
    }
}
