using System;
using System.Linq;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class SystemServiceTest : BaseServiceTest
    {
        private SystemService _systemService;
        private LocationService _locationService;
        private TestContext _testContextInstance;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _systemService = UnityContainer.Resolve<SystemService>();
            _locationService = UnityContainer.Resolve<LocationService>();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetTrackedEntityNamesTest()
        {
            var location = _locationService.GetLocations().FirstOrDefault();

            Assert.IsNotNull(location);

            var details = _locationService.GetLocationDetails(location.LocationId);

            details.LocationName = "Test" + DateTime.Now.Ticks;

            _locationService.UpdateLocationDetails(details);

            var trackedEntities = _systemService.GetTrackedEntityNames();
            Assert.IsTrue(trackedEntities.Any());

            Logger.Debug(string.Format("GetTrackedEntityNamesTest result: Found {0} tracked entities in database",
                                       trackedEntities.Count));

            foreach (var item in trackedEntities)
            {
                Logger.Debug(item);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetAuditLogOverviewsTest()
        {
            var location = _locationService.GetLocations().FirstOrDefault();

            Assert.IsNotNull(location);

            var details = _locationService.GetLocationDetails(location.LocationId);

            details.LocationName = "Test" + DateTime.Now.Ticks;

            _locationService.UpdateLocationDetails(details);

            var trackedEntities = _systemService.GetTrackedEntityNames();
            Assert.IsTrue(trackedEntities.Any());

            foreach (var item in trackedEntities)
            {
                var auditLogOverviews = _systemService.GetAuditLogOverviews(item);

                Assert.IsTrue(auditLogOverviews.Any());

                foreach (var auditLogOverview in auditLogOverviews)
                {
                    Assert.IsNotNull(auditLogOverview);
                }
            }
        }
    }
}
