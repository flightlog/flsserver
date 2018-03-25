using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.Testing;
using FLS.Data.WebApi.Articles;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;
using FLS.Server.Service.Accounting;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for deliveries.
    /// </summary>
    [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
    [RoutePrefix("api/v1/deliverycreationtests")]
    public class DeliveryCreationTestsController : ApiController
    {
        private DeliveryService DeliveryService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryCreationTestsController"/> class.
        /// </summary>
        public DeliveryCreationTestsController(DeliveryService deliveryService)
        {
            DeliveryService = deliveryService;
        }
        
        /// <summary>
        /// Gets the delivery creation tests overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<DeliveryCreationTestOverview>))]
        public IHttpActionResult GetPagedDeliveryCreationTestOverview([FromBody]PageableSearchFilter<DeliveryCreationTestOverviewSearchFilter> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var records = DeliveryService.GetPagedDeliveryCreationTestOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(records);
        }

        /// <summary>
        /// Gets the delivery creation test details.
        /// </summary>
        /// <param name="deliveryCreationTestId">The delivery creation test identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{deliveryCreationTestId}")]
        [ResponseType(typeof(DeliveryCreationTestDetails))]
        public IHttpActionResult GetDeliveryCreationTestDetails(Guid deliveryCreationTestId)
        {
            var articleDetails = DeliveryService.GetDeliveryCreationTestDetails(deliveryCreationTestId);
            return Ok(articleDetails);
        }

        /// <summary>
        /// Creates a test delivery for the specified flight.
        /// </summary>
        /// <param name="flightId">FlightId of flight to create a delivery.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("testdeliveryforflight/{flightId}")]
        [ResponseType(typeof(DeliveryCreationResult))]
        public IHttpActionResult CreateDeliveryDetailsForTest(Guid flightId)
        {
            var result = DeliveryService.CreateDeliveryDetailsForTest(flightId);
            return Ok(result);
        }

        /// <summary>
        /// Runs all test delivery rules.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("run")]
        [ResponseType(typeof(List<DeliveryCreationTestDetails>))]
        public IHttpActionResult RunDeliveryCreationTest()
        {
            var result = DeliveryService.RunDeliveryCreationTests();
            return Ok(result);
        }

        /// <summary>
        /// Runs a test delivery rule for the specified delivery test rule.
        /// </summary>
        /// <param name="deliveryCreationTestId">Id of DeliveryCreationTest.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("run/{deliveryCreationTestId}")]
        [ResponseType(typeof(DeliveryCreationTestDetails))]
        public IHttpActionResult RunDeliveryCreationTest(Guid deliveryCreationTestId)
        {
            var result = DeliveryService.RunDeliveryCreationTest(deliveryCreationTestId);
            return Ok(result);
        }

        /// <summary>
        /// Inserts the specified delivery creation test details.
        /// </summary>
        /// <param name="deliveryCreationTestDetails">The delivery creation test details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(DeliveryCreationTestDetails))]
        public IHttpActionResult Insert([FromBody] DeliveryCreationTestDetails deliveryCreationTestDetails)
        {
            DeliveryService.InsertDeliveryCreationTestDetails(deliveryCreationTestDetails);
            return Ok(deliveryCreationTestDetails);
        }

        /// <summary>
        /// Updates the specified delivery creation test identifier.
        /// </summary>
        /// <param name="deliveryCreationTestId">The delivery creation test identifier.</param>
        /// <param name="deliveryCreationTestDetails">The delivery creation test details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deliveryCreationTestId}")]
        [ResponseType(typeof(DeliveryCreationTestDetails))]
        public IHttpActionResult Update(Guid deliveryCreationTestId, [FromBody]DeliveryCreationTestDetails deliveryCreationTestDetails)
        {
            DeliveryService.UpdateDeliveryCreationTestDetails(deliveryCreationTestDetails);
            return Ok(deliveryCreationTestDetails);
        }

        /// <summary>
        /// Deletes the specified delivery creation test identifier.
        /// </summary>
        /// <param name="deliveryCreationTestId">The delivery creation test identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{deliveryCreationTestId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid deliveryCreationTestId)
        {
            DeliveryService.DeleteDeliveryCreationTest(deliveryCreationTestId);
            return Ok();
        }

    }
}
