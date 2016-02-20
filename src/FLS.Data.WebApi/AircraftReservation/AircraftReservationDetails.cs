using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.AircraftReservation
{
    public class AircraftReservationDetails : FLSBaseData
    {
        public Guid AircraftReservationId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public bool IsAllDayReservation { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid AircraftId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid PilotPersonId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid LocationId { get; set; }
        
        public Guid? InstructorPersonId { get; set; }

        [Required]
        public int ReservationTypeId { get; set; }
        public string Remarks { get; set; }

        public override Guid Id
        {
            get { return AircraftReservationId; }
            set { AircraftReservationId = value; }
        }
    }
}
