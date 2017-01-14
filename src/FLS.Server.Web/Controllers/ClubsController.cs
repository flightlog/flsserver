using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Articles;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for club entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/clubs")]
    public class ClubsController : ApiController
    {
        private readonly ClubService _clubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubsController"/> class.
        /// </summary>
        public ClubsController(ClubService clubService)
        {
            _clubService = clubService;
        }

        /// <summary>
        /// Gets the club overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<ClubOverview>))]
        public IHttpActionResult GetClubOverviews()
        {
            var clubs = _clubService.GetClubOverviews();
            return Ok(clubs);
        }

        /// <summary>
        /// Gets the club overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<ClubOverview>))]
        public IHttpActionResult GetPagedClubOverview([FromBody]PageableSearchFilter<ClubOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var clubs = _clubService.GetPagedClubOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(clubs);
        }

        /// <summary>
        /// Gets my club details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        [Route("own")]
        [ResponseType(typeof(ClubDetails))]
        public IHttpActionResult GetMyClubDetails()
        {
            var clubDetails = _clubService.GetMyClubDetails();
            return Ok(clubDetails);
        }

        /// <summary>
        /// Gets the club details.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clubId}")]
        [ResponseType(typeof(ClubDetails))]
        public IHttpActionResult GetClubDetails(Guid clubId)
        {
            var clubDetails = _clubService.GetClubDetails(clubId);
            return Ok(clubDetails);
        }

        /// <summary>
        /// Inserts the specified club details.
        /// </summary>
        /// <param name="clubDetails">The club details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(ClubDetails))]
        public IHttpActionResult Insert([FromBody] ClubDetails clubDetails)
        {
            _clubService.InsertClubDetails(clubDetails);
            return Ok(clubDetails);
        }

        /// <summary>
        /// Updates the specified club.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="clubDetails">The club details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{clubId}")]
        [ResponseType(typeof(ClubDetails))]
        public IHttpActionResult Update(Guid clubId, [FromBody]ClubDetails clubDetails)
        {
            _clubService.UpdateClubDetails(clubDetails);
            return Ok(clubDetails);
        }

        /// <summary>
        /// Deletes the specified club.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{clubId}")]
        public IHttpActionResult Delete(Guid clubId)
        {
            _clubService.DeleteClub(clubId);
            return Ok();
        }
    }
}
