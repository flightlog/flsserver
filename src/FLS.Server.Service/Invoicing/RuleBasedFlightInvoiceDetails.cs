using System;
using FLS.Data.WebApi.Invoicing;
using Newtonsoft.Json;

namespace FLS.Server.Service.Invoicing
{
    internal class RuleBasedFlightInvoiceDetails : FlightInvoiceDetails
    {

        //logical fields for simplifying rules
        [JsonIgnore]
        public bool IsInvoicedToClubInternal { get; set; }

        [JsonIgnore]
        public double ActiveFlightTime { get; set; }

        [JsonIgnore]
        public bool NoLandingTaxForTowFlight { get; set; }


        [JsonIgnore]
        public bool NoLandingTaxForGliderFlight { get; set; }

        [JsonIgnore]
        public bool NoLandingTaxForFlight { get; set; }

        [JsonIgnore]
        public Guid ClubId { get; set; }
    }
}
