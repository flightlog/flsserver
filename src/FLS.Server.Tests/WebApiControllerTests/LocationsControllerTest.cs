using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class LocationsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetLocationOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPagedLocationOverviewWebApiTest()
        {
            var pageSize = 2;
            var response = GetAsync<PagedList<LocationOverview>>("/api/v1/locations/1/" + pageSize).Result;

            Assert.AreEqual(pageSize, response.Items.Count, 0, "PageSize does not fit with items count in list.");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPagedOrderByLocationOverviewWebApiTest()
        {
            var pageSize = 2000;
            var orderBy = "Locationname";
            var filter = new LocationSearchFilter()
            {
                SearchText = "Speck",
                SearchInLocationName = true,
                //SearchInIcaoCode = true
            };

            var response = PostAsync<LocationSearchFilter>(filter, $"/api/v1/locations/1/{pageSize}/{orderBy}").Result;

            var result = ConvertToModel<PagedList<LocationOverview>>(response);

            Assert.IsTrue(pageSize >= result.Items.Count, "PageSize does not fit with items count in list.");

            orderBy = "LocationShortName:desc,Locationname:desc";
            var responseDesc = PostAsync<LocationSearchFilter>(filter, $"/api/v1/locations/1/{pageSize}/{orderBy}").Result;

            var resultDesc = ConvertToModel<PagedList<LocationOverview>>(responseDesc);
            Assert.IsTrue(pageSize >= resultDesc.Items.Count, "PageSize does not fit with items count in list.");

            Assert.IsTrue(result.Items.First().Id == resultDesc.Items.Last().Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetLocationDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().LocationId;

            var result = GetAsync<LocationDetails>("/api/v1/locations/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertLocationDetailsWebApiTest()
        {
            var location = new LocationDetails();
            location.LocationName = "Location @ " + DateTime.Now.ToShortTimeString();
            var country = GetCountry("CH");
            Assert.IsNotNull(country);
            location.CountryId = country.CountryId;

            var locationType = GetFirstLocationType();
            Assert.IsNotNull(locationType);
            location.LocationTypeId = locationType.LocationTypeId;
            Assert.AreEqual(location.Id, Guid.Empty);

            var response = PostAsync(location, "/api/v1/locations").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<LocationDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void LocationDetailsValidationWebApiTest()
        {
            var response = PostAsync<LocationDetails>(null, "/api/v1/locations").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            var country = GetCountry("CH");
            Assert.IsNotNull(country);

            var locationType = GetFirstLocationType();
            Assert.IsNotNull(locationType);

            var location = new LocationDetails();
            response = PostAsync(location, "/api/v1/locations").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            location = new LocationDetails();
            location.LocationName = "Location 1 @ " + DateTime.Now.ToShortTimeString();
            response = PostAsync(location, "/api/v1/locations").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            location = new LocationDetails();
            location.LocationName = "Location 2 @ " + DateTime.Now.ToShortTimeString();
            location.CountryId = country.CountryId;
            response = PostAsync(location, "/api/v1/locations").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            location = new LocationDetails();
            location.LocationName = "Too long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long long Location @ " + DateTime.Now.ToShortTimeString();
            location.CountryId = country.CountryId;
            location.LocationTypeId = locationType.LocationTypeId;
            response = PostAsync(location, "/api/v1/locations").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            location = new LocationDetails();
            location.LocationName = "Location 3 @ " + DateTime.Now.ToShortTimeString();
            location.CountryId = country.CountryId;
            location.LocationTypeId = locationType.LocationTypeId;
            response = PostAsync(location, "/api/v1/locations").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateLocationDetailsWebApiTest()
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
        [TestCategory("WebApi")]
        public void DeleteLocationDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().LocationId;

            var delResult = DeleteAsync("/api/v1/locations/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/locations"; }
        }
    }
}
