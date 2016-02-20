using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLS.Data.WebApi.Location;
using FLS.Server.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Web.Tests.Controllers
{
    [TestClass]
    public class LocationsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        public void GetLocationOverviewTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void GetLocationDetailsTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().LocationId;

            var result = GetAsync<LocationDetails>("/api/v1/locations/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public void PutLocationDetailsTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().LocationId;

            var result = GetAsync<LocationDetails>("/api/v1/locations/" + id).Result;

            Assert.AreEqual(id, result.Id);

            result.Description = "Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(result, "/api/v1/locations/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteLocationDetailsTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().LocationId;

            var delResult = DeleteAsync("/api/v1/locations/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostLocationDetailsTest()
        {
            var location = new LocationDetails();
            location.LocationName = "Location @ " + DateTime.Now.ToShortTimeString();
            var country = LocationHelper.GetCountry("CH");
            Assert.IsNotNull(country);
            location.CountryId = country.CountryId;

            var locationType = LocationHelper.GetFirstLocationType();
            Assert.IsNotNull(locationType);
            location.LocationTypeId = locationType.LocationTypeId;
            Assert.AreEqual(location.Id, Guid.Empty);

            var response = PostAsync(location, "/api/v1/locations").Result;
            
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return "/api/v1/locations"; }
        }
    }
}
