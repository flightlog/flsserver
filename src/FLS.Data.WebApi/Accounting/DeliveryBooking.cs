using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryBooking
    {
        [Required]
        [GuidNotEmptyValidator]
        public Guid DeliveryId { get; set; }
        
        [Required]
        public DateTime DeliveryDateTime { get; set; }

        [Required]
        public string DeliveryNumber { get; set; }
    }
}
