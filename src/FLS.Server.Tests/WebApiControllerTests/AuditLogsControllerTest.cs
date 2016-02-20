using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Audit;
using FLS.Data.WebApi.Dashboard;
using FLS.Data.WebApi.Location;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AuditLogsControllerTest : BaseAuthenticatedTests
    {
        private LocationHelper _locationHelper;
        private DataAccessService _dataAccessService;

        [TestInitialize]
        public void TestInitialize()
        {
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
            _dataAccessService = UnityContainer.Resolve<DataAccessService>();
            //_locationHelper.SetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAuditLogOverviewWebApiTest()
        {
            //insert location to create some audit logs
            var locationDetails = _locationHelper.CreateLocationDetails(_locationHelper.GetCountry("CH"),
                _locationHelper.GetFirstLocationType());

            var response = PostAsync(locationDetails, "/api/v1/locations").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<LocationDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            responseDetails.Description = "Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(responseDetails, "/api/v1/locations/" + responseDetails.Id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            var auditLogOverview = GetAsync<IEnumerable<AuditLogOverview>>(Uri + "/location").Result;
            Assert.IsNotNull(auditLogOverview);
            Assert.IsTrue(auditLogOverview.Any());

            var auditLogOverviewEntity = GetAsync<IEnumerable<AuditLogOverview>>(Uri + "/location/" + auditLogOverview.Last().RecordId).Result;
            Assert.IsNotNull(auditLogOverviewEntity);
            Assert.IsTrue(auditLogOverviewEntity.Any());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/auditlogs"; }
        }
    }
}
