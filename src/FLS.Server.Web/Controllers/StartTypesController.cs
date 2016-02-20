using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Flight;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for start type entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/starttypes")]
    public class StartTypesController : ApiController
    {
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTypesController"/> class.
        /// </summary>
        public StartTypesController(FlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Gets the start type list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<StartTypeListItem>))]
        public IHttpActionResult GetStartTypeListItems()
        {
            var startTypes = _flightService.GetStartTypeListItems();
            return Ok(startTypes);
        }

        /// <summary>
        /// Gets the start type list items for gliders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliders")]
        [Route("gliders/listitems")]
        [ResponseType(typeof(List<StartTypeListItem>))]
        public IHttpActionResult GetGliderStartTypeListItems()
        {
            var startTypes = _flightService.GetGliderStartTypeListItems();
            return Ok(startTypes);
        }

        /// <summary>
        /// Gets the start type list items for motor flights.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("motor")]
        [Route("motor/listitems")]
        [ResponseType(typeof(List<StartTypeListItem>))]
        public IHttpActionResult GetMotorStartTypeListItems()
        {
            var startTypes = _flightService.GetMotorStartTypeListItems();
            return Ok(startTypes);
        }
    }
}
