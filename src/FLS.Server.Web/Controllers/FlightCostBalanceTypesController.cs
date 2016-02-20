using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Flight;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for flight cost balance type entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flightcostbalancetypes")]
    public class FlightCostBalanceTypesController : ApiController
    {
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightCostBalanceTypesController"/> class.
        /// </summary>
        public FlightCostBalanceTypesController(FlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Gets the flight cost balance type list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<FlightCostBalanceTypeListItem>))]
        public IHttpActionResult GetFlightCostBalanceTypeListItems()
        {
            var flightCostBalanceTypes = _flightService.GetFlightCostBalanceTypeListItems();
            return Ok(flightCostBalanceTypes);
        }
    }
}
