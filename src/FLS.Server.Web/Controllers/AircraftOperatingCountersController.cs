using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for aircraft operating counter entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircraftoperatingcounters")]
    public class AircraftOperatingCountersController : ApiController
    {
        private readonly AircraftService _aircraftService;
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftOperatingCountersController"/> class.
        /// </summary>
        public AircraftOperatingCountersController(AircraftService aircraftService, FlightService flightService)
        {
            _aircraftService = aircraftService;
            _flightService = flightService;
        }

        /// <summary>
        /// Calculates the engine operating counter based on available values of aircraft operating counter values and flights data.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("request")]
        [ResponseType(typeof(AircraftOperatingCounterResult))]
        public IHttpActionResult GetAircraftOperatingCounterOverviews(AircraftOperatingCounterRequest request)
        {
            var aircraftOperatingCounters = _flightService.GetAircraftOperatingCounterResult(request);
            return Ok(aircraftOperatingCounters);
        }

        /// <summary>
        /// Gets the aircraft operating counter overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("aircraft/{aircraftId}")]
        [Route("overview/aircraft/{aircraftId}")]
        [ResponseType(typeof(List<AircraftOperatingCounterOverview>))]
        public IHttpActionResult GetAircraftOperatingCounterOverviews(Guid aircraftId)
        {
            var aircraftOperatingCounters = _aircraftService.GetAircraftOperatingCounterOverviewByAircraftId(aircraftId);
            return Ok(aircraftOperatingCounters);
        }

        /// <summary>
        /// Gets the aircraft operating counter overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("immatriculation/{immatriculation}")]
        [Route("overview/immatriculation/{immatriculation}")]
        [ResponseType(typeof(List<AircraftOperatingCounterOverview>))]
        public IHttpActionResult GetAircraftOperatingCounterOverviews(string immatriculation)
        {
            var aircraftOperatingCounters = _aircraftService.GetAircraftOperatingCounterOverviewByImmatriculation(immatriculation);
            return Ok(aircraftOperatingCounters);
        }

        /// <summary>
        /// Gets the aircraft operating counter details.
        /// </summary>
        /// <param name="aircraftOperatingCounterId">The aircraft operating counter identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{aircraftOperatingCounterId}")]
        [ResponseType(typeof(AircraftOperatingCounterDetails))]
        public IHttpActionResult GetAircraftOperatingCounterDetails(Guid aircraftOperatingCounterId)
        {
            var details = _aircraftService.GetAircraftOperatingCounterDetails(aircraftOperatingCounterId);
            return Ok(details);
        }

        /// <summary>
        /// Inserts the specified aircraft operating counter details.
        /// </summary>
        /// <param name="aircraftOperatingCounterDetails">The aircraft operating counter details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AircraftOperatingCounterDetails))]
        public IHttpActionResult Insert([FromBody] AircraftOperatingCounterDetails aircraftOperatingCounterDetails)
        {
            _aircraftService.InsertAircraftOperatingCounterDetails(aircraftOperatingCounterDetails);
            return Ok(aircraftOperatingCounterDetails);
        }

        /// <summary>
        /// Updates the specified aircraft operating counter.
        /// </summary>
        /// <param name="aircraftOperatingCounterId">The aircraft operating counter identifier.</param>
        /// <param name="aircraftOperatingCounterDetails">The aircraft operating counter details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{aircraftOperatingCounterId}")]
        [ResponseType(typeof(AircraftOperatingCounterDetails))]
        public IHttpActionResult Update(Guid aircraftOperatingCounterId, [FromBody]AircraftOperatingCounterDetails aircraftOperatingCounterDetails)
        {
            _aircraftService.UpdateAircraftOperatingCounterDetails(aircraftOperatingCounterDetails);
            return Ok(aircraftOperatingCounterDetails);
        }

        /// <summary>
        /// Deletes the specified aircraft operating counter.
        /// </summary>
        /// <param name="aircraftOperatingCounterId">The aircraft operating counter identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{aircraftOperatingCounterId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid aircraftOperatingCounterId)
        {
            _aircraftService.DeleteAircraftOperatingCounterDetails(aircraftOperatingCounterId);
            return Ok();
        }
    }
}
