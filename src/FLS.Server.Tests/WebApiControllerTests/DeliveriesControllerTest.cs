using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.Mapping;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class DeliveriesControllerTest : BaseAuthenticatedTests
    {
        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAllFlightInvoiceDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<DeliveryDetails>>("/api/v1/invoices").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void CreateDeliveriesWebApiTest()
        {
            var flight = CreateGliderFlight(CurrentIdentityUser.ClubId, DateTime.Now.AddHours(-1));
            Assert.IsNotNull(flight);
            var flightDetails = flight.ToFlightDetails();
            Assert.IsNotNull(flightDetails);
            FlightService.InsertFlightDetails(flightDetails);
            Assert.AreNotEqual(Guid.Empty, flightDetails.FlightId, "FlightId is empty or invalid");

            SetFlightAsLocked(flightDetails);

            var response = GetAsync("/api/v1/deliveries/create").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);

            var deliveries = GetAsync<List<DeliveryDetails>>("/api/v1/deliveries/notprocessed").Result;

            Assert.IsTrue(deliveries.Any());
        }
        
        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/invoices"; }
        }
    }
}
