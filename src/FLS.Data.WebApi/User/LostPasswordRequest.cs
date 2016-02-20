using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.User
{
    public class LostPasswordRequest
    {
        [Required]
        [StringLength(255)]
        public string UsernameOrNotificationEmailAddress { get; set; }

        public bool SearchForUsernameOnly { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordResetLink { get; set; }
    }
}
