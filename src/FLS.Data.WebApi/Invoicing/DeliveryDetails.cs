using System;

namespace FLS.Data.WebApi.Invoicing
{
    public class DeliveryDetails
    {
        public string DeliveryNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public Nullable<DateTime> InvoicePaymentDate { get; set; }
    }
}
