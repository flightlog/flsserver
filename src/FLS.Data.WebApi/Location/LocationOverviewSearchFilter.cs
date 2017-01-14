using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FLS.Data.WebApi.Location
{
    public class LocationOverviewSearchFilter
    {
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
                sb.Append($"{info.Name}: {info.GetValue(this)}, ");

            foreach (PropertyInfo info in type.GetProperties())
                sb.Append($"{info.Name}: {info.GetValue(this, null)}, ");
            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
