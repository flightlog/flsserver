using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Flight;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class FlightStatesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetFlightStatesListItemsWebApiTest()
        {
            var flightStates = GetAsync<IEnumerable<FlightStateListItem>>(Uri).Result;
            Assert.IsTrue(flightStates.Any());

            var flightStates1 = GetAsync<IEnumerable<FlightStateListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(flightStates1.Any());

            Assert.AreEqual(flightStates.Count(), flightStates1.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/flightstates"; }
        }
    }
}
