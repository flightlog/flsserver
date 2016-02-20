using System;

namespace FLS.Data.WebApi.Flight
{
    public class GliderFlightOverview : FLSBaseData
    {
        
        public Guid FlightId { get; set; }
        
        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public int FlightState { get; set; }

        public string FlightCode { get; set; }
        
        public Nullable<DateTime> LdgDateTime { get; set; }

        public bool IsSoloFlight { get; set; }
        
        public Nullable<DateTime> StartDateTime { get; set; }
        
        public Nullable<int> StartType { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }
        
        public Nullable<Guid> TowFlightId { get; set; }

        public Nullable<int> TowFlightState { get; set; }

        public string TowAircraftImmatriculation { get; set; }

        public string TowPilotName { get; set; }

        public Nullable<DateTime> TowFlightStartDateTime { get; set; }

        public Nullable<DateTime> TowFlightLdgDateTime { get; set; }

        public string TowFlightStartLocation { get; set; }

        public string TowFlightLdgLocation { get; set; }

        public override Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }

        public string WinchOperatorName { get; set; }
    }
}
