using System;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.System;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for SystemData entities.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
    [RoutePrefix("api/v1/systemdatas")]
    public class SystemDatasController : ApiController
    {
        private readonly SystemService _systemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDatasController"/> class.
        /// </summary>
        public SystemDatasController(SystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Gets the SystemData details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(SystemDataDetails))]
        public IHttpActionResult GetSystemDataDetails()
        {
            var systemDataDetails = _systemService.GetSystemDataDetails();
            return Ok(systemDataDetails);
        }

        /// <summary>
        /// Updates the specified SystemData.
        /// </summary>
        /// <param name="systemDataId">The SystemData identifier.</param>
        /// <param name="systemDataDetails">The SystemData details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{systemDataId}")]
        [ResponseType(typeof(SystemDataDetails))]
        public IHttpActionResult Update(Guid systemDataId, [FromBody]SystemDataDetails systemDataDetails)
        {
            _systemService.UpdateSystemDataDetails(systemDataDetails);
            return Ok(systemDataDetails);
        }
    }
}
