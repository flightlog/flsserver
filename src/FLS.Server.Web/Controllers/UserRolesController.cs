using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.User;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for user role entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/userroles")]
    public class UserRolesController : ApiController
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesController"/> class.
        /// </summary>
        public UserRolesController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets the role overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<RoleOverview>))]
        public IHttpActionResult GetRoleOverviews()
        {
            var userRoles = _userService.GetRoleOverviews();
            return Ok(userRoles);
        }
    }
}
