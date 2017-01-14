using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for aircraft entities
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/aircrafts")]
    public class AircraftsController : ApiController
    {
        private readonly AircraftService _aircraftService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftsController"/> class.
        /// </summary>
        public AircraftsController(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// Gets the glider aircraft list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliders/listitems")]
        [Route("listitems/gliders")]
        [ResponseType(typeof(List<AircraftListItem>))]
        public IHttpActionResult GetGliderAircraftListItems()
        {
            var aircrafts = _aircraftService.GetGliderAircraftListItems();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the towing aircraft list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("towingaircrafts/listitems")]
        [Route("listitems/towingaircrafts")]
        [ResponseType(typeof(List<AircraftListItem>))]
        public IHttpActionResult GetTowingAircraftListItems()
        {
            var aircrafts = _aircraftService.GetTowingAircraftListItems();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the motor aircraft list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("motoraircrafts/listitems")]
        [Route("listitems/motoraircrafts")]
        [ResponseType(typeof(List<AircraftListItem>))]
        public IHttpActionResult GetMotorAircraftListItems()
        {
            var aircrafts = _aircraftService.GetMotorAircraftListItems();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the aircraft overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<AircraftOverview>))]
        public IHttpActionResult GetAircraftOverviews()
        {
            var aircrafts = _aircraftService.GetAircraftOverviews();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the aircraft overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<AircraftOverview>))]
        public IHttpActionResult GetPagedLocationOverviews([FromBody]PageableSearchFilter<AircraftOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var aircrafts = _aircraftService.GetPagedAircraftOverviews(pageStart, pageSize, pageableSearchFilter);
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the glider aircraft overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliders")]
        [Route("gliders/overview")]
        [Route("overview/gliders")]
        [ResponseType(typeof(List<AircraftOverview>))]
        public IHttpActionResult GetGliderAircraftOverviews()
        {
            var aircrafts = _aircraftService.GetGliderAircraftOverviews();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the towing aircraft overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("towingaircrafts")]
        [Route("towingaircrafts/overview")]
        [Route("overview/towingaircrafts")]
        [ResponseType(typeof(List<AircraftOverview>))]
        public IHttpActionResult GetTowingAircraftOverviews()
        {
            var aircrafts = _aircraftService.GetTowingAircraftOverviews();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the aircraft overviews.
        /// </summary>
        /// <param name="aircraftType">Type of the aircraft.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("type/{aircraftType}")]
        [Route("overview/type/{aircraftType}")]
        [Route("overview/aircrafttype/{aircraftType}")]
        [ResponseType(typeof(List<AircraftOverview>))]
        public IHttpActionResult GetAircraftOverviews(int aircraftType)
        {
            var aircrafts = _aircraftService.GetAircraftOverviews(aircraftType);
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets the aircraft details.
        /// </summary>
        /// <param name="aircraftId">The aircraft identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{aircraftId}")]
        [ResponseType(typeof(AircraftDetails))]
        public IHttpActionResult GetAircraftDetails(Guid aircraftId)
        {
            var aircraftDetails = _aircraftService.GetAircraftDetails(aircraftId);
            return Ok(aircraftDetails);
        }

        /// <summary>
        /// Gets the aircraft details.
        /// </summary>
        /// <param name="immatriculation">The immatriculation.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("immatriculation/{immatriculation}")]
        [ResponseType(typeof(AircraftDetails))]
        public IHttpActionResult GetAircraftDetails(string immatriculation)
        {
            var aircraftDetails = _aircraftService.GetAircraftDetails(immatriculation);
            return Ok(aircraftDetails);
        }

        /// <summary>
        /// Inserts the specified aircraft.
        /// </summary>
        /// <param name="aircraftDetails">The aircraft details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AircraftDetails))]
        public IHttpActionResult Insert([FromBody] AircraftDetails aircraftDetails)
        {
            _aircraftService.InsertAircraftDetails(aircraftDetails);
            return Ok(aircraftDetails);
        }

        /// <summary>
        /// Updates the specified aircraft.
        /// </summary>
        /// <param name="aircraftId">The aircraft identifier.</param>
        /// <param name="aircraftDetails">The aircraft details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{aircraftId}")]
        [ResponseType(typeof(AircraftDetails))]
        public IHttpActionResult Update(Guid aircraftId, [FromBody]AircraftDetails aircraftDetails)
        {
            _aircraftService.UpdateAircraftDetails(aircraftDetails);
            return Ok(aircraftDetails);
        }

        /// <summary>
        /// Deletes the specified aircraft.
        /// </summary>
        /// <param name="aircraftId">The aircraft identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{aircraftId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid aircraftId)
        {
            _aircraftService.DeleteAircraft(aircraftId);
            return Ok();
        }
    }
}
