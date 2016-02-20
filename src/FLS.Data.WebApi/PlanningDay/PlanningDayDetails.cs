using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.PlanningDay
{
    public class PlanningDayDetails : FLSBaseData
    {
        public Guid PlanningDayId { get; set; }

        [Required]
        public DateTime Day { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid LocationId { get; set; }

        public string Remarks { get; set; }

        public Guid? TowingPilotPersonId { get; set; }
        
        public Guid? FlightOperatorPersonId { get; set; }

        public Guid? InstructorPersonId { get; set; }

        public override Guid Id
        {
            get { return PlanningDayId; }
            set { PlanningDayId = value; }
        }
    }
}
