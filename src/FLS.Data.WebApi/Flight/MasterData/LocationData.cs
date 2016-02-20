using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    public class LocationData
    {
        public Guid LocationId { get; set; }

        public string CountryCode { get; set; }

        public string IcaoCode { get; set; }

        public string LocationName { get; set; }

        public bool IsAirfield { get; set; }
    }
}
