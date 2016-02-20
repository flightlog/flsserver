using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Club
{
    public class FlightTypeDetails : FLSBaseData
    {

        public Guid FlightTypeId { get; set; }

        [StringLength(30)]
        public string FlightCode { get; set; }

        [Required]
        [StringLength(100)]
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

        public override Guid Id
        {
            get { return FlightTypeId; }
            set { FlightTypeId = value; }
        }
    }
}
