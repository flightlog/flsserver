using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for counter unit type entities.
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/counterunittypes")]
    public class CounterUnitTypesController : ApiController
    {
        private readonly AircraftService _aircraftService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterUnitTypesController"/> class.
        /// </summary>
        /// <param name="aircraftService">The aircraft service.</param>
        public CounterUnitTypesController(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// Gets the counter unit types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<CounterUnitTypeListItem>))]
        public IHttpActionResult GetCounterUnitTypes()
        {
            var unitTypes = _aircraftService.GetCounterUnitTypeListItems();
            return Ok(unitTypes);
        }
    }
}
