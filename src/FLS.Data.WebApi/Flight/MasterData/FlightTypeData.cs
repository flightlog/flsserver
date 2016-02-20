using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    public class FlightTypeData
    {
        public Guid FlightTypeId { get; set; }

        public string FlightCode { get; set; }

        public string FlightTypeName { get; set; }

        public bool InstructorRequired { get; set; }

        public bool ObserverPilotOrInstructorRequired { get; set; }

        public bool IsCheckFlight { get; set; }

        public bool IsPassengerFlight { get; set; }

        public bool IsSystemFlight { get; set; }

        public bool IsFlightCostBalanceSelectable { get; set; }

        public bool IsSoloFlight { get; set; }
    }
}
