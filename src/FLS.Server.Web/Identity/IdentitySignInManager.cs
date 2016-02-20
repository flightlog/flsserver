using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FLS.Server.WebApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentitySignInManager : SignInManager<User, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserManager"/> class.
        /// </summary>
        public IdentitySignInManager(UserManager<User, Guid> manager, IAuthenticationManager authenticationManager)
            : base(manager, authenticationManager)
        {
        }

        //internal static IdentitySignInManager Create(IdentityFactoryOptions<IdentitySignInManager> options, IOwinContext context)
        //{
        //    var unityContainer = (UnityContainer)UnityConfig.GetConfiguredContainer();
        //    var userManager = unityContainer.Resolve<IdentityUserManager>();

        //    var signInManager = new IdentitySignInManager(userManager, context.Authentication);
            
        //    return signInManager;
        //}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync(UserManager, AuthenticationType);
        }
    }
}
