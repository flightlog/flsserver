using System;

namespace FLS.Data.WebApi.Club
{
    public class FlightTypeOverview : FLSBaseData
    {
        
        public Guid FlightTypeId { get; set; }

        public string FlightCode { get; set; }

        public string FlightTypeName { get; set; }

        public bool InstructorRequired { get; set; }

        public bool ObserverPilotOrInstructorRequired { get; set; }

        public bool IsCheckFlight { get; set; }

        public bool IsPassengerFlight { get; set; }

        public bool IsForGliderFlights { get; set; }

        public bool IsForTowFlights { get; set; }

        public bool IsForMotorFlights { get; set; }

        public bool IsFlightCostBalanceSelectable { get; set; }

        public bool IsSoloFlight { get; set; }

        public bool IsCouponNumberRequired { get; set; }

        public int MinNrOfAircraftSeatsRequired { get; set; }

        public override Guid Id
        {
            get { return FlightTypeId; }
            set { FlightTypeId = value; }
        }
    }
}
