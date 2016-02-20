using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FLS.Server.Data.DbEntities;
using FLS.Server.WebApi.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;

namespace FLS.Server.WebApi.Providers
{
    public class FLSOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public FLSOAuthAuthorizationServerProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        /// <summary>
        /// Those methods (GrantResourceOwnerCredentials and ValidateClientAuthentication) should only be invoked 
        /// when getting the token, when you use the token it's the OAuthBearerAuthentication middleware 
        /// that is used --> http://stackoverflow.com/questions/25483508/owin-bearer-token-not-working-for-webapi
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            //var userManager = context.OwinContext.GetUserManager<IdentityUserManager>();
            var unityContainer = (UnityContainer)UnityConfig.GetConfiguredContainer();
            var userManager = unityContainer.Resolve<IdentityUserManager>();

            var user = userManager.FindAsync(context.UserName, context.Password).Result;

            //http://stackoverflow.com/questions/24969332/how-do-i-enable-the-accessfailedcount-and-lockout-functionality-on-asp-net-ident
            //var signInManager = unityContainer.Resolve<IdentitySignInManager>();
            //var status = signInManager.PasswordSignInAsync(context.UserName, context.Password, false, true);

            //if (status.Result == SignInStatus.Failure) return;

            if (user == null)
            {
                user = userManager.FindByNameAsync(context.UserName).Result;

                if (user != null)
                {
                    //password was wrong. Increase failed login
                    //http://tech.trailmax.info/2014/06/asp-net-identity-user-lockout/
                    user.DoNotUpdateMetaData = true;
                    await userManager.AccessFailedAsync(user.Id);
                }

                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            user.DoNotUpdateMetaData = true;
            var isEmailConfirmed = userManager.IsEmailConfirmedAsync(user.UserId).Result;

            if (isEmailConfirmed == false)
            {
                context.SetError("invalid_grant", "The email address is not confirmed until now.");
                return;
            }

            //http://tech.trailmax.info/2014/06/asp-net-identity-user-lockout/
            var isAccountLocked = userManager.IsLockedOutAsync(user.UserId).Result;

            if (isAccountLocked)
            {
                context.SetError("invalid_grant", "Account is currently locked, please contact your club administrator or the system administrator.");
                return;
            }

            //http://tech.trailmax.info/2014/06/asp-net-identity-user-lockout/
            await userManager.ResetAccessFailedCountAsync(user.Id);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(User user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                //{ "UserId", user.UserId.ToString() },
                { "userName", user.UserName }
            };
            return new AuthenticationProperties(data);
        }
    }
}