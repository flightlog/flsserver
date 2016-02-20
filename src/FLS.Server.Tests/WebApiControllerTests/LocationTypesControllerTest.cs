using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Location;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class LocationTypesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetLocationTypeListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<LocationTypeListItem>>(Uri).Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<LocationTypeListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/locationtypes"; }
        }
    }
}
