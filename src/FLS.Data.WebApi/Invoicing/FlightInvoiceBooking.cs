using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Invoicing
{
    public class FlightInvoiceBooking
    {
        [Required]
        [GuidNotEmptyValidator]
        public Guid FlightId { get; set; }

        //[Required]
        //[GuidNotEmptyValidator]
        //public Guid PersonId { get; set; }

        /// <summary>
        /// If <code>IncludesTowFlightId</code> has value (Guid), the invoice details includes the line items for the tow flight and hold their flightId for reference.
        /// </summary>
        /// <value>
        /// The flightId of the referenced TowFlight.
        /// </value>
        public Nullable<Guid> IncludesTowFlightId { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }
    }
}
