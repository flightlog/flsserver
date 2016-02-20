using System;
using System.Diagnostics;
using System.Linq;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace FLS.Server.Service.Tests
{
    [TestClass]
    public class LocationServiceTest : BaseServiceTest
    {
        private TestContext _testContextInstance;

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
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void LocationServiceTestInitialize()
        {
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
        public void GetLocationsTest()
        {
            var locationService = new LocationService();
            var locations = locationService.GetLocations();
            Assert.IsNotNull(locations);

            Logger.Debug(string.Format("GetLocationsTest result: Found {0} location records in database",
                                       locations.Count));

            foreach (var location in locations)
            {
                Logger.Debug(location.LocationName);
            }
        }

        [TestMethod]
        public void GetLocationOverviewsTest()
        {
            var locationService = new LocationService();
            var locations = locationService.GetLocationOverviews();
            Assert.IsNotNull(locations);

            Logger.Debug(string.Format("GetLocationOverviewsTest result: Found {0} location records in database",
                                       locations.Count));

            foreach (var location in locations)
            {
                Logger.Debug(location.LocationName);
            }
        }

        [TestMethod]
        public void InsertLocationTest()
        {
            var locationService = new LocationService();

            var country = LocationHelper.GetFirstCountry();
            var locationType = LocationHelper.GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var location = LocationHelper.CreateLocation(country, locationType);
            
            Assert.AreEqual(location.Id, Guid.Empty);
            locationService.InsertLocation(location);

            var loadedLocation = locationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }

        [TestMethod]
        public void InsertLocationWithElevationAndRunwayLengthTest()
        {
            var locationService = new LocationService();

            var country = LocationHelper.GetFirstCountry();
            var locationType = LocationHelper.GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var elevationUnitType = LocationHelper.GetFirstElevationUnitType();
            var lengthUnitType = LocationHelper.GetFirstLengthUnitType();
            var location = LocationHelper.CreateLocation(country, locationType, elevationUnitType, lengthUnitType);

            Assert.AreEqual(location.Id, Guid.Empty);
            locationService.InsertLocation(location);

            var loadedLocation = locationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }
        
        [TestMethod]
        public void UpateLocationTest()
        {
            var locationService = new LocationService();

            var location = locationService.GetLocations(true).First();

            Assert.IsNotNull(location);

            location.LocationName = location.LocationShortName + DateTime.Now.ToShortTimeString();
            locationService.UpdateLocation(location);

            var loadedLocation = locationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }

        [TestMethod]
        public void UpateLocationDetailsTest()
        {
            var locationService = new LocationService();

            var locations = locationService.GetLocations(true);
            var location = locations.First();
            var locationDetail = locationService.GetLocationDetails(location.LocationId);

            Assert.IsNotNull(locationDetail);

            locationDetail.LocationName = locationDetail.LocationShortName + DateTime.Now.ToShortTimeString();
            locationService.UpdateLocationDetails(locationDetail);

            var loadedLocationDetails = locationService.GetLocationDetails(location.Id);

            Assert.IsNotNull(loadedLocationDetails);
            Assert.AreEqual(locationDetail.LocationName, loadedLocationDetails.LocationName);
        }

        [TestMethod]
        public void DeleteLocationTest()
        {
            var locationService = new LocationService();

            var locations = locationService.GetLocations(true);
            var location = locations.First();

            Assert.IsNotNull(location);

            locationService.DeleteLocation(location.LocationId);

            var locationsAfterDelete = locationService.GetLocations(true);

            Assert.IsTrue(locationsAfterDelete.Count < locations.Count);
        }
    }
}
