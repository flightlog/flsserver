using System;
using System.Linq;
using System.Web;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.WebApi.ActionFilters;
using FLS.Server.WebApi.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;

namespace FLS.Server.WebApi
{
    /// <summary>
    /// http://michael-mckenna.com/blog/dependency-injection-for-asp-net-web-api-action-filters-in-3-easy-steps
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        private static bool _registerTypes;

        private static Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            Console.WriteLine($"Create new FLS.Server.WebApi.UnityConfig.UnityContainer: {container.GetHashCode()}");

            if (_registerTypes)
            {
                RegisterTypes(container);
            }

            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            _registerTypes = true;
            return _container.Value;
        }

        public static IUnityContainer GetEmptyContainer()
        {
            Console.WriteLine("UnityConfig.GetEmptyContainer() " + _container.Value);
            _registerTypes = false;
            
            return _container.Value;
        }
        
        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            //container.RegisterType<ValidateModelStateAttribute>();
            //container.RegisterType<CheckModelForNullAttribute>();
            //container.RegisterType<UnhandledExceptionFilterAttribute>();
            container.RegisterType<IIdentityMessageService, IdentityEmailService>();
            container.RegisterType<IIdentityService, IdentityService>(new HierarchicalLifetimeManager());
            container.RegisterType<IdentityUserManager>();
            container.RegisterType<UserManager<User, Guid>, IdentityUserManager>();
            //container.RegisterType<IdentitySignInManager<User, Guid>, IdentitySignInManager>();
            container.RegisterType<IUserStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserPasswordStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserRoleStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserLockoutStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserEmailStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserSecurityStampStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IRoleStore<Role, Guid>, IdentityRoleStoreService>();
            container.RegisterType<IEmailSendService, EmailSendService>();
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
    }
}
