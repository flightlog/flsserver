using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftOperatingCounterResult
    {
        [Required]
        public Guid AircraftId { get; set; }

        public DateTime? AtDateTime { get; set; }

        /// <summary>
        /// Gets the engine operating counter in seconds
        /// </summary>
        public long? EngineOperatingCounterInSeconds { get; set; }

        public string EngineOperatingCounterUnitTypeKeyName { get; set; }

        public bool AircraftHasNoEngine { get; set; }
    }
}
