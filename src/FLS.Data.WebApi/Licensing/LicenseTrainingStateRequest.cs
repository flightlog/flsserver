using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Licensing
{
    public class LicenseTrainingStateRequest
    {
        [Required]
        public Guid PilotPersonId { get; set; }

        [Required]
        public int StartTypeId { get; set; }

        public Guid? AircraftId { get; set; }

        public bool? IsPassengerFlight { get; set; }
    }
}
