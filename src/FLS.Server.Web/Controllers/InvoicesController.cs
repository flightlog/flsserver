﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for invoices.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/invoices")]
    public class InvoicesController : ApiController
    {
        private InvoiceService InvoiceService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoicesController"/> class.
        /// </summary>
        public InvoicesController(InvoiceService invoiceService)
        {
            InvoiceService = invoiceService;
        }

        /// <summary>
        /// Gets all the flight invoice details.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<FlightInvoiceDetails>))]
        public IHttpActionResult GetFlightInvoiceDetails()
        {
            var invoices = InvoiceService.GetFlightInvoiceDetails(DateTime.MinValue, DateTime.MaxValue);
            return Ok(invoices);
        }

        /// <summary>
        /// Gets the flight invoice details.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpGet]
        [Route("daterange/{fromDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{toDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [ResponseType(typeof(List<FlightInvoiceDetails>))]
        public IHttpActionResult GetFlightInvoiceDetails(DateTime fromDate, DateTime toDate)
        {
            var invoices = InvoiceService.GetFlightInvoiceDetails(fromDate, toDate);
            return Ok(invoices);
        }

        /// <summary>
        /// Sets the flight as invoiced or stored with a delivery number in the invoice system.
        /// </summary>
        /// <param name="flightInvoiceBooking">The flight invoice booking.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("invoiced")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SetFlightAsInvoiced([FromBody] FlightInvoiceBooking flightInvoiceBooking)
        {
            var result = InvoiceService.SetFlightAsInvoiced(flightInvoiceBooking);
            return Ok(result);
        }

        /// <summary>
        /// Sets the invoice number for deliverables.
        /// </summary>
        /// <param name="deliveryNumber">The delivery number.</param>
        /// <param name="deliveryDetails">The delivery details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("deliveries/{deliveryNumber}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SetInvoiceNumberForDeliverables(string deliveryNumber, [FromBody] DeliveryDetails deliveryDetails)
        {
            var result = InvoiceService.SetInvoiceNumberForDeliverables(deliveryDetails.DeliveryNumber,
                                                                        deliveryDetails.InvoiceNumber,
                                                                        deliveryDetails.InvoicePaymentDate);
            return Ok(result);
        }

        /// <summary>
        /// Sets the invoiced as paid.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <param name="invoicePaymentDetails">The invoice payment details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator)]
        [HttpPost]
        [Route("{invoiceNumber}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SetInvoiceAsPaid(string invoiceNumber, [FromBody] InvoicePaymentDetails invoicePaymentDetails)
        {
            if (invoiceNumber != invoicePaymentDetails.InvoiceNumber)
            {
                return BadRequest("Invoice number is not equal in body");
            }

            var result = InvoiceService.SetInvoiceAsPaid(invoiceNumber, invoicePaymentDetails.InvoicePaymentDate);
            return Ok(result);
        }
    }
}
