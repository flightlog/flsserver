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
    /// Passenger flights registrations controller
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/passengerflightsregistrations")]
    public class PassengerFlightsRegistrationsController : ApiController
    {
        private readonly RegistrationService _registrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassengerFlightsRegistrationsController"/> class.
        /// </summary>
        /// <param name="registrationService">The registration service.</param>
        public PassengerFlightsRegistrationsController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        
        /// <summary>
        /// Registers for a new passenger flight.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RegisterForPassengerFlight(PassengerFlightRegistrationDetails passengerFlightRegistrationDetails)
        {
            _registrationService.RegisterForPassengerFlight(passengerFlightRegistrationDetails);
            return Ok();
        }
    }
}
