using FLS.Data.WebApi.System;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class SystemDatasControllerTest : BaseAuthenticatedTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSystemDataDetailsWebApiTest()
        {
            LoginAsSystemAdmin();
            var response = GetAsync<SystemDataDetails>(Uri).Result;
            Assert.IsNotNull(response);
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateSystemDataDetailsWebApiTest()
        {
            LoginAsSystemAdmin();
            var systemDataDetails = GetAsync<SystemDataDetails>(Uri).Result;
            Assert.IsNotNull(systemDataDetails);

            systemDataDetails.SystemSenderEmailAddress = "UpdateSystemDataDetailsWebApiTest@test.ch";

            var putResult = PutAsync(systemDataDetails,  Uri  + "/" + systemDataDetails.SystemDataId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/SystemDatas"; }
        }
        
    }
}
