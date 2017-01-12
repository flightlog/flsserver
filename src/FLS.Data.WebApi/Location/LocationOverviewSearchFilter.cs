using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Location
{
    public class LocationOverviewSearchFilter
    {
        public LocationOverviewSearchFilter()
        {
            Sorting = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Sorting { get; set; }

        public string AirportFrequency { get; set; }

        public string CountryName { get; set; }

        public string Elevation { get; set; }

        public string IcaoCode { get; set; }

        public string LocationName { get; set; }

        public string LocationShortName { get; set; }

        public string LocationTypeName { get; set; }

        public bool? IsAirfield { get; set; }

        public string RunwayDirection { get; set; }

        public string RunwayLength { get; set; }

        public string Description { get; set; }

        public bool? IsInboundRouteRequired { get; set; }

        public bool? IsOutboundRouteRequired { get; set; }
    }
}
