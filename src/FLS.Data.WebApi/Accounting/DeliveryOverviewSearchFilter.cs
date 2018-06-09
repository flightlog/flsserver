using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryOverviewSearchFilter
    {
        public DateTimeFilter FlightStartDateTime { get; set; }

        public string FlightAircraftImmatriculation { get; set; }

        public string RecipientName { get; set; }

        public string DeliveryInformation { get; set; }

        public string DeliveryNumber { get; set; }

        /// <summary>
        /// Delivery date and in case of a flight, the flight date.
        /// </summary>
        public DateTimeFilter DeliveredOn { get; set; }

        public bool? IsFurtherProcessed { get; set; }

        public long? BatchId { get; set; }
    }
}
