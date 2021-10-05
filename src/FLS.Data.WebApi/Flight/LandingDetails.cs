using System;

namespace FLS.Data.WebApi.Flight
{
    public class LandingDetails
    {
        public string OgnDeviceId { get; set; }

        public string Immatriculation { get; set; }

        public string LandingLocationIcaoCode { get; set; }

        public DateTime LandingTimeUtc { get; set; }
    }
}
