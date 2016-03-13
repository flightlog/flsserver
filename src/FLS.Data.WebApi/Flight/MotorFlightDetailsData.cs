using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Flight
{
    public class MotorFlightDetailsData : FlightDetailsData
    {
        public Nullable<DateTime> BlockStartDateTime { get; set; }

        public Nullable<DateTime> BlockEndDateTime { get; set; }

        public List<Guid> PassengerPersonIds { get; set; }

        public int? NrOfPassengers { get; set; }

    }
}
