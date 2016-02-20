using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting
{
    public class FlightReportData
    {
        public DateTime StatisticStartDateTime { get; set; }

        public DateTime StatisticEndDateTime { get; set; }

        public int NumberOfWinchStarts { get; set; }

        public int NumberOfTowingStarts { get; set; }

        public int NumberOfSelfStarts { get; set; }

        public int NumberOfStartsOfUnknownType { get; set; }

        public int NumberOfAllStarts { get; set; }

        public TimeSpan TotalFlightDuration { get; set; }

        public List<FlightDetailsReportData> FlightDetailsReportDatas { get; set; }
        
    }
}
