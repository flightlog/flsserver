using System;

namespace FLS.Data.WebApi.Location
{
    public class LocationOverview : FLSBaseData
    {
        public Guid LocationId { get; set; }

        public string AirportFrequency { get; set; }

        public string CountryName { get; set; }

        public string Elevation { get; set; }

        public string IcaoCode { get; set; }

        public string LocationName { get; set; }

        public string LocationShortName { get; set; }

        public string LocationTypeName { get; set; }

        public bool IsAirfield { get; set; }
        
        public string RunwayDirection { get; set; }

        public string RunwayLength { get; set; }

        public string Description { get; set; }

        public bool IsInboundRouteRequired { get; set; }

        public bool IsOutboundRouteRequired { get; set; }

        public override Guid Id
        {
            get { return LocationId; }
            set { LocationId = value; }
        }
    }
}
