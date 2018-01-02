using System;

namespace FLS.Data.WebApi.AircraftReservation
{
    public class AircraftReservationTypeListItem 
    {
        public Guid AircraftReservationTypeId { get; set; }

        public string AircraftReservationTypeName { get; set; }

        public string Remarks { get; set; }

        public bool IsInstructorRequired { get; set; }

        public bool IsObserverPilotOrInstructorRequired { get; set; }

        public bool IsPassengerRequired { get; set; }
    }
}
