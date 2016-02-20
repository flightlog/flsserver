using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace FLS.Server.Service.Tests
{
    [TestClass]
    public class BaseServiceTest
    {
        
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
        }

        private UserStoreService UserStoreService { get; set; }
        
        protected User CurrentUser
        {
            get { return IdentityService.Instance.CurrentAuthenticatedFLSUser; }
        }

        public BaseServiceTest()
        {
            UserStoreService = new UserStoreService();
            SetCurrentUser("fgzo");
        }

        protected void SetCurrentUser(string userName)
        {
            var user = UserStoreService.FindByNameAsync(userName).Result;
            IdentityService.Instance.CurrentAuthenticatedFLSUser = user;
        }

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            DatabasePreparer.Instance.PrepareDatabaseForTests();
        }
    }
}
