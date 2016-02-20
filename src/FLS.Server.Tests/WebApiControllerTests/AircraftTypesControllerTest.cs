using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AircraftTypesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftStatesListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftTypeListItem>>(Uri).Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<AircraftTypeListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircrafttypes"; }
        }
    }
}
