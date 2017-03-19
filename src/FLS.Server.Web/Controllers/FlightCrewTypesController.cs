using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Flight;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for flight crew types.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flightcrewtypes")]
    public class FlightCrewTypesController : ApiController
    {
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightCrewTypesController"/> class.
        /// </summary>
        public FlightCrewTypesController(FlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Gets the flight crew type list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listitems")]
        [ResponseType(typeof(List<FlightCrewTypeListItem>))]
        public IHttpActionResult GetFlightCrewTypeListItems()
        {
            var items = _flightService.GetFlightCrewTypeListItems();
            return Ok(items);
        }

    }
}
