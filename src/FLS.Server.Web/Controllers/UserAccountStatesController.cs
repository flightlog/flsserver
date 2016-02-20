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
    /// Api controller for user account state entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/useraccountstates")]
    public class UserAccountStatesController : ApiController
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountStatesController"/> class.
        /// </summary>
        public UserAccountStatesController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets the user account state list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<UserAccountStateListItem>))]
        public IHttpActionResult GetUserAccountStateListItems()
        {
            var userAccountStates = _userService.GetUserAccountStateListItems();
            return Ok(userAccountStates);
        }
    }
}
