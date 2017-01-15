using System;

namespace FLS.Data.WebApi.Flight
{
    public class GliderFlightOverviewSearchFilter
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

        public string GliderFlightDuration { get; set; }


        public string TowFlightAirState { get; set; }

        public string TowFlightValidationState { get; set; }

        public string TowFlightProcessState { get; set; }

        public string TowAircraftImmatriculation { get; set; }

        public string TowPilotName { get; set; }

        public string TowFlightStartDateTime { get; set; }

        public string TowFlightLdgDateTime { get; set; }

        public string TowFlightStartLocation { get; set; }

        public string TowFlightLdgLocation { get; set; }

        public string TowFlightDuration { get; set; }

        public string WinchOperatorName { get; set; }
    }
}
