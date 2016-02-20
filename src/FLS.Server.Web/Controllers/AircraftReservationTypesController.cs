using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for aircraft reservation type entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircraftreservationtypes")]
    public class AircraftReservationTypesController : ApiController
    {
        private readonly AircraftReservationService _aircraftReservationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftReservationTypesController"/> class.
        /// </summary>
        public AircraftReservationTypesController(AircraftReservationService aircraftReservationService)
        {
            _aircraftReservationService = aircraftReservationService;
        }

        /// <summary>
        /// Gets the aircraft reservation type overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<AircraftReservationTypeListItem>))]
        public IHttpActionResult GetAircraftReservationTypeListItems()
        {
            var items = _aircraftReservationService.GetAircraftReservationTypeListItems();
            return Ok(items);
        }
    }
}
