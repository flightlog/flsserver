using System;

namespace FLS.Data.WebApi.Location
{
    public class LocationTypeListItem
    {
        public Guid LocationTypeId { get; set; }

        public string LocationTypeName { get; set; }

        public bool IsAirfield { get; set; }

        public int? CupWaypointId { get; set; }
    }
}
