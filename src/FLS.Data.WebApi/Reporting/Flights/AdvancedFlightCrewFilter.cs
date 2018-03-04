using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class AdvancedFlightCrewFilter
    {
        public bool TakeAllFlightCrewPersonsExceptListed { get; set; } = true;

        public List<Guid> FlightCrewPersonIds { get; set; }

        public bool TakeAllFlightCrewTypesExceptListed { get; set; } = true;
        
        public List<Guid> FlightCrewTypeIds { get; set; }
    }
}
