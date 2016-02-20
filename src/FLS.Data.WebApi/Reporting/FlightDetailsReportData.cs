using System;

namespace FLS.Data.WebApi.Reporting
{
    public class FlightDetailsReportData
    {
        public Guid FlightId { get; set; }

        public string AircraftImmatriculation { get; set; }

        public string PilotPersonName { get; set; }

        public string AdditionalFlightCrewMembers { get; set; }

        public Nullable<DateTime> EngineTime { get; set; }

        public string FlightType { get; set; }

        public Nullable<DateTime> LdgDateTime { get; set; }

        public Nullable<DateTime> StartDateTime { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public Nullable<int> NrOfLdgs { get; set; }

        public bool IsSoloFlight { get; set; }
    }
}
