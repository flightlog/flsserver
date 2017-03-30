using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Flight
{
    public class GliderFlightOverviewSearchFilter
    {
        public DateTimeFilter FlightDate { get; set; }

        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public List<int> AirStates { get; set; }

        public List<int> ProcessStates { get; set; }

        public string FlightCode { get; set; }
        
        public string LdgTime { get; set; }

        public bool? IsSoloFlight { get; set; }
        
        public string StartTime { get; set; }
        
        public string StartType { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public string GliderFlightDuration { get; set; }
        
        public List<int> TowFlightAirStates { get; set; }

        public List<int> TowFlightProcessStates { get; set; }

        public string TowAircraftImmatriculation { get; set; }

        public string TowPilotName { get; set; }

        public string TowFlightStartTime { get; set; }

        public string TowFlightLdgTime { get; set; }

        public string TowFlightStartLocation { get; set; }

        public string TowFlightLdgLocation { get; set; }

        public string TowFlightDuration { get; set; }

        public string WinchOperatorName { get; set; }
    }
}
