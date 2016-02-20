using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    public class LocationTypeData
    {
        public Guid LocationTypeId { get; set; }

        public string LocationTypeName { get; set; }

        public bool IsAirfield { get; set; }
    }
}
