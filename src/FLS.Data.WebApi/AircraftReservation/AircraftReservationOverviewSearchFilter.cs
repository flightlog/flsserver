using System;

namespace FLS.Data.WebApi.AircraftReservation
{
    public class AircraftReservationOverviewSearchFilter
    {
        public DateTimeFilter Start { get; set; }

        public DateTimeFilter End { get; set; }

        public bool? IsAllDayReservation { get; set; }
        public string Immatriculation { get; set; }

        public string PilotName { get; set; }
        public string LocationName { get; set; }

        public string InstructorName { get; set; }
        public string ReservationTypeName { get; set; }
        public string Remarks { get; set; }
    }
}
