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
    /// Api controller for aircraft reservation entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircraftreservations")]
    public class AircraftReservationsController : ApiController
    {
        private readonly AircraftReservationService _aircraftReservationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftReservationsController"/> class.
        /// </summary>
        public AircraftReservationsController(AircraftReservationService aircraftReservationService)
        {
            _aircraftReservationService = aircraftReservationService;
        }

        /// <summary>
        /// Gets the aircraft reservation overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<AircraftReservationOverview>))]
        public IHttpActionResult GetAircraftReservationOverviews()
        {
            var aircraftReservations = _aircraftReservationService.GetAircraftReservationOverview();
            return Ok(aircraftReservations);
        }

        /// <summary>
        /// Gets the aircraft reservation overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("future")]
        [Route("overview/future")]
        [ResponseType(typeof(List<AircraftReservationOverview>))]
        public IHttpActionResult GetFutureAircraftReservationOverviews()
        {
            var aircraftReservations = _aircraftReservationService.GetAircraftReservationOverview(DateTime.Now.Date);
            return Ok(aircraftReservations);
        }

        /// <summary>
        /// Gets the aircraft reservation overviews by day.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("day/{day:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<AircraftReservationOverview>))]
        public IHttpActionResult GetAircraftReservationOverviewsOfDay(DateTime day)
        {
            var planningDays = _aircraftReservationService.GetAircraftReservationOverviewOfDay(day);
            return Ok(planningDays);
        }

        /// <summary>
        /// Gets the aircraft reservation details.
        /// </summary>
        /// <param name="aircraftReservationId">The aircraft reservation identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{aircraftReservationId}")]
        [ResponseType(typeof(AircraftReservationDetails))]
        public IHttpActionResult GetAircraftReservationDetails(Guid aircraftReservationId)
        {
            var details = _aircraftReservationService.GetAircraftReservationDetails(aircraftReservationId);
            return Ok(details);
        }

        /// <summary>
        /// Gets the aircraft reservations by planning day id.
        /// </summary>
        /// <param name="planningDayId">The planning day identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("planningday/{planningDayId}")]
        [ResponseType(typeof(List<AircraftReservationOverview>))]
        public IHttpActionResult GetAircraftReservationsByPlanningDayId(Guid planningDayId)
        {
            var details = _aircraftReservationService.GetAircraftReservationsByPlanningDayId(planningDayId);
            return Ok(details);
        }
        
        /// <summary>
        /// Inserts the specified aircraft reservation details.
        /// </summary>
        /// <param name="aircraftReservationDetails">The aircraft reservation details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AircraftReservationDetails))]
        public IHttpActionResult Insert([FromBody] AircraftReservationDetails aircraftReservationDetails)
        {
            _aircraftReservationService.InsertAircraftReservationDetails(aircraftReservationDetails);
            return Ok(aircraftReservationDetails);
        }

        /// <summary>
        /// Updates the specified aircraft reservation.
        /// </summary>
        /// <param name="aircraftReservationId">The aircraft reservation identifier.</param>
        /// <param name="aircraftReservationDetails">The aircraft reservation details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{aircraftReservationId}")]
        [ResponseType(typeof(AircraftReservationDetails))]
        public IHttpActionResult Update(Guid aircraftReservationId, [FromBody]AircraftReservationDetails aircraftReservationDetails)
        {
            _aircraftReservationService.UpdateAircraftReservationDetails(aircraftReservationDetails);
            return Ok(aircraftReservationDetails);
        }

        /// <summary>
        /// Deletes the specified aircraft reservation.
        /// </summary>
        /// <param name="aircraftReservationId">The aircraft reservation identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{aircraftReservationId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid aircraftReservationId)
        {
            _aircraftReservationService.DeleteAircraftReservationDetails(aircraftReservationId);
            return Ok();
        }
    }
}
