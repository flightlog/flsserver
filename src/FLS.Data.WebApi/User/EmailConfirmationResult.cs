using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.User
{
    public class EmailConfirmationResult
    {
        public Guid UserId { get; set; }

        public string PasswordResetCode { get; set; }
    }
}
