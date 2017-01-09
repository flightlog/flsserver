using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Server.Service;
using FLS.Server.WebApi.ActionFilters;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for location entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/locations")]
    public class LocationsController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsController"/> class.
        /// </summary>
        public LocationsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the location overviews.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<LocationOverview>))]
        public IHttpActionResult GetLocationOverviews()
        {
            var locations = _locationService.GetLocationOverviews(false);
            return Ok(locations);
        }

        /// <summary>
        /// Gets the location overviews.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{pageStart:int}/{pageSize:int}")]
        [ResponseType(typeof(PagedList<LocationOverview>))]
        public IHttpActionResult GetPagedLocationOverviews(int pageStart, int pageSize)
        {
            var locations = _locationService.GetPagedLocationOverviews(pageStart, pageSize, false);
            return Ok(locations);
        }

        /// <summary>
        /// Gets the location details.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{locationId}")]
        [ResponseType(typeof(LocationDetails))]
        public IHttpActionResult GetLocationDetails(Guid locationId)
        {
            var locationDetails = _locationService.GetLocationDetails(locationId);
            return Ok(locationDetails);
        }

        /// <summary>
        /// Inserts the specified location details.
        /// </summary>
        /// <param name="locationDetails">The location details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(LocationDetails))]
        public IHttpActionResult Insert([FromBody] LocationDetails locationDetails)
        {
            _locationService.InsertLocationDetails(locationDetails);
            return Ok(locationDetails);
        }

        // PUT api/locations/5
        /// <summary>
        /// Updates the specified location.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="locationDetails">The location details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{locationId}")]
        [ResponseType(typeof(LocationDetails))]
        public IHttpActionResult Update(Guid locationId, [FromBody]LocationDetails locationDetails)
        {
            _locationService.UpdateLocationDetails(locationDetails);
            return Ok(locationDetails);
        }

        /// <summary>
        /// Deletes the specified location.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{locationId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid locationId)
        {
            _locationService.DeleteLocation(locationId);
            return Ok();
        }
    }
}
