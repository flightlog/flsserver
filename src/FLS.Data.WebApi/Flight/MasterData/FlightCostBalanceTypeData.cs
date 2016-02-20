using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    [Serializable]
    public class FlightCostBalanceTypeData
    {
        
        public int FlightCostBalanceTypeId { get; set; }

        public string FlightCostBalanceTypeName { get; set; }

        public string Comment { get; set; }

        public bool PersonForInvoiceRequired { get; set; }
    }
}
