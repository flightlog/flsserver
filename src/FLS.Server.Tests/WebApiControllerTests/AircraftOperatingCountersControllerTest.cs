using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Flight;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AircraftOperatingCountersControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftEngineOperatingCounterResultOverviewWebApiTest()
        {
            //create new motor aircraft
            var aircraftDetails = CreateMotorAircraftDetails();
            var response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseAircraftDetails = ConvertToModel<AircraftDetails>(response);
            Assert.IsNotNull(responseAircraftDetails);
            Assert.IsTrue(responseAircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseAircraftDetails));
            
            //create new aircraft operating counter details for aircraft
            var newCounter = new AircraftOperatingCounterDetails()
            {
                AircraftId = responseAircraftDetails.AircraftId,
                AtDateTime = new DateTime(2015, 1, 1),
                EngineOperatingCounterInSeconds = 75300,
                FlightOperatingCounterInSeconds = 75300,
                TotalSelfStarts = 150,
                NextMaintenanceAtFlightOperatingCounterInSeconds = 78000,
                NextMaintenanceAtEngineOperatingCounterInSeconds = 12000
            };

            var newCounterResponse = PostAsync(newCounter, "/api/v1/aircraftoperatingcounters").Result;

            Assert.IsTrue(newCounterResponse.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", newCounterResponse.StatusCode));
            var newCounterResponseDetails = ConvertToModel<AircraftOperatingCounterDetails>(newCounterResponse);
            Assert.IsNotNull(newCounterResponseDetails);
            Assert.IsTrue(newCounterResponseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", newCounterResponseDetails));

            var request = new AircraftOperatingCounterRequest
            {
                AircraftId = responseAircraftDetails.AircraftId,
                AtDateTime = DateTime.UtcNow
            };

            var requestResponse = PostAsync<AircraftOperatingCounterRequest>(request, RoutePrefix + "/request").Result;

            Assert.IsTrue(requestResponse.IsSuccessStatusCode);
            var result = ConvertToModel<AircraftOperatingCounterResult>(requestResponse);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.AircraftHasNoEngine);
            Assert.IsTrue(result.EngineOperatingCounterInSeconds == 75300);

            var flightDetails = CreateMotorFlightDetails(ClubId);
            flightDetails.MotorFlightDetailsData.AircraftId = responseAircraftDetails.AircraftId;
            flightDetails.MotorFlightDetailsData.StartDateTime = new DateTime(2015, 1, 10, 10, 0, 0);
            flightDetails.MotorFlightDetailsData.LdgDateTime = new DateTime(2015, 1, 10, 11, 15, 0);

            var responsedFlight = PostAsync(flightDetails, "/api/v1/flights").Result;

            Assert.IsTrue(responsedFlight.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", responsedFlight.StatusCode));
            var responseFlightDetails = ConvertToModel<FlightDetails>(responsedFlight);
            Assert.IsTrue(responseFlightDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseFlightDetails));

            var requestResponse2 = PostAsync<AircraftOperatingCounterRequest>(request, RoutePrefix + "/request").Result;

            Assert.IsTrue(requestResponse2.IsSuccessStatusCode);
            var result2 = ConvertToModel<AircraftOperatingCounterResult>(requestResponse2);
            Assert.IsNotNull(result2);
            Assert.IsFalse(result2.AircraftHasNoEngine);
            Assert.IsTrue(result2.EngineOperatingCounterInSeconds == 79800);
        }


        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftOperatingCounterOverviewWebApiTest()
        {
            //InsertAircraftReservationsWebApiTest();
            var aircraftId = GetFirstTowingAircraft().AircraftId;
            var response = GetAsync<IEnumerable<AircraftOperatingCounterOverview>>(RoutePrefix + "/aircraft/" + aircraftId).Result;

            //Assert.IsTrue(response.Any());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircraftoperatingcounters"; }
        }
    }
}
