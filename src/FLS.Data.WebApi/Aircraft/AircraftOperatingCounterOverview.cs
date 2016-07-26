using System;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftOperatingCounterOverview : FLSBaseData
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public Guid AircraftOperatingCounterId { get; set; }

        public string Immatriculation { get; set; }

        public DateTime AtDateTime { get; set; }

        public int? TotalStarts { get; set; }

        public long? FlightOperatingCounter { get; set; }

        public long? EngineOperatingCounter { get; set; }
        
        public int? FlightOperatingCounterUnitTypeId { get; set; }

        public int? EngineOperatingCounterUnitTypeId { get; set; }

        public override Guid Id
        {
            get { return AircraftOperatingCounterId; }
            set { AircraftOperatingCounterId = value; }
        }
    }
}
