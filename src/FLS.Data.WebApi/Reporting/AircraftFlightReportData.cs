using System;

namespace FLS.Data.WebApi.Reporting
{
    public class AircraftFlightReportData
    {
        public Guid AircraftId { get; set; }

        public string AircraftImmatriculation { get; set; }
        
        public int NumberOfWinchStarts { get; set; }

        public int NumberOfTowingStarts { get; set; }

        public int NumberOfSelfStarts { get; set; }

        public int NumberOfMotorflightStarts { get; set; }

        public int NumberOfStartsOfUnknownType { get; set; }

        public int NumberOfAllStarts { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public string FlightDurationString
        {
            get
            {
                return string.Format("{0}:{1:mm}", (int) FlightDuration.TotalHours, FlightDuration);
            }
        }

        public TimeSpan? EngineDuration { get; set; }

        public string EngineDurationString
        {
            get
            {
                if (EngineDuration.HasValue)
                {
                    return string.Format("{0}:{1:mm}", (int)EngineDuration.Value.TotalHours, EngineDuration.Value);
                }
                else
                {
                    return "n/a";
                }
            }
        }

    }
}
