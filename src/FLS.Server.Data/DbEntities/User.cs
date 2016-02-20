using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi;
using Microsoft.AspNet.Identity;

namespace FLS.Server.Data.DbEntities
{
    public partial class User : IUser<Guid>, IFLSMetaData
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }
        
        public Guid UserId { get; set; }

        public Guid ClubId { get; set; }

        [Required]
        [StringLength(256)]
        [Column("Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string FriendlyName { get; set; }

        public string PasswordHash { get; set; }

        public Guid? PersonId { get; set; }

        [Required]
        [StringLength(256)]
        public string NotificationEmail { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        public string SecurityStamp { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }
        
        [StringLength(250)]
        public string Remarks { get; set; }

        public int AccountState { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastPasswordChangeOn { get; set; }

        public bool ForcePasswordChangeNextLogon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do not update meta data].
        /// Used for workflow processes to not create a modified user error when trying to save records.
        /// </summary>
        /// <value>
        /// <c>true</c> if [do not update meta data]; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool DoNotUpdateMetaData { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Club Club { get; set; }

        public virtual Person Person { get; set; }

        public virtual UserAccountState UserAccountState { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Guid Id
        {
            get { return UserId; }
            set { UserId = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, Guid> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
            {
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));
            }

            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo info in type.GetProperties())
            {
                Type t = info.PropertyType;
                if (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                    t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)
                    || (t.Namespace != null && t.Namespace.Contains("FLS.Server.Data.DbEntities")))
                {
                    continue;
                }

                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            }

            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
