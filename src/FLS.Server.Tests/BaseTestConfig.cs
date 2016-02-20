using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Infrastructure;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests
{
    /// <summary>
    /// <see cref="http://www.aaron-powell.com/posts/2014-01-12-integration-testing-katana-with-auth.html"/>
    /// </summary>
    [TestClass]
    public abstract class BaseTestConfig
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            DatabasePreparer.Instance.PrepareDatabaseForTests();

            var setup = new AdditionalTestDataController();
            //setup.SetupFullTestClub();
        }
    }
}
