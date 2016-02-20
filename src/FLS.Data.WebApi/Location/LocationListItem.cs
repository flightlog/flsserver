using System;

namespace FLS.Data.WebApi.Location
{
    public class LocationListItem 
    {

        public Guid LocationId { get; set; }

        public string CountryCode { get; set; }

        public string IcaoCode { get; set; }

        public string LocationName { get; set; }

        public bool IsAirfield { get; set; }

        public bool IsInboundRouteRequired { get; set; }

        public bool IsOutboundRouteRequired { get; set; }
    }
}
