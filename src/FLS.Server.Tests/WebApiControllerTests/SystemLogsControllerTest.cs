using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.System;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class SystemLogsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSystemLogsOverviewWebApiTest()
        {
            LoginAsSystemAdmin();
            var response = GetAsync<IEnumerable<SystemLogOverview>>(Uri).Result;
            Assert.IsTrue(response.Any());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/systemlogs"; }
        }
    }
}
