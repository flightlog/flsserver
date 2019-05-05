using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter();
            searchFilter.Sorting.Add("LocationName", "asc");

            var response = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/{pageSize}").Result;

            var result = ConvertToModel<PagedList<LocationOverview>>(response);

            Assert.AreEqual(pageSize, result.Items.Count, 0, "PageSize does not fit with items count in list.");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPagedOrderByLocationOverviewWebApiTest()
        {
            var searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter();
            searchFilter.Sorting.Add("CountryName", "asc");
            searchFilter.Sorting.Add("LocationTypeName", "asc");
            searchFilter.Sorting.Add("LocationName", "asc");

            var response = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/100").Result;

            var result = ConvertToModel<PagedList<LocationOverview>>(response);

            Assert.IsTrue(2 <= result.Items.Count, "PageSize does not fit with items count in list.");

            searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter();
            searchFilter.Sorting.Add("LocationShortName", "asc");
            searchFilter.Sorting.Add("LocationName", "desc");
            var responseDesc = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/100").Result;

            var resultDesc = ConvertToModel<PagedList<LocationOverview>>(responseDesc);
            Assert.IsTrue(2 <= resultDesc.Items.Count, "PageSize does not fit with items count in list.");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetSearchFilterLocationOverviewWebApiTest()
        {
            var searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter()
            {
                LocationName = "Speck",
                IcaoCode = "LSZK",
                CountryName = "Schweiz",
                IsAirfield = true,
            };

            var response = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1").Result;

            var result = ConvertToModel<PagedList<LocationOverview>>(response);

            Assert.AreEqual(1, result.Items.Count, "Search filter returned to many records in step 1.");

            searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter()
            {
                LocationName = "Speck"
            };
            var response2 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/").Result;

            var result2 = ConvertToModel<PagedList<LocationOverview>>(response2);
            Assert.AreEqual(1, result2.Items.Count, "Search filter returned to many records in step 2.");

            searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter()
            {
                IcaoCode = "LSZK"
            };
            var response3 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/50").Result;

            var result3 = ConvertToModel<PagedList<LocationOverview>>(response3);
            Assert.AreEqual(1, result3.Items.Count, "Search filter returned to many records in step 3.");

            searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter()
            {
                IsAirfield = true
            };
            var response4 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/20").Result;

            var result4 = ConvertToModel<PagedList<LocationOverview>>(response4);
            Assert.IsTrue(result4.Items.Count > 1, "It should have more than 1 airfield in locations table.");

            searchFilter = new PageableSearchFilter<LocationOverviewSearchFilter>();
            searchFilter.SearchFilter = new LocationOverviewSearchFilter()
            {
                LocationName = "sPECK"
            };
            var response5 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page/1/80").Result;

            var result5 = ConvertToModel<PagedList<LocationOverview>>(response5);
            Assert.AreEqual(1, result5.Items.Count, "Search filter returned to many records in step 5.");

            var response6 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page?pageStart=1&pageSize=44").Result;

            var result6 = ConvertToModel<PagedList<LocationOverview>>(response6);
            Assert.AreEqual(1, result6.Items.Count, "Search filter returned to many records in step 6.");

            var response7 = PostAsync<PageableSearchFilter<LocationOverviewSearchFilter>>(searchFilter, $"/api/v1/locations/page").Result;

            var result7 = ConvertToModel<PagedList<LocationOverview>>(response7);
            Assert.AreEqual(1, result7.Items.Count, "Search filter returned to many records in step 7.");

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
