namespace FLS.Data.WebApi.AircraftReservation
{
    public class AircraftReservationTypeListItem 
    {
        public int AircraftReservationTypeId { get; set; }

        public string AircraftReservationTypeName { get; set; }

        public string Remarks { get; set; }

        public bool IsInstructorRequired { get; set; }
    }
}
