using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting.Flights
{
    public class AdvancedFlightReportFilterCriteria
    {
        public DateTimeFilter FlightDate { get; set; }

        public bool TakeAllAircraftsExceptListed { get; set; } = true;

        /// <summary>
        /// Gets or sets the aircraft ids which should be included into the aircraft filter. Consider the <seealso cref="TakeAllAircraftsExceptListed"/> setting too.
        /// </summary>
        /// <value>
        /// The aircraft ids.
        /// </value>
        public List<Guid> AircraftIds { get; set; }

        public bool TakeWinchStarts { get; set; } = true;

        public bool TakeTowingStarts { get; set; } = true;

        public bool TakeSelfStarts { get; set; } = true;

        public bool TakeMotorFlightStarts { get; set; } = true;

        public bool TakeExternalStarts { get; set; } = true;

        public bool TakeAllStartLocationsExceptListed { get; set; } = true;

        public List<Guid> StartLocationIds { get; set; }

        public bool TakeAllLdgLocationsExceptListed { get; set; } = true;

        public List<Guid> LdgLocationIds { get; set; }

        public StartLdgLocationCondition StartLdgLocationCondition { get; set; } = StartLdgLocationCondition.Or;

        public bool? IsSoloFlight { get; set; }

        public bool TakeAllFlightTypesExceptListed { get; set; } = true;

        public List<Guid> FlightTypeIds { get; set; }

        public List<AdvancedFlightCrewFilter> FlightCrewFilters { get; set; } = new List<AdvancedFlightCrewFilter>() { new AdvancedFlightCrewFilter() };

        public FlightCrewFilterCondition FlightCrewFilterCondition { get; set; } = FlightCrewFilterCondition.Or;
    }
}
