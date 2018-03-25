using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Resources;
using FLS.Data.WebApi.System;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for aircraft types.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
    [RoutePrefix("api/v1/systemlogs")]
    public class SystemLogsController : ApiController
    {
        private readonly SystemService _systemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemLogsController"/> class.
        /// </summary>
        public SystemLogsController(SystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Gets the systemlog overview.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<SystemLogOverview>))]
        public IHttpActionResult GetSystemLogOverview()
        {
            var systemLogs = _systemService.GetSystemLogOverviews();
            return Ok(systemLogs);
        }

        /// <summary>
        /// Gets the systemlog overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<SystemLogOverview>))]
        public IHttpActionResult GetPagedSystemLogOverview([FromBody]PageableSearchFilter<SystemLogOverviewSearchFilter> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var systemLogs = _systemService.GetPagedSystemLogOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(systemLogs);
        }

        /// <summary>
        /// Gets the systemlog details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{logId}")]
        [ResponseType(typeof(SystemLogDetails))]
        public IHttpActionResult GetSystemLogDetails(long logId)
        {
            var systemLog = _systemService.GetSystemLogDetails(logId);
            return Ok(systemLog);
        }
    }
}
