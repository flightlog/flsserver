using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for flight type entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flighttypes")]
    public class FlightTypesController : ApiController
    {
        private readonly ClubService _clubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightTypesController"/> class.
        /// </summary>
        public FlightTypesController(ClubService clubService)
        {
            _clubService = clubService;
        }

        /// <summary>
        /// Gets the flight type overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<FlightTypeOverview>))]
        public IHttpActionResult GetFlightTypeOverviews()
        {
            var flightTypes = _clubService.GetFlightTypeOverviews();
            return Ok(flightTypes);
        }

        /// <summary>
        /// Gets the glider flight type overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliders")]
        [Route("gliders/overview")]
        [ResponseType(typeof(List<FlightTypeOverview>))]
        public IHttpActionResult GetGliderFlightTypeOverviews()
        {
            var flightTypes = _clubService.GetGliderFlightTypeOverviews();
            return Ok(flightTypes);
        }

        /// <summary>
        /// Gets the towing flight type overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("towing")]
        [Route("towing/overview")]
        [ResponseType(typeof(List<FlightTypeOverview>))]
        public IHttpActionResult GetTowingFlightTypeOverviews()
        {
            var flightTypes = _clubService.GetTowingFlightTypeOverviews();
            return Ok(flightTypes);
        }

        /// <summary>
        /// Gets the motor flight type overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("motor")]
        [Route("motor/overview")]
        [ResponseType(typeof(List<FlightTypeOverview>))]
        public IHttpActionResult GetMotorFlightTypeOverviews()
        {
            var flightTypes = _clubService.GetMotorFlightTypeOverviews();
            return Ok(flightTypes);
        }

        #region future implementation for sys-admins
        ///// <summary>
        ///// Gets the flight type overviews of a defined club.
        ///// <remarks>Method is only available for Sys-Admins</remarks>
        ///// </summary>
        ///// <param name="clubId">The club identifier.</param>
        ///// <returns></returns>
        //[Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        //[HttpGet]
        //[Route("club/{clubId}")]
        //[Route("overview/club/{clubId}")]
        //[ResponseType(typeof(List<FlightTypeOverview>))]
        //public IHttpActionResult GetFlightTypeOverviews(Guid clubId)
        //{
        //    var flightTypes = _clubService.GetFlightTypeOverviews(clubId);
        //    return Ok(flightTypes);
        //}

        ///// <summary>
        ///// Gets the glider flight type overviews.
        ///// </summary>
        ///// <param name="clubId">The club identifier.</param>
        ///// <returns></returns>
        //[Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        //[HttpGet]
        //[Route("gliders/club/{clubId}")]
        //[Route("gliders/overview/club/{clubId}")]
        //[ResponseType(typeof(List<FlightTypeOverview>))]
        //public IHttpActionResult GetGliderFlightTypeOverviews(Guid clubId)
        //{
        //    var flightTypes = _clubService.GetGliderFlightTypeOverviews(clubId);
        //    return Ok(flightTypes);
        //}

        ///// <summary>
        ///// Gets the towing flight type overviews.
        ///// </summary>
        ///// <param name="clubId">The club identifier.</param>
        ///// <returns></returns>
        //[Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        //[HttpGet]
        //[Route("towing/club/{clubId}")]
        //[Route("towing/overview/club/{clubId}")]
        //[ResponseType(typeof(List<FlightTypeOverview>))]
        //public IHttpActionResult GetTowingFlightTypeOverviews(Guid clubId)
        //{
        //    var flightTypes = _clubService.GetTowingFlightTypeOverviews(clubId);
        //    return Ok(flightTypes);
        //}

        ///// <summary>
        ///// Gets the motor flight type overviews.
        ///// </summary>
        ///// <param name="clubId">The club identifier.</param>
        ///// <returns></returns>
        //[Authorize(Roles = RoleApplicationKeyStrings.SystemAdministrator)]
        //[HttpGet]
        //[Route("motor/club/{clubId}")]
        //[Route("motor/overview/club/{clubId}")]
        //[ResponseType(typeof(List<FlightTypeOverview>))]
        //public IHttpActionResult GetMotorFlightTypeOverviews(Guid clubId)
        //{
        //    var flightTypes = _clubService.GetMotorFlightTypeOverviews(clubId);
        //    return Ok(flightTypes);
        //}
        #endregion future implementation for sys-admins

        /// <summary>
        /// Gets the flight type details.
        /// </summary>
        /// <param name="flightTypeId">The flight type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{flightTypeId}")]
        [ResponseType(typeof(FlightTypeDetails))]
        public IHttpActionResult GetFlightTypeDetails(Guid flightTypeId)
        {
            var flightTypeDetails = _clubService.GetFlightTypeDetails(flightTypeId);
            return Ok(flightTypeDetails);
        }

        /// <summary>
        /// Inserts the specified flight type details.
        /// </summary>
        /// <param name="flightTypeDetails">The flight type details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(FlightTypeDetails))]
        public IHttpActionResult Insert([FromBody] FlightTypeDetails flightTypeDetails)
        {
            _clubService.InsertFlightTypeDetails(flightTypeDetails);
            return Ok(flightTypeDetails);
        }

        /// <summary>
        /// Updates the specified flight type identifier.
        /// </summary>
        /// <param name="flightTypeId">The flight type identifier.</param>
        /// <param name="flightTypeDetails">The flight type details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPut]
        [Route("{flightTypeId}")]
        [ResponseType(typeof(FlightTypeDetails))]
        public IHttpActionResult Update(Guid flightTypeId, [FromBody]FlightTypeDetails flightTypeDetails)
        {
            _clubService.UpdateFlightTypeDetails(flightTypeDetails);
            return Ok(flightTypeDetails);
        }

        /// <summary>
        /// Deletes the specified flight type identifier.
        /// </summary>
        /// <param name="flightTypeId">The flight type identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpDelete]
        [Route("{flightTypeId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid flightTypeId)
        {
            _clubService.DeleteFlightType(flightTypeId);
            return Ok();
        }
    }
}
