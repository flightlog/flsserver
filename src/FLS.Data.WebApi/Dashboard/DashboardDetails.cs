using System;
using FLS.Data.WebApi.AircraftReservation;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Dashboard
{
    public class DashboardDetails
    {
        public SafetyDashboardDetails SafetyDashboardDetails { get; set; }

        public PersonDashboardDetails PersonDashboardDetails { get; set; }

        public FlightStatisticDashboardDetails GliderPilotFlightStatisticDashboardDetails { get; set; }

        //public FlightStatisticDashboardDetails GliderInstructorFlightStatisticDashboardDetails { get; set; }

        public FlightStatisticDashboardDetails MotorPilotFlightStatisticDashboardDetails { get; set; }

        public GliderLicenceStateDetails GliderLicenceStateDetails { get; set; }

        public List<AircraftReservationOverview> MyNextAircraftReservations { get; set; }
    }
}
