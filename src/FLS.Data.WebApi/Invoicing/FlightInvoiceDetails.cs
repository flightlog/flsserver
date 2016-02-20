using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Invoicing
{
    public class FlightInvoiceDetails
    {

        public Guid FlightId { get; set; }

        public DateTime FlightDate { get; set; }

        public string AircraftImmatriculation { get; set; }

        public string InvoiceRecipientPersonClubMemberKey { get; set; }

        public string InvoiceRecipientPersonClubMemberNumber { get; set; }

        public string InvoiceRecipientPersonDisplayName { get; set; }

        public string FlightInvoiceInfo { get; set; }

        public string AdditionalInfo { get; set; }

        /// <summary>
        /// If <code>IncludesTowFlightId</code> has value (Guid), the invoice details includes the line items for the tow flight and hold their flightId for reference.
        /// </summary>
        /// <value>
        /// The flightId of the referenced TowFlight.
        /// </value>
        public Nullable<Guid> IncludesTowFlightId { get; set; }

        public List<FlightInvoiceLineItem> FlightInvoiceLineItems { get; set; }
    }
}
