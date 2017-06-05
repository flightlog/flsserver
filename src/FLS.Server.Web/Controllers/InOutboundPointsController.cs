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
    /// Api controller for InOutboundPoints entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/inoutboundpoints")]
    public class InOutboundPointsController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InOutboundPointsController"/> class.
        /// </summary>
        public InOutboundPointsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the InOutboundPoints of a location.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("location/{locationId}")]
        [ResponseType(typeof(List<InOutboundPointDetails>))]
        public IHttpActionResult GetInOutboundPointDetailsByLocationId(Guid locationId)
        {
            var items = _locationService.GetInOutboundPointDetailsByLocationId(locationId);
            return Ok(items);
        }

        /// <summary>
        /// Gets an specific InOutboundPoint.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{inOutboundPointId}")]
        [ResponseType(typeof(InOutboundPointDetails))]
        public IHttpActionResult GetInOutboundPointDetails(Guid inOutboundPointId)
        {
            var item = _locationService.GetInOutboundPointDetails(inOutboundPointId);
            return Ok(item);
        }

        /// <summary>
        /// Inserts the specified InOutboundPoint details.
        /// </summary>
        /// <param name="inOutboundPointDetails">The InOutboundPoint details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(LocationDetails))]
        public IHttpActionResult Insert([FromBody] InOutboundPointDetails inOutboundPointDetails)
        {
            _locationService.InsertInOutboundPointDetails(inOutboundPointDetails);
            return Ok(inOutboundPointDetails);
        }

        /// <summary>
        /// Updates the specified inOutboundPointDetails.
        /// </summary>
        /// <param name="inOutboundPointId">The location identifier.</param>
        /// <param name="inOutboundPointDetails">The InOutboundPointDetails.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{inOutboundPointId}")]
        [ResponseType(typeof(InOutboundPointDetails))]
        public IHttpActionResult Update(Guid inOutboundPointId, [FromBody]InOutboundPointDetails inOutboundPointDetails)
        {
            _locationService.UpdateInOutboundPointDetails(inOutboundPointDetails);
            return Ok(inOutboundPointDetails);
        }

        /// <summary>
        /// Deletes the specified InOutboundPoint.
        /// </summary>
        /// <param name="inOutboundPointId">The InOutboundPoint identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{inOutboundPointId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid inOutboundPointId)
        {
            _locationService.DeleteInOutboundPoint(inOutboundPointId);
            return Ok();
        }
    }
}
