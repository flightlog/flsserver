using System;
using FLS.Data.WebApi.Flight;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLS.Data.WebApi.AircraftReservation
{
    public class AircraftReservationOverview : FLSBaseData
    {
        public Guid AircraftReservationId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsAllDayReservation { get; set; }
        public string Immatriculation { get; set; }

        public string PilotName { get; set; }
        public string LocationName { get; set; }

        public string SecondCrewName { get; set; }
        public string ReservationTypeName { get; set; }
        public string Remarks { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FlightCategory FlightCategory { get; set; }

        public override Guid Id
        {
            get { return AircraftReservationId; }
            set { AircraftReservationId = value; }
        }
    }
}
