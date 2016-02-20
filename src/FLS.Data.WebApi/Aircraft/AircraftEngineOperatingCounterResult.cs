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
        /// Gets the engine operating counter after engine shutdown in minutes and decimal in seconds as divide of 60
        /// </summary>
        public Decimal? EngineOperatingCounterInMinutes { get; set; }

        public bool AircraftHasNoEngine { get; set; }
    }
}
