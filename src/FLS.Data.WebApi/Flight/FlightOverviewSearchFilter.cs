using System;

namespace FLS.Data.WebApi.Flight
{
    public class FlightOverviewSearchFilter
    {
        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public string AirState { get; set; }

        public string ValidationState { get; set; }

        public string ProcessState { get; set; }

        public string FlightCode { get; set; }
        
        public string LdgDateTime { get; set; }

        public bool? IsSoloFlight { get; set; }
        
        public string StartDateTime { get; set; }
        
        public string StartType { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public string FlightDuration { get; set; }
    }
}
