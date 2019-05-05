using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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



        /// <summary>
        /// Controls if a flight is not needed to invoice.
        /// This flag is only internal for rule handling,
        /// but can not be in derived class RuleBasedDeliveryDetails
        /// </summary>
        [JsonIgnore]
        internal bool DoNotInvoiceFlight { get; set; }
    }
}
