using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.System;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for System version info.
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/systemversions")]
    public class SystemVersionsController : ApiController
    {
        private readonly SystemService _systemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemVersionsController"/> class.
        /// </summary>
        public SystemVersionsController(SystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Gets the system version info overview.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(SystemVersionInfoOverview))]
        public IHttpActionResult GetSystemVersionInfoOverview()
        {
            var systemVersionInfo = _systemService.GetSystemVersionInfoOverview();
            return Ok(systemVersionInfo);
        }

        /// <summary>
        /// Gets the system version info.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("details")]
        [ResponseType(typeof(SystemVersionInfoDetails))]
        public IHttpActionResult GetSystemVersionInfoDetails()
        {
            var systemVersionInfo = _systemService.GetSystemVersionInfoDetails();
            return Ok(systemVersionInfo);
        }
        
    }
}
