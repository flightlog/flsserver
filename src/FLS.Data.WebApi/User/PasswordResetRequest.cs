using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.User
{
    public class PasswordResetRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string PasswordResetCode { get; set; }

        [Required]
        [StringLength(255)]
        public string NewPassword { get; set; }
    }
}
