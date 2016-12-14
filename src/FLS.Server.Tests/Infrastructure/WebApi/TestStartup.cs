using System;
using System.Web;
using System.Web.Http;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.Tests.Mocks.Services;
using FLS.Server.WebApi;
using FLS.Server.WebApi.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

namespace FLS.Server.Tests.Infrastructure.WebApi
{
    public class TestStartup : Startup
    {
        public override HttpConfiguration GetInjectionConfiguration()
        {
            Console.WriteLine("TestStartup.GetInjectionConfiguration()");
            var config = new HttpConfiguration();
            //http://codeclimber.net.nz/archive/2015/02/20/Using-Entity-Framework-within-an-Owin-hosted-Web-API-with.aspx
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            //var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            //we create an empty container and register the types self for the test environment
            var container = new UnityContainer(); //UnityConfig.GetEmptyContainer();
            Console.WriteLine($"Create new FLS.Server.Tests.TestStartup.UnityContainer: {container.GetHashCode()}");
            RegisterTypes(container);
            var resolver = new UnityDependencyResolver(container);
            config.DependencyResolver = resolver;
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            return config;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            Console.WriteLine("TestStartup.RegisterTypes(), Container-Hashcode: " + container.GetHashCode());
            //_identityService = new IdentityService();

            try
            {

           
                //container.RegisterType<AircraftHelper>();
                //container.RegisterType<PersonHelper>();
                //container.RegisterType<LocationHelper>();
                //container.RegisterType<AircraftReservationHelper>();
                //container.RegisterType<ClubHelper>();
                //container.RegisterType<UserHelper>();

                //container.RegisterType<ValidateModelStateAttribute>();
                //container.RegisterType<CheckModelForNullAttribute>();
                //container.RegisterType<UnhandledExceptionFilterAttribute>();

                container.RegisterType<IDataProtectionProvider, MachineKeyDataProtectionProvider>(new HierarchicalLifetimeManager());
                container.RegisterType<IIdentityMessageService, IdentityEmailService>();

                try
                {
                    container.RegisterType<IIdentityService, IdentityService>(new HierarchicalLifetimeManager());
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error while trying to register IIdentityService: {exception.Message}\n{exception.StackTrace}");
                    throw;
                }

                //container.RegisterInstance(typeof(IIdentityService), _identityService);
                container.RegisterType<IdentityUserManager>();
                container.RegisterType<IUserStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IUserPasswordStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IUserRoleStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IUserLockoutStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IUserEmailStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IUserSecurityStampStore<User, Guid>, IdentityUserStoreService>();
                container.RegisterType<IRoleStore<Role, Guid>, IdentityRoleStoreService>();
                container.RegisterType<IEmailSendService, MockEmailSendService>();
                container.RegisterType<ILocationService, LocationService>();
                container.RegisterType<IAircraftService, AircraftService>();
                container.RegisterType<IPersonService, PersonService>();
                container.RegisterType<IExtensionService, ExtensionService>();

                try
                {
                    container.RegisterType<UserManager<User, Guid>, IdentityUserManager>();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error while trying to register UserManager: {exception.Message}\n{exception.StackTrace}");
                    throw;
                }

                //container.RegisterType<DataAccessService>();
                //container.RegisterType<AircraftReservationService>();
                //container.RegisterType<AircraftService>();
                //container.RegisterType<ClubService>();
                //container.RegisterType<LocationService>();
                //container.RegisterType<FlightService>();
                //container.RegisterType<InvoiceService>();
                //container.RegisterType<LanguageService>();
                //container.RegisterType<PersonService>();
                //container.RegisterType<PlanningDayService>();
                //container.RegisterType<UserService>();
                //container.RegisterType<WorkflowService>();

                //container.RegisterType<HttpContextBase>(new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));
                //container.RegisterType<IOwinContext>(new InjectionFactory(c => c.Resolve<HttpContextBase>().GetOwinContext()));
                //container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => c.Resolve<IOwinContext>().Authentication));
                container.RegisterType<IAuthenticationManager>(
                    new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));


                //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                //    new InjectionConstructor(typeof(ApplicationDbContext)));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in TestStartup.RegisterTypes: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
