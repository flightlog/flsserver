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
    /// Length unit types controller
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/lengthunittypes")]
    public class LengthUnitTypesController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthUnitTypesController"/> class.
        /// </summary>
        /// <param name="locationService">The location service.</param>
        public LengthUnitTypesController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the length unit types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<LengthUnitType>))]
        public IHttpActionResult GetLengthUnitTypes()
        {
            var lengthUnitTypes = _locationService.GetLengthUnitTypeListItems();
            return Ok(lengthUnitTypes);
        }
    }
}
