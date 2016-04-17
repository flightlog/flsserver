using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.User
{
    public class EmailTokenResendRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string EmailConfirmationLink { get; set; }
    }
}
