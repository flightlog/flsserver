using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryOverview : FLSBaseData
    {
        public DeliveryOverview()
        {
            FlightInformation = new FlightInformation();
        }

        public Guid DeliveryId { get; set; }

        public FlightInformation FlightInformation { get; set; }

        public string Recipient { get; set; }

        [StringLength(250)]
        public string DeliveryInformation { get; set; }

        public int NumberOfDeliveryItems { get; set; }

        public override Guid Id
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }
    }
}
