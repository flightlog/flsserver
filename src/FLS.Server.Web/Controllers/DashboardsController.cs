using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Dashboard;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for the users dashboard
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/dashboards")]
    public class DashboardsController : ApiController
    {
        private readonly DashboardService _dashboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardsController"/> class.
        /// </summary>
        public DashboardsController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets the aircraft type list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(DashboardDetails))]
        public IHttpActionResult GetDashboardDetails()
        {
            var dashboardDetails = _dashboardService.GetDashboardDetails();
            return Ok(dashboardDetails);
        }

    }
}
