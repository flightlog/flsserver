using System;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Flight
{
    public class FlightOverview : FLSBaseData
    {
        private int? _flightDurationInSeconds;

        public Guid FlightId { get; set; }

        public Nullable<DateTime> FlightDate { get; set; }

        public string Immatriculation { get; set; }

        public string PilotName { get; set; }

        public string SecondCrewName { get; set; }

        public string FlightComment { get; set; }

        public int AirState { get; set; }

        public int ValidationState { get; set; }

        public int ProcessState { get; set; }

        public string FlightCode { get; set; }
        
        public Nullable<DateTime> LdgDateTime { get; set; }

        public bool IsSoloFlight { get; set; }
        
        public Nullable<DateTime> StartDateTime { get; set; }
        
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
