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
    /// Api controller for flight state entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flightstates")]
    public class FlightStatesController : ApiController
    {
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightStatesController"/> class.
        /// </summary>
        public FlightStatesController(FlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Gets the flight state list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<FlightStateListItem>))]
        public IHttpActionResult GetFlightStateListItems()
        {
            var flightStates = _flightService.GetFlightStateListItems();
            return Ok(flightStates);
        }
    }
}
