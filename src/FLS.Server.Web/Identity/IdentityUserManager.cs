using System;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace FLS.Server.WebApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityUserManager : UserManager<User, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserManager"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public IdentityUserManager(IUserStore<User, Guid> store,
            IDataProtectionProvider dataProtectionProvider, 
            IIdentityMessageService identityMessageService)
            : base(store)
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<User, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            //TODO: Catch data from Database
            this.UserLockoutEnabledByDefault = false;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            if (identityMessageService != null)
            {
                EmailService = identityMessageService;
            }

            //var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

                this.UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtector)
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(24)
                };
            }
        }
    }
}
