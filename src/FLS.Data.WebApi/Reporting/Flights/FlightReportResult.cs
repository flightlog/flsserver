using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class FlightReportResult
    {
        public FlightReportFilterCriteria FlightReportFilterCriteria { get; set; }

        public PagedList<FlightReportDataRecord> Flights { get; set; }
        
        public List<FlightReportSummary> FlightReportSummaries { get; set; }
    }
}
