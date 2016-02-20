namespace FLS.Server.Service.Email.Model
{
    public class PlanningDayInfoModel
    {
        public string FLSUrl { get; set; }
        public string SenderName { get; set; }
        public string Date { get; set; }
        public string LocationName { get; set; }
        public string FlightOperatorName { get; set; }
        public string FlightOperatorContact { get; set; }
        public string TowPilotName { get; set; }
        public string TowPilotContact { get; set; }
        public string InstructorName { get; set; }
        public string Remarks { get; set; }
        public ReservationInfoRow[] AircraftReservations { get; set; }
    }
}
