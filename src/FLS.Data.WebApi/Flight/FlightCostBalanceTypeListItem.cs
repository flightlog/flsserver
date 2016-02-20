using System;

namespace FLS.Data.WebApi.Flight
{
    public class FlightCostBalanceTypeListItem
    {
        public int FlightCostBalanceTypeId { get; set; }

        public string FlightCostBalanceTypeName { get; set; }

        public string Comment { get; set; }

        public bool PersonForInvoiceRequired { get; set; }
    }
}
