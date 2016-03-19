using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AircraftReservationsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftReservationsOverviewWebApiTest()
        {
            InsertAircraftReservationsWebApiTest();

            var response = GetAsync<IEnumerable<AircraftReservationOverview>>(RoutePrefix).Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftReservationsDetailsWebApiTest()
        {
            InsertAircraftReservationsWebApiTest();

            var response = GetAsync<IEnumerable<AircraftReservationOverview>>(RoutePrefix).Result;

            Assert.IsTrue(response.Any());

            var id = response.First().AircraftReservationId;

            var result = GetAsync<AircraftReservationDetails>(RoutePrefix + "/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertAircraftReservationsWebApiTest()
        {
            var reservation = CreateAircraftReservationDetails();

            var response = PostAsync(reservation, RoutePrefix).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var reservationDetails = ConvertToModel<AircraftReservationDetails>(response);
            Assert.IsTrue(reservationDetails.AircraftReservationId.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", reservationDetails));
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircraftreservations"; }
        }
    }
}
