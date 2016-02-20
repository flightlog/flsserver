using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Flight
{
    public class FlightCrewDetails
    {
        public Nullable<Guid> FlightCrewId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid PersonId { get; set; }

        [Required]
        [Range(1, 10)]
        public int CrewType { get; set; }

    }
}
