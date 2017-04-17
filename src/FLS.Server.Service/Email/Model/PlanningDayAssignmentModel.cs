namespace FLS.Server.Service.Email.Model
{
    public class PlanningDayAssignmentModel
    {
        public string FLSUrl { get; set; }
        public string SenderName { get; set; }
        public string Date { get; set; }
        public string LocationName { get; set; }
        public string AssignedPersonName { get; set; }
        public string AssignmentTypeName { get; set; }
        public string Remarks { get; set; }
        public int NrOfAircraftReservations { get; set; }
    }
}
