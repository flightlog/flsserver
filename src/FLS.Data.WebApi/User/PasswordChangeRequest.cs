using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.User
{
    public class PasswordChangeRequest
    {
        public Guid UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(255)]
        public string NewPassword { get; set; }
    }
}
