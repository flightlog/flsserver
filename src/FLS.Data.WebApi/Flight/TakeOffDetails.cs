using System;

namespace FLS.Data.WebApi.Flight
{
    public class TakeOffDetails
    {
        public string OgnDeviceId { get; set; }

        public AprsAircraftType AircraftType { get; set; }

        public string Immatriculation { get; set; }

        public string TakeOffLocationIcaoCode { get; set; }

        public DateTime TakeOffTimeUtc { get; set; }
    }
}