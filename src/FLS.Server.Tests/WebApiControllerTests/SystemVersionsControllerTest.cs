using FLS.Data.WebApi.System;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class SystemVersionsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSystemVersionInfoOverviewWebApiTest()
        {
            Logout();
            var response = GetAsync<SystemVersionInfoOverview>(Uri).Result;
            Assert.IsNotNull(response);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSystemVersionInfoDetailsWebApiTest()
        {
            Logout();
            var response = GetAsync<SystemVersionInfoDetails>(Uri + "/details").Result;
            Assert.IsNotNull(response);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/systemversions"; }
        }
    }
}
