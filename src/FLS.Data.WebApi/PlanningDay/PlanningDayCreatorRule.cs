using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.PlanningDay
{
    public class PlanningDayCreatorRule
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool EveryMonday { get; set; }

        public bool EveryTuesday { get; set; }

        public bool EveryWednesday { get; set; }

        public bool EveryThursday { get; set; }

        public bool EveryFriday { get; set; }

        public bool EverySaturday { get; set; }

        public bool EverySunday { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid LocationId { get; set; }

        public string Remarks { get; set; }
    }
}
