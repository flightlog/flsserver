using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Processing;
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
        /// Gets the flight air state list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("air")]
        [Route("listitems")]
        [Route("air/listitems")]
        [ResponseType(typeof(List<FlightStateListItem>))]
        public IHttpActionResult GetFlightAirStateListItems()
        {
            var flightStates = _flightService.GetFlightAirStateListItems();
            return Ok(flightStates);
        }
        
        /// <summary>
        /// Gets the flight process state list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("process")]
        [Route("process/listitems")]
        [ResponseType(typeof(List<FlightStateListItem>))]
        public IHttpActionResult GetFlightProcessStateListItems()
        {
            var flightStates = _flightService.GetFlightProcessStateListItems();
            return Ok(flightStates);
        }

        /// <summary>
        /// Gets the flight process state actions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("processactions")]
        [ResponseType(typeof(List<FlightProcessAction>))]
        public IHttpActionResult GetFlightProcessActions()
        {
            var flightProcessActions = _flightService.GetFlightProcessActions();
            return Ok(flightProcessActions);
        }
    }
}
