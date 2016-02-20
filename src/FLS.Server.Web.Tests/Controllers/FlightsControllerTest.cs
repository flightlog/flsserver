using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Location;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Web.Tests.Controllers
{
    [TestClass]
    public class FlightsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        public void GetFlightsOverviewTest()
        {
            var response = GetAsync<IEnumerable<FlightOverview>>("/api/v1/flights").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void GetFlightsDetailsTest()
        {
            var response = GetAsync<IEnumerable<FlightOverview>>("/api/v1/flights").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().FlightId;

            var result = GetAsync<FlightDetails>("/api/v1/flights/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        protected override string Uri
        {
            get { return "/api/v1/flights"; }
        }
    }
}
