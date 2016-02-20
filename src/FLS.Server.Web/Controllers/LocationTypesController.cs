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
    /// Api controller for location type entities.
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/locationtypes")]
    public class LocationTypesController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypesController"/> class.
        /// </summary>
        public LocationTypesController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the location types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<LocationTypeListItem>))]
        public IHttpActionResult GetLocationTypes()
        {
            var locations = _locationService.GetLocationTypeListItems();
            return Ok(locations);
        }
    }
}
