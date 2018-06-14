using Newtonsoft.Json;
using System;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class FlightReportSummary
    {
        private int? _flightDurationInSeconds;

        /// <summary>
        /// The totals needs to be summarized (grouped) by flight crew function, like PIC, Flight Instructor,
        /// or by Location.
        /// </summary>
        public string GroupBy { get; set; }

        public int TotalStarts { get; set; }

        public int TotalLdgs { get; set; }

        public TimeSpan TotalFlightDuration { get; set; }

        [JsonIgnore]
        public int? TotalFlightDurationInSeconds
        {
            get { return _flightDurationInSeconds; }
            set
            {
                _flightDurationInSeconds = value;
                if (value != null)
                {
                    TotalFlightDuration = TimeSpan.FromSeconds(value.Value);
                }
            }
        }
    }
}
