using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for aircraft types.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircrafttypes")]
    public class AircraftTypesController : ApiController
    {
        private readonly AircraftService _aircraftService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftTypesController"/> class.
        /// </summary>
        public AircraftTypesController(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// Gets the aircraft type list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<AircraftTypeListItem>))]
        public IHttpActionResult GetAircraftTypeListItems()
        {
            var aircrafts = _aircraftService.GetAircraftTypeListItems();
            return Ok(aircrafts);
        }

    }
}
