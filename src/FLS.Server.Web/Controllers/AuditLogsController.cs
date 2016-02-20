using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Audit;
using FLS.Data.WebApi.Dashboard;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for the users dashboard
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/auditlogs")]
    public class AuditLogsController : ApiController
    {
        private readonly SystemService _systemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogsController"/> class.
        /// </summary>
        public AuditLogsController(SystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Gets a list of tracked entity names.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("trackedentities")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult GetTrackedEntityNames()
        {
            var items = _systemService.GetTrackedEntityNames();
            return Ok(items);
        }

        /// <summary>
        /// Gets a list of AuditLogOverview for the related entities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{entityName}")]
        [Route("overview/{entityName}")]
        [ResponseType(typeof(List<AuditLogOverview>))]
        public IHttpActionResult GetAuditLogOverviews(string entityName)
        {
            var items = _systemService.GetAuditLogOverviews(entityName);
            return Ok(items);
        }

        /// <summary>
        /// Gets a list of AuditLogOverview for the related entity with the referenced record Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{entityName}/{recordId}")]
        [Route("overview/{entityName}/{recordId}")]
        [ResponseType(typeof(List<AuditLogOverview>))]
        public IHttpActionResult GetAuditLogOverviews(string entityName, Guid recordId)
        {
            var items = _systemService.GetAuditLogOverviews(entityName, recordId);
            return Ok(items);
        }
    }
}
