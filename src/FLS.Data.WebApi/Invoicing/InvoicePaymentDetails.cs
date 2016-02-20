using System;

namespace FLS.Data.WebApi.Invoicing
{
    public class InvoicePaymentDetails
    {
        public string InvoiceNumber { get; set; }

        public DateTime InvoicePaymentDate { get; set; }
    }
}
