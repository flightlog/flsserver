using System;

namespace FLS.Data.WebApi.Flight
{
    public class FlightDetails : FLSBaseData
    {
        public virtual Guid FlightId { get; set; }

        public Nullable<int> StartType { get; set; }

        public Nullable<DateTime> FlightDate { get; set; }

        public GliderFlightDetailsData GliderFlightDetailsData { get; set; }

        public TowFlightDetailsData TowFlightDetailsData { get; set; }

        public MotorFlightDetailsData MotorFlightDetailsData { get; set; }

        public override Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }
    }
}
