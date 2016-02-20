using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Location;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class CountriesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetCountryListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<CountryListItem>>(Uri).Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<CountryListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetCountryOverviewsWebApiTest()
        {
            var response = GetAsync<IEnumerable<CountryOverview>>(Uri + "/overview").Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<CountryListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/countries"; }
        }
    }
}
