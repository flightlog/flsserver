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
    /// Api controller for aircraft states.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircraftstates")]
    public class AircraftStatesController : ApiController
    {
        private readonly AircraftService _aircraftService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftStatesController"/> class.
        /// </summary>
        public AircraftStatesController(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// Gets the aircraft state list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<AircraftStateListItem>))]
        public IHttpActionResult GetAircraftStateListItems()
        {
            var aircrafts = _aircraftService.GetAircraftStateListItems();
            return Ok(aircrafts);
        }

    }
}
