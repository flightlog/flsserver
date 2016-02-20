using System;

namespace FLS.Data.WebApi.Flight
{
    public class GliderFlightDetailsData : FlightDetailsData
    {
        public string CouponNumber { get; set; }

        public Nullable<Guid> WinchOperatorPersonId { get; set; }

        public int? StartPosition { get; set; }

        public Nullable<Guid> PassengerPersonId { get; set; }
    }
}
