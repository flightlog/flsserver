using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryDetails : FLSBaseData
    {
        public DeliveryDetails()
        {
            RecipientDetails = new RecipientDetails();
            FlightInformation = new FlightInformation();
            DeliveryItems = new List<DeliveryItemDetails>();
        }

        public Guid DeliveryId { get; set; }

        public FlightInformation FlightInformation { get; set; }

        public RecipientDetails RecipientDetails { get; set; }

        [StringLength(250)]
        public string DeliveryInformation { get; set; }

        [StringLength(250)]
        public string AdditionalInformation { get; set; }

        public List<DeliveryItemDetails> DeliveryItems { get; set; }

        public string DeliveryNumber { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public bool IsFurtherProcessed { get; set; }

        public override Guid Id
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }
    }
}
