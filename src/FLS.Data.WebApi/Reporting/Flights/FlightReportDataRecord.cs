using System;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Flight;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class FlightReportDataRecord
    {
        private int? _flightDurationInSeconds;
        private DateTime? _ldgDateTime;
        private DateTime? _startDateTime;

        public Guid FlightId { get; set; }

        public DateTime? FlightDate { get; set; }

        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public int AirState { get; set; }

        public int ProcessState { get; set; }

        public string FlightCode { get; set; }

        public string FlightTypeName { get; set; }

        public Nullable<DateTime> LdgDateTime
        {
            get { return _ldgDateTime; }
            set { _ldgDateTime = value.SetAsUtc(); }
        }

        public bool IsSoloFlight { get; set; }

        public Nullable<DateTime> StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value.SetAsUtc(); }
        }

        //TODO: using enum?
        public int? StartType { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public TimeSpan? FlightDuration { get; set; }

        public TowFlightReportDataRecord TowFlight { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FlightCategory FlightCategory { get; set; }

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
