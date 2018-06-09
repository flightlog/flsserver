using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;
using FLS.Server.Service.Accounting;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for deliveries.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/deliveries")]
    public class DeliveriesController : ApiController
    {
        private DeliveryService DeliveryService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveriesController"/> class.
        /// </summary>
        public DeliveriesController(DeliveryService deliveryService)
        {
            DeliveryService = deliveryService;
        }

        /// <summary>
        /// Gets the delivery details.
        /// </summary>
        /// <param name="deliveryId">The delivery identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{deliveryId}")]
        [ResponseType(typeof(DeliveryDetails))]
        public IHttpActionResult GetDeliveryDetails(Guid deliveryId)
        {
            var details = DeliveryService.GetDeliveryDetails(deliveryId);
            return Ok(details);
        }

        /// <summary>
        /// Gets the delivery overviews.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<DeliveryOverview>))]
        public IHttpActionResult GetPagedDeliveryOverview([FromBody]PageableSearchFilter<DeliveryOverviewSearchFilter> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var deliveries = DeliveryService.GetPagedDeliveryOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(deliveries);
        }

        /// <summary>
        /// Gets a list of delivery details which have not been further processed.
        /// A processed delivery must set as further processed by calling the web api method 
        /// <code>api/v1/deliveries/delivered</code> with the relevant <code>DeliveryBooking</code>.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpGet]
        [Route("notprocessed")]
        [ResponseType(typeof(List<DeliveryDetails>))]
        public IHttpActionResult GetNotProcessedDeliveries()
        {
            var deliveries = DeliveryService.GetDeliveryDetailsList(furtherProcessed:false);
            return Ok(deliveries);
        }

        /// <summary>
        /// Sets the delivery as further processed.
        /// </summary>
        /// <param name="deliveryBooking">The delivery booking.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("delivered")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SetDeliveryAsDelivered([FromBody] DeliveryBooking deliveryBooking)
        {
            var result = DeliveryService.SetDeliveryAsDelivered(deliveryBooking);
            return Ok(result);
        }

        /// <summary>
        /// Runs a manually triggered delivery creation process to create deliveries from the flights which are in locked state 
        /// and therefore ready for delivery creation.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpGet]
        [Route("create")]
        [ResponseType(typeof(void))]
        public IHttpActionResult CreateDeliveries()
        {
            DeliveryService.CreateDeliveriesFromFlights();
            return Ok();
        }
    }
}
