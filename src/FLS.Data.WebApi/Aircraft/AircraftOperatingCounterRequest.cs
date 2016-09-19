using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftOperatingCounterRequest
    {
        [Required]
        public Guid AircraftId { get; set; }

        public DateTime? AtDateTime { get; set; }
    }
}
