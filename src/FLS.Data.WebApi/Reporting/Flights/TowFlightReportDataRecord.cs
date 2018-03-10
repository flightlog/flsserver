using System;
using FLS.Common.Extensions;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class TowFlightReportDataRecord
    {
        private int? _flightDurationInSeconds;
        private DateTime? _ldgDateTime;
        private DateTime? _startDateTime;

        public Guid TowFlightId { get; set; }

        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public int AirState { get; set; }

        public int ProcessState { get; set; }

        public string FlightCode { get; set; }

        public string FlightTypeName { get; set; }

        public DateTime? LdgDateTime
        {
            get { return _ldgDateTime; }
            set { _ldgDateTime = value.SetAsUtc(); }
        }

        public DateTime? StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value.SetAsUtc(); }
        }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public TimeSpan? FlightDuration { get; set; }

        #region Helper Properties with JsonIgnore
        [JsonIgnore]
        public int? FlightDurationInSeconds
        {
            get { return _flightDurationInSeconds; }
            set
            {
                _flightDurationInSeconds = value;
                if (value != null)
                {
                    FlightDuration = TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        #endregion Helper Properties with JsonIgnore
    }
}
