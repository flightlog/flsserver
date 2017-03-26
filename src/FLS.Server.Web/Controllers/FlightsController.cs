using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;
using FLS.Server.WebApi.ActionFilters;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for flight entities
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/flights")]
    public class FlightsController : ApiController
    {
        private readonly FlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Gets the flight overviews of all flights and flight types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetFlightOverviews()
        {
            var flights = _flightService.GetFlightOverviews();
            return Ok(flights);
        }

        /// <summary>
        /// Gets the glider flight overviews within today.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("today")]
        [Route("today/overview")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetFlightOverviewsWithinToday()
        {
            var flights = _flightService.GetFlightOverviewsWithinToday();
            return Ok(flights);
        }

        
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing
        /// <summary>
        /// Gets the flight overviews.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("daterange/{fromDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{toDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var flights = _flightService.GetFlightOverviews(fromDate, toDate);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the flight overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<FlightOverview>))]
        public IHttpActionResult GetPagedFlightOverview([FromBody]PageableSearchFilter<FlightOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var flights = _flightService.GetPagedFlightOverview(pageStart, pageSize, pageableSearchFilter, false);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the glider flight overviews with referenced tow flights.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderflights")]
        [Route("gliderflights/overview")]
        [ResponseType(typeof(List<GliderFlightOverview>))]
        public IHttpActionResult GetGliderFlightOverviews()
        {
            var flights = _flightService.GetGliderFlightOverviews();
            return Ok(flights);
        }
        
        /// <summary>
        /// Gets the glider flight overviews within today.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderflights/today")]
        [Route("gliderflights/today/overview")]
        [ResponseType(typeof(List<GliderFlightOverview>))]
        public IHttpActionResult GetGliderFlightOverviewsWithinToday()
        {
            var flights = _flightService.GetGliderFlightOverviewsWithinToday();
            return Ok(flights);
        }

        
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing
        /// <summary>
        /// Gets the glider flight overviews.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderflights/daterange/{fromDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{toDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<GliderFlightOverview>))]
        public IHttpActionResult GetGliderFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var flights = _flightService.GetGliderFlightOverviews(fromDate, toDate);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the glider flight overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("gliderflights/page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<GliderFlightOverview>))]
        public IHttpActionResult GetPagedGliderFlightOverview([FromBody]PageableSearchFilter<GliderFlightOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var flights = _flightService.GetPagedGliderFlightOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the motor flight overviews of all flights and flight types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("motorflights")]
        [Route("motorflights/overview")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetMotorFlightOverviews()
        {
            var flights = _flightService.GetMotorFlightOverviews();
            return Ok(flights);
        }

        /// <summary>
        /// Gets the motor flight overviews within today.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("motorflights/today")]
        [Route("motorflights/today/overview")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetMotorFlightOverviewsWithinToday()
        {
            var flights = _flightService.GetMotorFlightOverviewsWithinToday();
            return Ok(flights);
        }


        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing
        /// <summary>
        /// Gets the motor flight overviews.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("motorflights/daterange/{fromDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{toDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<FlightOverview>))]
        public IHttpActionResult GetMotorFlightOverviews(DateTime fromDate, DateTime toDate)
        {
            var flights = _flightService.GetMotorFlightOverviews(fromDate, toDate);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the motor flight overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("motorflights/page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<FlightOverview>))]
        public IHttpActionResult GetPagedMotorFlightOverview([FromBody]PageableSearchFilter<FlightOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var flights = _flightService.GetPagedFlightOverview(pageStart, pageSize, pageableSearchFilter, true);
            return Ok(flights);
        }

        /// <summary>
        /// Gets the flight details.
        /// </summary>
        /// <param name="flightId">The flight identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{flightId}")]
        [ResponseType(typeof(FlightDetails))]
        public IHttpActionResult GetFlightDetails(Guid flightId)
        {
            var flightDetails = _flightService.GetFlightDetails(flightId);
            return Ok(flightDetails);
        }

        /// <summary>
        /// Inserts the specified flight details.
        /// </summary>
        /// <param name="flightDetails">The flight details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [DontValidateModelState]
        [ResponseType(typeof(FlightDetails))]
        public IHttpActionResult Insert([FromBody] FlightDetails flightDetails)
        {
            _flightService.InsertFlightDetails(flightDetails);
            return Ok(flightDetails);
        }

        /// <summary>
        /// Updates the specified flight.
        /// </summary>
        /// <param name="flightId">The flight identifier.</param>
        /// <param name="flightDetails">The flight details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{flightId}")]
        [DontValidateModelState]
        [ResponseType(typeof(FlightDetails))]
        public IHttpActionResult Update(Guid flightId, [FromBody]FlightDetails flightDetails)
        {
            _flightService.UpdateFlightDetails(flightDetails);
            return Ok(flightDetails);
        }

        /// <summary>
        /// Deletes the specified flight.
        /// </summary>
        /// <param name="flightId">The flight identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{flightId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid flightId)
        {
            _flightService.DeleteFlight(flightId);
            return Ok();
        }

        /// <summary>
        /// Validates the flights which are not already validated.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("validate")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ValidateFlights()
        {
            _flightService.ValidateFlights();
            return Ok();
        }

        /// <summary>
        /// Lock the flights which are not already locked.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lock")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
        public IHttpActionResult LockFlights()
        {
            _flightService.LockFlights();
            return Ok();
        }

        /// <summary>
        /// Forces to lock the flights which are not already locked.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lock/force")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
        public IHttpActionResult ForceLockFlights()
        {
            _flightService.LockFlights(true);
            return Ok();
        }

        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing
        /// <summary>
        /// Gets the flights for exchange created or modified since.
        /// </summary>
        /// <param name="modifiedSince">Created or modified date time.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("exchange/modified/{modifiedSince:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("exchange/modified/{*modifiedSince:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<FlightExchangeData>))]
        public IHttpActionResult GetFlightsModifiedSince(DateTime modifiedSince)
        {
            var flights = _flightService.GetFlightsModifiedSince(modifiedSince);
            return Ok(flights);
        }
    }
}
