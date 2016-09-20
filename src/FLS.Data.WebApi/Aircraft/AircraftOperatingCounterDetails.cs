using System;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftOperatingCounterDetails : FLSBaseData
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public Guid AircraftOperatingCounterId { get; set; }

        public Guid AircraftId { get; set; }

        public DateTime AtDateTime { get; set; }

        public int? TotalTowedGliderStarts { get; set; }

        public int? TotalWinchLaunchStarts { get; set; }

        public int? TotalSelfStarts { get; set; }

        public long? FlightOperatingCounterInSeconds { get; set; }

        public long? EngineOperatingCounterInSeconds { get; set; }

        public long? NextMaintenanceAtFlightOperatingCounterInSeconds { get; set; }

        public long? NextMaintenanceAtEngineOperatingCounterInSeconds { get; set; }

        public override Guid Id
        {
            get { return AircraftOperatingCounterId; }
            set { AircraftOperatingCounterId = value; }
        }
    }
}
