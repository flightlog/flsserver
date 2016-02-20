namespace FLS.Server.Service.Email.Model
{
    public class FlightInfoRow
    {
        public string FlightDate { get; set; }
        public string AircraftImmatriculation { get; set; }
        public string PilotName { get; set; }
        public string StartType { get; set; }
        public string FlightTypeName { get; set; }
        public string IsSoloFlight { get; set; }
        public string StartLocation { get; set; }
        public string LdgLocation { get; set; }
        public string StartTimeLocal { get; set; }
        public string LdgTimeLocal { get; set; }
        public string FlightDuration { get; set; }
        public string SecondCrewName { get; set; }
        public string FlightComment { get; set; }
        public string TowAircraftImmatriculation { get; set; }
        public string TowPilotName { get; set; }
        public string TowStartTimeLocal { get; set; }
        public string TowLdgTimeLocal { get; set; }
        public string TowFlightDuration { get; set; }
    }
}