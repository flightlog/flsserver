using System;
using System.Web.Http;
using FLS.Server.WebApi;
using FLS.Server.WebApi.Identity;
using FLS.Server.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(Startup))]
namespace FLS.Server.WebApi
{
    public class Startup
    {
        public static string PublicClientId { get; private set; }
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        static Startup()
        {
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new FLSOAuthAuthorizationServerProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        /// <summary>
        /// Configures the HttpConfiguration and the Unity Container
        /// <remarks>This method is overriden in Test project</remarks>
        /// </summary>
        /// <returns></returns>
        public virtual HttpConfiguration GetInjectionConfiguration()
        {
            var config = new HttpConfiguration();
            //http://codeclimber.net.nz/archive/2015/02/20/Using-Entity-Framework-within-an-Owin-hosted-Web-API-with.aspx
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            //var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            config.DependencyResolver = resolver;

            return config;
        }

        /// <summary>
        /// Startup method which is called when OWIN server starts.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = GetInjectionConfiguration();

            WebApiConfig.Register(config);

            ConfigureOAuth(app, config);

            //UseWebApi must be after configuration of authentification stuff: see: http://stackoverflow.com/questions/25483508/owin-bearer-token-not-working-for-webapi
            app.UseWebApi(config);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private void ConfigureOAuth(IAppBuilder app, HttpConfiguration config)
        {
            //http://stackoverflow.com/questions/26725866/cors-on-owin-and-accessing-token-causes-access-control-allow-origin-error
            //app.UseCors(CorsOptions.AllowAll);

            //http://stackoverflow.com/questions/26980271/web-api-2-preflight-cors-request-for-bearer-token
            //app.UseCors(new CorsOptions
            //{
            //    PolicyProvider = new CorsPolicyProvider
            //    {
            //        PolicyResolver = context => Task.FromResult(new CorsPolicy
            //        {
            //            AllowAnyHeader = true,
            //            AllowAnyMethod = true,
            //            AllowAnyOrigin = true,
            //            SupportsCredentials = false,
            //            PreflightMaxAge = Int32.MaxValue // << ---- THIS
            //        })
            //    }
            //});
            var dataProtectionProvider = app.GetDataProtectionProvider();

            //In unit tests the dataProtectionProvider is null, so we have to create DpapiDataProtectionProvider
            if (dataProtectionProvider == null)
            {
                dataProtectionProvider = new MachineKeyDataProtectionProvider();
            }

            UnityConfig.GetConfiguredContainer().RegisterInstance(dataProtectionProvider);

            //http://blog.iteedee.com/2014/03/asp-net-identity-2-0-cookie-token-authentication/
            //http://www.codeproject.com/Articles/823263/ASP-NET-Identity-Introduction-to-Working-with-Iden
            //app.CreatePerOwinContext<IdentityUserManager>(IdentityUserManager.Create);
            //app.CreatePerOwinContext<IdentityRoleManager>(IdentityRoleManager.Create);
            app.CreatePerOwinContext<IdentityUserManager>(() => (IdentityUserManager)config.DependencyResolver.GetService(typeof(IdentityUserManager)));
            app.CreatePerOwinContext<IdentityRoleManager>(() => (IdentityRoleManager)config.DependencyResolver.GetService(typeof(IdentityRoleManager)));
            //app.CreatePerOwinContext<IdentitySignInManager>(IdentitySignInManager.Create);

            //app.CreatePerOwinContext<UserManager<User, Guid>>(
            //    (options, owinContext) => UnityConfig.GetConfiguredContainer().Resolve<UserManager<User, Guid>>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            app.UseGoogleAuthentication(clientId: "721862177625-okeu8trhtrno0pml026tf1ti1geho35u.apps.googleusercontent.com", clientSecret: "3GOKs5WFHboQNGkfNBaawpZt");
        }
    }
}
