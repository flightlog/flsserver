using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.User
{
    public class UserDetails : FLSBaseData
    {
        public UserDetails()
        {
            UserRoleIds = new List<Guid>();
        }

        public Guid UserId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid ClubId { get; set; }

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

        public int AccountState { get; set; }
        
        public Nullable<DateTime> LastPasswordChangeOn { get; set; }

        public bool ForcePasswordChangeNextLogon { get; set; }

        public override Guid Id
        {
            get { return UserId; }
            set { UserId = value; }
        }
    }
}
