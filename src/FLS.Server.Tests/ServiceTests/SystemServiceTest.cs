using System;
using System.Linq;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class SystemServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Service")]
        public void GetTrackedEntityNamesTest()
        {
            var location = LocationService.GetLocations().FirstOrDefault();

            Assert.IsNotNull(location);

            var details = LocationService.GetLocationDetails(location.LocationId);

            details.LocationName = "Test" + DateTime.Now.Ticks;

            LocationService.UpdateLocationDetails(details);

            var trackedEntities = SystemService.GetTrackedEntityNames();
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
            var location = LocationService.GetLocations().FirstOrDefault();

            Assert.IsNotNull(location);

            var details = LocationService.GetLocationDetails(location.LocationId);

            details.LocationName = "Test" + DateTime.Now.Ticks;

            LocationService.UpdateLocationDetails(details);

            var trackedEntities = SystemService.GetTrackedEntityNames();
            Assert.IsTrue(trackedEntities.Any());

            foreach (var item in trackedEntities)
            {
                var auditLogOverviews = SystemService.GetAuditLogOverviews(item);

                Assert.IsTrue(auditLogOverviews.Any());

                foreach (var auditLogOverview in auditLogOverviews)
                {
                    Assert.IsNotNull(auditLogOverview);
                }
            }
        }
    }
}
