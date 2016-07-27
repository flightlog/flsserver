using System;

namespace FLS.Data.WebApi.Flight
{
    [Serializable]
    public class FlightFilterSettings
    {
        public Nullable<Guid> AircraftId { get; set; }

        public bool IsFlightAirStateNew { get; set; }

        public bool IsFlightAirStateStarted { get; set; }
        
        public bool IsFlightAirStateLanded { get; set; }

        public bool IsFlightProcessStateInvoiced { get; set; }

        public Nullable<Guid> FlightCrewPersonId { get; set; }

        public bool IsFlightCrewPersonIdAPilot { get; set; }

        public bool IsFlightCrewPersonIdAnInstructor { get; set; }

        public bool IsFlightCrewPersonIdAnyFlightCrewType { get; set; }

        public Nullable<DateTime> BeginDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }

        public Nullable<Guid> StartLocationId { get; set; }

        public Nullable<Guid> StartTypeId { get; set; }

        public Nullable<Guid> FlightTypeId { get; set; }
    }
}
