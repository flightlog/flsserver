using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.User
{
    public class UserRegistrationDetails
    {
        public UserRegistrationDetails()
        {
            UserRoleIds = new List<Guid>();
        }

        [Required]
        [StringLength(100)]
        public string FriendlyName { get; set; }

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string NotificationEmail { get; set; }

        public Guid? PersonId { get; set; }

        public string Remarks { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public List<Guid> UserRoleIds { get; set; }

        [Required]
        [StringLength(256)]
        public string EmailConfirmationLink { get; set; }
    }
}
