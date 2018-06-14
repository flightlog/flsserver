using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Registrations;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{

    /// <summary>
    /// Trial flights registrations controller
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/trialflightsregistrations")]
    public class TrialFlightsRegistrationsController : ApiController
    {
        private readonly RegistrationService _registrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrialFlightsRegistrationsController"/> class.
        /// </summary>
        /// <param name="registrationService">The registration service.</param>
        public TrialFlightsRegistrationsController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// Gets the trial flight dates which are available.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("availabledates/{clubKey}")]
        [ResponseType(typeof(List<DateTime>))]
        public IHttpActionResult GetAvailableTrialFlightDates(string clubKey)
        {
            var dates = _registrationService.GetTrialFlightsDates(clubKey);
            return Ok(dates);
        }

        /// <summary>
        /// Registers for a new trial flight.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RegisterForTrialFlight(TrialFlightRegistrationDetails trialFlightRegistrationDetails)
        {
            _registrationService.RegisterForTrialFlight(trialFlightRegistrationDetails);
            return Ok();
        }
    }
}
