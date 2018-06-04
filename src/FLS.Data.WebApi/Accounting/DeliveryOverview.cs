using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryOverview : FLSBaseData
    {
        public DeliveryOverview()
        {
            FlightInformation = new DeliveryOverviewFlightInformation();
        }

        public Guid DeliveryId { get; set; }

        public DeliveryOverviewFlightInformation FlightInformation { get; set; }

        public string RecipientName { get; set; }

        [StringLength(250)]
        public string DeliveryInformation { get; set; }
        
        public string DeliveryNumber { get; set; }

        /// <summary>
        /// Delivery date and in case of a flight, the flight date.
        /// </summary>
        public DateTime? DeliveredOn { get; set; }

        public bool IsFurtherProcessed { get; set; }

        public long BatchId { get; set; }

        public override Guid Id
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }
    }
}
