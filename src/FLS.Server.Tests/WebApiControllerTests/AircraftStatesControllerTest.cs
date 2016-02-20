using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AircraftStatesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftStatesListItemsWebApiTest()
        {
            var aircraftStates = GetAsync<IEnumerable<AircraftStateListItem>>(Uri).Result;
            Assert.IsTrue(aircraftStates.Any());

            var aircraftStates1 = GetAsync<IEnumerable<AircraftStateListItem>>(Uri + "/listitems").Result;
            Assert.IsTrue(aircraftStates1.Any());

            Assert.AreEqual(aircraftStates.Count(), aircraftStates1.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircraftstates"; }
        }
    }
}
