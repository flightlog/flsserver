using System;
using System.Linq;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using NLog;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class LocationServiceTest : BaseTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void LocationServiceTestInitialize(TestContext testContext)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            //using (var context = new FLSDataEntities())
            //{
            //    var countries = context.Countries.ToList();
            //}
            //sw.Stop();
            //Console.WriteLine(string.Format("Database connection and loading all countries took: {0} ms", sw.ElapsedMilliseconds));
        }
        
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        [TestCategory("Service")]
        public void GetLocationsTest()
        {
            var locations = LocationService.GetLocations();
            Assert.IsNotNull(locations);

            Logger.Debug(string.Format("GetLocationsTest result: Found {0} location records in database",
                                       locations.Count));

            foreach (var location in locations)
            {
                Logger.Debug(location.LocationName);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetLocationOverviewsTest()
        {
            var locations = LocationService.GetLocationOverviews();
            Assert.IsNotNull(locations);

            Logger.Debug(string.Format("GetLocationOverviewsTest result: Found {0} location records in database",
                                       locations.Count));

            foreach (var location in locations)
            {
                Logger.Debug(location.LocationName);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertLocationTest()
        {
            var country = GetFirstCountry();
            var locationType = GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var location = CreateLocation(country, locationType);
            
            Assert.AreEqual(location.Id, Guid.Empty);
            LocationService.InsertLocation(location);

            var loadedLocation = LocationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertLocationWithElevationAndRunwayLengthTest()
        {
            var country = GetFirstCountry();
            var locationType = GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var elevationUnitType = GetFirstElevationUnitType();
            var lengthUnitType = GetFirstLengthUnitType();
            var location = CreateLocation(country, locationType, elevationUnitType, lengthUnitType);

            Assert.AreEqual(location.Id, Guid.Empty);
            LocationService.InsertLocation(location);

            var loadedLocation = LocationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }
        
        [TestMethod]
        [TestCategory("Service")]
        public void UpateLocationDetailsTest()
        {
            var locations = LocationService.GetLocations(true);
            var location = locations.First();
            var locationDetail = LocationService.GetLocationDetails(location.LocationId);

            Assert.IsNotNull(locationDetail);

            locationDetail.LocationName = locationDetail.LocationName;
            LocationService.UpdateLocationDetails(locationDetail);

            locationDetail.LocationName = locationDetail.LocationShortName + DateTime.Now.ToShortTimeString();
            LocationService.UpdateLocationDetails(locationDetail);

            var loadedLocationDetails = LocationService.GetLocationDetails(location.Id);

            Assert.IsNotNull(loadedLocationDetails);
            Assert.AreEqual(locationDetail.LocationName, loadedLocationDetails.LocationName);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void DeleteLocationTest()
        {
            var locations = LocationService.GetLocations(true).ToList();
            var location = locations.First();

            Assert.IsNotNull(location, "No location available");

            LocationService.DeleteLocation(location.LocationId);

            var locationsAfterDelete = LocationService.GetLocations(true).ToList();

            Assert.IsTrue(locationsAfterDelete.Count < locations.Count, string.Format("Location count condition is wrong. Nr of locations after delete: {0}, nr of locations before: {1}", locationsAfterDelete.Count, locations.Count));
        }
    }
}
