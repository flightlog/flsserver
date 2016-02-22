using FLS.Server.WebApi.Identity;
using FLS.Server.WebApi.Models;
using FLS.Server.WebApi.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace FLS.Server.WebApi.Controllers
{
    [RoutePrefix("api/v1/auth")]
    public class AuthenticationController : ApiController
    {
        private IAuthenticationManager authContext { get { return Request.GetOwinContext().Authentication; } }
        private readonly IdentityUserManager userManager;

        public AuthenticationController(IdentityUserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Validate external login. Basically like "/Token" action, but checks "external login" authentication method (OverrideAuthentication / HostAuthentication attibutes).
        /// Normal web-api controllers are configured to allow only a local oauth bearer token.
        /// </summary>
        /// <param name="provider">External login provider "eg. Google"</param>
        /// <returns>403 unauthorized or bearer token.</returns>
        [AllowAnonymous]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [HttpGet, Route("external/token")]
        public async Task<IHttpActionResult> ExternalToken()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            // TODO: auflösen zum app user
            var username = "testclubuser";
            var user = userManager.FindByName(username);
            
            // create oauth taken manually.
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());

            DateTime currentUtc = DateTime.UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(Startup.AccessTokenDaysValid));

            string accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            return Ok(new { access_token = accessToken, userName = username });
        }

        /// <summary>
        /// Sign in external login. Double check Identity again and redirect to challange / response handler
        /// of the external authentication provider if necessary.
        /// External authentication gets called in a popup window to avoid refreshing the whole page. Therefore the external auth flow
        /// has to end somewhere. In this case in a static html page, which just closes the popup window.
        /// </summary>
        /// <param name="provider">External login provider "eg. Google"</param>
        /// <returns></returns>
        [AllowAnonymous]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [HttpGet, Route("external/signin/{provider}")]
        public IHttpActionResult ExternalSignIn(string provider)
        {
            if (string.IsNullOrEmpty(provider))
                throw new ArgumentException("provider not set.", "provider");

            // check error querystring.
            if (Request.RequestUri.Query.Contains("?error="))
            {
                string error = Request.RequestUri.Query.Substring("!error=".Length);
                if(!string.IsNullOrWhiteSpace(error))
                    throw new InvalidOperationException(error);
            }

            // unauthorized - send challange (redirects to login provider)
            if (!User.Identity.IsAuthenticated)
            {
                authContext.Challenge(provider);
                return Unauthorized();
            }

            return Redirect(Url.Content("/Content/auth_success.html"));
        }
    }
}
