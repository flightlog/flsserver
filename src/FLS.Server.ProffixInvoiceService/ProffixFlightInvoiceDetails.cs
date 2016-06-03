using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing;
using Newtonsoft.Json;

namespace FLS.Server.ProffixInvoiceService
{
    internal class ProffixFlightInvoiceDetails : FlightInvoiceDetails
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
        public Guid ClubId { get; set; }
    }
}
