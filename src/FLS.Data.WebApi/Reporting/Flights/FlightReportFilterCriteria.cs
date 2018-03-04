using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class FlightReportFilterCriteria
    {
        public DateTimeFilter FlightDate { get; set; }

        public Guid? FlightCrewPersonId { get; set; }

        public Guid? LocationId { get; set; }

        public bool GliderFlights { get; set; } = true;

        public bool MotorFlights { get; set; } = true;
    }
}
