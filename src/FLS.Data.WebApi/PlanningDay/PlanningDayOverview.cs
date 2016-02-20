using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.PlanningDay
{
    public class PlanningDayOverview : FLSBaseData
    {
        public Guid PlanningDayId { get; set; }

        public DateTime Day { get; set; }

        public string LocationName { get; set; }

        public string Remarks { get; set; }

        //public bool AreStillAssignmentsRequired { get; set; }

        public string TowingPilotName { get; set; }

        public string FlightOperatorName { get; set; }

        public string InstructorName { get; set; }

        public int NumberOfAircraftReservations { get; set; }

        public override Guid Id
        {
            get { return PlanningDayId; }
            set { PlanningDayId = value; }
        }
    }
}
