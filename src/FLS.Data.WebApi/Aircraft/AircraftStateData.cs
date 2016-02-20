using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftStateData
    {
        [Required]
        public Guid AircraftId { get; set; }

        [Required]
        public int AircraftState { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public Nullable<Guid> NoticedByPersonId { get; set; }

        public string Remarks { get; set; }
    }
}
