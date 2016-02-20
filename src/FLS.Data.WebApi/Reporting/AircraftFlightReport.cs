using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting
{
    public class AircraftFlightReport
    {
        public DateTime ReportingDateTime { get; set; }

        public AircraftFlightReportFilterCriteria FilterCriteria { get; set; }

        public List<AircraftFlightReportData> AircraftFlightReportData { get; set; }

        public AircraftFlightReport()
        {
            ReportingDateTime = DateTime.UtcNow;
            AircraftFlightReportData = new List<AircraftFlightReportData>();
        }

        public AircraftFlightReport(AircraftFlightReport aircraftFlightReport)
        {
            AircraftFlightReportData = aircraftFlightReport.AircraftFlightReportData;
            FilterCriteria = aircraftFlightReport.FilterCriteria;
            ReportingDateTime = aircraftFlightReport.ReportingDateTime;
        }
    }
}
