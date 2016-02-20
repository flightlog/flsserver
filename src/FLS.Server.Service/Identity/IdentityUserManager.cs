using System;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FLS.Server.Service.Identity
{
    public class IdentityUserManager : UserManager<User, Guid>
    {
        public IdentityUserManager(IUserStore<User, Guid> store)
            : base(store)
        {
        }

        public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options, IOwinContext context)
        {
            //TODO: Review with Chrigi Moser
            //var manager = new IdentityUserManager(new UserStoreService(new UserService(new DataAccessService())));
            var identityService = new IdentityService();
            var manager = new IdentityUserManager(new UserStoreService(new UserService(new DataAccessService(identityService), identityService)));
            // Configure validation logic for usernames
            //manager.UserValidator = new UserValidator<User, Guid>(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = true
            //};

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            return manager;
        }

        public static string HashPassword(string plainPassword)
        {
            var passwordHasher = new PasswordHasher();
            return passwordHasher.HashPassword(plainPassword);
        }
    }
}
