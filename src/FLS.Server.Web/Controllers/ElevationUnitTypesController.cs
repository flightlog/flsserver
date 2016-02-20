using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Location;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for elevation unit type entities.
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/elevationunittypes")]
    public class ElevationUnitTypesController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevationUnitTypesController"/> class.
        /// </summary>
        /// <param name="locationService">The location service.</param>
        public ElevationUnitTypesController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the elevation unit types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<ElevationUnitType>))]
        public IHttpActionResult GetElevationUnitTypes()
        {
            var locations = _locationService.GetElevationUnitTypeListItems();
            return Ok(locations);
        }
    }
}
