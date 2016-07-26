using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftEngineOperatingCounterResult
    {
        [Required]
        public Guid AircraftId { get; set; }

        public DateTime? AtDateTime { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown (units see EngineOperatingCounterUnitTypeId)
        /// </summary>
        public long? EngineOperatingCounter { get; set; }

        public int? EngineOperatingCounterUnitTypeId { get; set; }

        public bool AircraftHasNoEngine { get; set; }
    }
}
