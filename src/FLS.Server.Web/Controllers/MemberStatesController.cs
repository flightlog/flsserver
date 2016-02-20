using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for memberState entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/memberstates")]
    public class MemberStatesController : ApiController
    {
        private readonly ClubService _clubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberStatesController"/> class.
        /// </summary>
        public MemberStatesController(ClubService clubService)
        {
            _clubService = clubService;
        }

        /// <summary>
        /// Gets the memberState overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<MemberStateOverview>))]
        public IHttpActionResult GetMemberStateOverviews()
        {
            var memberStates = _clubService.GetMemberStateOverviews();
            return Ok(memberStates);
        }
        
        /// <summary>
        /// Gets the memberState details.
        /// </summary>
        /// <param name="memberStateId">The memberState identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{memberStateId}")]
        [ResponseType(typeof(MemberStateDetails))]
        public IHttpActionResult GetMemberStateDetails(Guid memberStateId)
        {
            var memberStateDetails = _clubService.GetMemberStateDetails(memberStateId);
            return Ok(memberStateDetails);
        }

        /// <summary>
        /// Inserts the specified memberState details.
        /// </summary>
        /// <param name="memberStateDetails">The memberState details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(MemberStateDetails))]
        public IHttpActionResult Insert([FromBody] MemberStateDetails memberStateDetails)
        {
            _clubService.InsertMemberStateDetails(memberStateDetails);
            return Ok(memberStateDetails);
        }

        /// <summary>
        /// Updates the specified memberState.
        /// </summary>
        /// <param name="memberStateId">The memberState identifier.</param>
        /// <param name="memberStateDetails">The memberState details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPut]
        [Route("{memberStateId}")]
        [ResponseType(typeof(MemberStateDetails))]
        public IHttpActionResult Update(Guid memberStateId, [FromBody]MemberStateDetails memberStateDetails)
        {
            _clubService.UpdateMemberStateDetails(memberStateDetails);
            return Ok(memberStateDetails);
        }

        /// <summary>
        /// Deletes the specified memberState.
        /// </summary>
        /// <param name="memberStateId">The memberState identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpDelete]
        [Route("{memberStateId}")]
        public IHttpActionResult Delete(Guid memberStateId)
        {
            _clubService.DeleteMemberState(memberStateId);
            return Ok();
        }
    }
}
