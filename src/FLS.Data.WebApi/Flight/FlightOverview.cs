using System;
using FLS.Common.Extensions;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Flight
{
    public class FlightOverview : FLSBaseData
    {
        private int? _flightDurationInSeconds;
        private DateTime? _startDateTime;
        private DateTime? _ldgDateTime;

        public Guid FlightId { get; set; }

        public Nullable<DateTime> FlightDate { get; set; }

        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public int AirState { get; set; }

        public int ProcessState { get; set; }

        public string FlightCode { get; set; }

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

        public Nullable<int> StartType { get; set; }

        public string StartLocation { get; set; }

        public string LdgLocation { get; set; }

        public TimeSpan? FlightDuration { get; set; }

        public override Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }

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
                else if (StartDateTime.HasValue && LdgDateTime.HasValue == false)
                {
                    FlightDuration = DateTime.UtcNow - StartDateTime.Value;
                }
            }
        }
        #endregion Helper Properties with JsonIgnore
    }
}
