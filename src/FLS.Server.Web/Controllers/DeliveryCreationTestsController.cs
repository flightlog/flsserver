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
    [Authorize]
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
        /// Gets all the delivery creation tests for an overview.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<DeliveryCreationTestOverview>))]
        public IHttpActionResult GetDeliveryCreationTestOverview()
        {
            var records = DeliveryService.GetDeliveryCreationTestOverviews();
            return Ok(records);
        }

        /// <summary>
        /// Gets the delivery creation tests overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<DeliveryCreationTestOverview>))]
        public IHttpActionResult GetPagedDeliveryCreationTestOverview([FromBody]PageableSearchFilter<DeliveryCreationTestOverviewSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
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
