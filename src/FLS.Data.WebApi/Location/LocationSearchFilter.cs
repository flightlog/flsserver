using System;

namespace FLS.Data.WebApi.Location
{
    public class LocationSearchFilter
    {
        public string SearchText { get; set; }

        public bool SearchInAirportFrequency { get; set; }

        public bool SearchInCountryName { get; set; }

        public bool SearchInElevation { get; set; }

        public bool SearchInIcaoCode { get; set; }

        public bool SearchInLocationName { get; set; }

        public bool SearchInLocationShortName { get; set; }

        public bool SearchInLocationTypeName { get; set; }

        public bool? IsAirfield { get; set; }
        
        public bool SearchInRunwayDirection { get; set; }

        public bool SearchInRunwayLength { get; set; }

        public bool SearchInDescription { get; set; }

        public bool? IsInboundRouteRequired { get; set; }

        public bool? IsOutboundRouteRequired { get; set; }
    }
}
