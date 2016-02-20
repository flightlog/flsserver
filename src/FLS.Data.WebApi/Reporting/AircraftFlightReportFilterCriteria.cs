using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Reporting
{
    public class AircraftFlightReportFilterCriteria
    {
        public DateTime StatisticStartDateTime { get; set; }

        public DateTime StatisticEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the aircraft ids. If the list is empty, all aircrafts will be taken for the report.
        /// </summary>
        /// <value>
        /// The aircraft ids.
        /// </value>
        public List<Guid> AircraftIds { get; set; }

        public bool TakeWinchStarts { get; set; }

        public bool TakeTowingStarts { get; set; }

        public bool TakeSelfStarts { get; set; }

        public bool TakeMotorFlightStarts { get; set; }

        public bool TakeExternalStarts { get; set; }

        /// <summary>
        /// Gets or sets the flown by person ids. If the list is empty, all flights and their flightcrew will be taken for the report
        /// </summary>
        /// <value>
        /// The flown by person ids.
        /// </value>
        public List<Guid> FlownByPersonIds { get; set; }

        public AircraftFlightReportFilterCriteria(bool takeWinchStarts = true, bool takeTowingStarts = true, bool takeSelfStarts = true, bool takeStartsOfUnknownType = true)
        {
            TakeWinchStarts = takeWinchStarts;
            TakeTowingStarts = takeTowingStarts;
            TakeSelfStarts = takeSelfStarts;
            TakeExternalStarts = takeStartsOfUnknownType;
            AircraftIds = new List<Guid>();
            FlownByPersonIds = new List<Guid>();
        }

        public AircraftFlightReportFilterCriteria(AircraftFlightReportFilterCriteria filterCriteria)
        {
            StatisticStartDateTime = filterCriteria.StatisticStartDateTime;
            StatisticEndDateTime = filterCriteria.StatisticEndDateTime;
            AircraftIds = filterCriteria.AircraftIds;
            TakeWinchStarts = filterCriteria.TakeWinchStarts;
            TakeTowingStarts = filterCriteria.TakeTowingStarts;
            TakeSelfStarts = filterCriteria.TakeSelfStarts;
            TakeExternalStarts = filterCriteria.TakeExternalStarts;
            FlownByPersonIds = filterCriteria.FlownByPersonIds;
        }
    }
}
