using System;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Mocks.Services;
using FLS.Server.WebApi;
using FLS.Server.WebApi.ActionFilters;
using FLS.Server.WebApi.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public abstract class BaseServiceTest : BaseTestConfig
    {
        private IUserStore<User, Guid> _userStoreService;
        private IIdentityService _identityService;
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
        }

        protected IUnityContainer UnityContainer { get; set; }

        public void UnityInitialize()
        {
            UnityContainer = UnityConfig.GetEmptyContainer();
            RegisterTypes(UnityContainer);
            
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            container.RegisterType<ValidateModelStateAttribute>();
            container.RegisterType<CheckModelForNullAttribute>();
            container.RegisterType<UnhandledExceptionFilterAttribute>();
            container.RegisterType<IDataProtectionProvider, MachineKeyDataProtectionProvider>(new HierarchicalLifetimeManager());
            container.RegisterType<IIdentityMessageService, IdentityEmailService>();

            container.RegisterType<IIdentityService, IdentityService>(new HierarchicalLifetimeManager());
            //container.RegisterType<IdentityUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<User, Guid>, IdentityUserManager>();
            container.RegisterType<IUserStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserPasswordStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserRoleStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IRoleStore<Role, Guid>, IdentityRoleStoreService>();
            container.RegisterType<IEmailSendService, MockEmailSendService>();
            container.RegisterType<DataAccessService>();
            container.RegisterType<AircraftReservationService>();
            container.RegisterType<AircraftService>();
            container.RegisterType<ClubService>();
            container.RegisterType<LocationService>();
            container.RegisterType<FlightService>();
            container.RegisterType<InvoiceService>();
            container.RegisterType<LanguageService>();
            container.RegisterType<PersonService>();
            container.RegisterType<PlanningDayService>();
            container.RegisterType<UserService>();
            container.RegisterType<WorkflowService>();
            
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
            //    new InjectionConstructor(typeof(ApplicationDbContext)));
        }
        [TestInitialize]
        public void Setup()
        {
            UnityInitialize();
            _userStoreService = UnityContainer.Resolve<IUserStore<User, Guid>>();
            _identityService = UnityContainer.Resolve<IIdentityService>();
            SetCurrentUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
        }

        protected void SetCurrentUser(string userName)
        {
            var user = _userStoreService.FindByNameAsync(userName).Result;
            _identityService.SetUser(user);
        }

        protected User CurrentIdentityUser
        {
            get { return _identityService.CurrentAuthenticatedFLSUser; }
        }
    }
}
