using System;
using FLS.Common.Extensions;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Flight
{
    public class GliderFlightOverview : FLSBaseData
    {
        private int? _towFlightDurationInSeconds;
        private int? _gliderFlightDurationInSeconds;
        private DateTime? _ldgDateTime;
        private DateTime? _startDateTime;
        private DateTime? _towFlightStartDateTime;
        private DateTime? _towFlightLdgDateTime;

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

        public TimeSpan? GliderFlightDuration { get; set; }

        public Nullable<Guid> TowFlightId { get; set; }

        public Nullable<int> TowFlightAirState { get; set; }

        public Nullable<int> TowFlightValidationState { get; set; }

        public Nullable<int> TowFlightProcessState { get; set; }

        public string TowAircraftImmatriculation { get; set; }

        public string TowPilotName { get; set; }

        public Nullable<DateTime> TowFlightStartDateTime
        {
            get { return _towFlightStartDateTime; }
            set { _towFlightStartDateTime = value.SetAsUtc(); }
        }

        public Nullable<DateTime> TowFlightLdgDateTime
        {
            get { return _towFlightLdgDateTime; }
            set { _towFlightLdgDateTime = value.SetAsUtc(); }
        }

        public string TowFlightStartLocation { get; set; }

        public string TowFlightLdgLocation { get; set; }

        public TimeSpan? TowFlightDuration { get; set; }

        
        public override Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }

        public string WinchOperatorName { get; set; }

        #region Helper Properties with JsonIgnore
        [JsonIgnore]
        public int? GliderFlightDurationInSeconds
        {
            get { return _gliderFlightDurationInSeconds; }
            set
            {
                _gliderFlightDurationInSeconds = value;
                if (value != null)
                {
                    GliderFlightDuration = TimeSpan.FromSeconds(value.Value);
                }
                else if (StartDateTime.HasValue && LdgDateTime.HasValue == false)
                {
                    GliderFlightDuration = DateTime.UtcNow - StartDateTime.Value;
                }
            }
        }

        [JsonIgnore]
        public int? TowFlightDurationInSeconds
        {
            get { return _towFlightDurationInSeconds; }
            set
            {
                _towFlightDurationInSeconds = value;

                if (value != null)
                {
                    TowFlightDuration = TimeSpan.FromSeconds(value.Value);
                }
                else if (TowFlightStartDateTime.HasValue && TowFlightLdgDateTime.HasValue == false)
                {
                    TowFlightDuration = DateTime.UtcNow - TowFlightStartDateTime.Value;
                }
            }
        }
        #endregion Helper Properties with JsonIgnore
    }
}
