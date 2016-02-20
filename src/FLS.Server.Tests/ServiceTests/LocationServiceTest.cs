using System;
using System.Linq;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class LocationServiceTest : BaseServiceTest
    {
        private LocationService _locationService;
        private LocationHelper _locationHelper;
        private TestContext _testContextInstance;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _locationService = UnityContainer.Resolve<LocationService>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
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
            var locations = _locationService.GetLocations();
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
            var locations = _locationService.GetLocationOverviews();
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
            var country = _locationHelper.GetFirstCountry();
            var locationType = _locationHelper.GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var location = _locationHelper.CreateLocation(country, locationType);
            
            Assert.AreEqual(location.Id, Guid.Empty);
            _locationService.InsertLocation(location);

            var loadedLocation = _locationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertLocationWithElevationAndRunwayLengthTest()
        {
            var country = _locationHelper.GetFirstCountry();
            var locationType = _locationHelper.GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid);
            var elevationUnitType = _locationHelper.GetFirstElevationUnitType();
            var lengthUnitType = _locationHelper.GetFirstLengthUnitType();
            var location = _locationHelper.CreateLocation(country, locationType, elevationUnitType, lengthUnitType);

            Assert.AreEqual(location.Id, Guid.Empty);
            _locationService.InsertLocation(location);

            var loadedLocation = _locationService.GetLocation(location.Id);

            Assert.IsNotNull(loadedLocation);
            Assert.AreEqual(location.LocationName, loadedLocation.LocationName);
        }
        
        [TestMethod]
        [TestCategory("Service")]
        public void UpateLocationDetailsTest()
        {
            var locations = _locationService.GetLocations(true);
            var location = locations.First();
            var locationDetail = _locationService.GetLocationDetails(location.LocationId);

            Assert.IsNotNull(locationDetail);

            locationDetail.LocationName = locationDetail.LocationName;
            _locationService.UpdateLocationDetails(locationDetail);

            locationDetail.LocationName = locationDetail.LocationShortName + DateTime.Now.ToShortTimeString();
            _locationService.UpdateLocationDetails(locationDetail);

            var loadedLocationDetails = _locationService.GetLocationDetails(location.Id);

            Assert.IsNotNull(loadedLocationDetails);
            Assert.AreEqual(locationDetail.LocationName, loadedLocationDetails.LocationName);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void DeleteLocationTest()
        {
            var locations = _locationService.GetLocations(true).ToList();
            var location = locations.First();

            Assert.IsNotNull(location, "No location available");

            _locationService.DeleteLocation(location.LocationId);

            var locationsAfterDelete = _locationService.GetLocations(true).ToList();

            Assert.IsTrue(locationsAfterDelete.Count < locations.Count, string.Format("Location count condition is wrong. Nr of locations after delete: {0}, nr of locations before: {1}", locationsAfterDelete.Count, locations.Count));
        }
    }
}
