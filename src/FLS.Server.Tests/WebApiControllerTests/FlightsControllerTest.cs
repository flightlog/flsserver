using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Flight;
using FLS.Server.Service;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class FlightsControllerTest : BaseAuthenticatedTests
    {
        #region MotorFlights

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertMotorFlightDetailsWebApiTest()
        {
            var overview = GetAsync<IEnumerable<FlightOverview>>("/api/v1/flights").Result;

            Assert.IsTrue(overview.Any());

            var flightDetails = CreateMotorFlightDetails(ClubId);

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var overviewNew = GetAsync<IEnumerable<FlightOverview>>("/api/v1/flights").Result;
            Assert.IsTrue(overviewNew.Any());

            Assert.AreEqual(overview.Count() + 1, overviewNew.Count(), "Number of flights does not match the test");
        }
        #endregion MotorFlights

        #region GliderFlights
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetGliderFlightsOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights/gliderflights").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetGliderFlightsDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights/gliderflights").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().FlightId;

            var result = GetAsync<FlightDetails>("/api/v1/flights/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetGliderFlightsDetailsOfAllGliderFlightsWebApiTest()
        {
            var response = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights/gliderflights").Result;

            Assert.IsTrue(response.Any());

            foreach (var flightOverview in response)
            {
                var result = GetAsync<FlightDetails>("/api/v1/flights/" + flightOverview.FlightId).Result;

                Assert.AreEqual(flightOverview.FlightId, result.Id);
                Assert.IsNotNull(result.GliderFlightDetailsData);
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertGliderFlightDetailsWebApiTest()
        {
            var flightDetails = CreateGliderFlightDetails(ClubId);
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;
            flightDetails.GliderFlightDetailsData.PassengerPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var overview = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights/gliderflights").Result;
            Assert.IsTrue(overview.Any());
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertOneSeatGliderFlightDetailsWebApiTest()
        {
            var flightDetails = CreateOneSeatGliderFlightDetails(ClubId);
            flightDetails.GliderFlightDetailsData.FlightTypeId = GetFirstSoloGliderFlightType(ClubId).FlightTypeId;

            //add some other stuff, which must be removed by server logic as it is only a one seat glider
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;
            flightDetails.GliderFlightDetailsData.PassengerPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            Assert.IsTrue(responseDetails.GliderFlightDetailsData.CoPilotPersonId.HasValue == false, string.Format("Copilot is set serverside on one seated glider flight: {0}", responseDetails));
            Assert.IsTrue(responseDetails.GliderFlightDetailsData.PassengerPersonId.HasValue == false, string.Format("Passenger is set serverside on one seated glider flight: {0}", responseDetails));

            var overview = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights/gliderflights").Result;
            Assert.IsTrue(overview.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertMinimalGliderFlightDetailsWebApiTest()
        {
            var flightDetails = CreateMinimalGliderFlightDetails(ClubId);

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateFlightsDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().FlightId;

            var flightDetails = GetAsync<FlightDetails>("/api/v1/flights/" + id).Result;

            Assert.AreEqual(id, flightDetails.Id);

            if (flightDetails.GliderFlightDetailsData != null)
            {
                var flightComment = DateTime.Now.ToShortTimeString();
                flightDetails.GliderFlightDetailsData.FlightComment = flightComment;

                var putResult = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;

                Assert.IsTrue(putResult.IsSuccessStatusCode);

                Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightComment, flightComment);
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertAndUpdateSchoolFlightDetailsWebApiTest()
        {
            var utcNow = DateTime.UtcNow;

            //create a minimal glider flight (set only aircraft and trainee)
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = GetFirstDoubleSeatGlider().AircraftId;
            gliderData.PilotPersonId = GetFirstGliderTraineePerson(ClubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responsedFlightDetails = ConvertToModel<FlightDetails>(response);

            //flightDetails.GliderFlightDetailsData.FlightState = (int) FlightState.New;
            //var expectedFlightDetails = new Likeness<FlightDetails, FlightDetails>(flightDetails);
            //expectedFlightDetails.Without(x => x.FlightId);
            //Assert.AreEqual(expectedFlightDetails, responsedFlightDetails);

            Assert.IsTrue(responsedFlightDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responsedFlightDetails));
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual((int)FlightAirState.New, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNull(responsedFlightDetails.TowFlightDetailsData);
            var flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNull(flight.ModifiedOn);
            Assert.IsNull(flight.ModifiedByUserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);
            Assert.IsNull(flight.TowFlightId);

            flightDetails = responsedFlightDetails;

            //update with Flight Type and instructor person
            flightDetails.GliderFlightDetailsData.FlightTypeId =
                GetFirstInstructorRequiredGliderFlightType(ClubId).FlightTypeId;
            flightDetails.GliderFlightDetailsData.InstructorPersonId =
                GetFirstGliderInstructorPerson(ClubId).PersonId;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.New, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNull(responsedFlightDetails.TowFlightDetailsData);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);

            flightDetails = responsedFlightDetails;

            //add Towflight data
            var towFlightData = new TowFlightDetailsData();
            towFlightData.AircraftId = GetFirstTowingAircraft().AircraftId;
            towFlightData.PilotPersonId = GetFirstTowingPilotPerson(ClubId).PersonId;
            towFlightData.FlightTypeId = GetFirstTowingFlightType(ClubId).FlightTypeId;
            flightDetails.TowFlightDetailsData = towFlightData;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.New, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNotNull(responsedFlightDetails.TowFlightDetailsData); 
            Assert.AreEqual(flightDetails.TowFlightDetailsData.AircraftId, responsedFlightDetails.TowFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.PilotPersonId, responsedFlightDetails.TowFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.FlightTypeId, responsedFlightDetails.TowFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.New, responsedFlightDetails.TowFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.TowFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.TowFlightDetailsData.NrOfLdgs.Value == 0);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);
            
            var towFlight = GetFlight(responsedFlightDetails.TowFlightDetailsData.FlightId);
            Assert.IsNotNull(towFlight);
            Assert.IsNull(towFlight.ModifiedOn);
            Assert.IsNull(towFlight.ModifiedByUserId);
            Assert.IsTrue(towFlight.CreatedOn >= utcNow);
            Assert.AreEqual(towFlight.CreatedByUserId, MyUserDetails.UserId);

            flightDetails = responsedFlightDetails;

            //start the flight
            var startTime = DateTime.Now;
            flightDetails.GliderFlightDetailsData.StartDateTime = startTime;
            flightDetails.GliderFlightDetailsData.StartLocationId = GetFirstLocation().LocationId;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.Started, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNotNull(responsedFlightDetails.TowFlightDetailsData);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.AircraftId, responsedFlightDetails.TowFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.PilotPersonId, responsedFlightDetails.TowFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.FlightTypeId, responsedFlightDetails.TowFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.Started, responsedFlightDetails.TowFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.TowFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.TowFlightDetailsData.NrOfLdgs.Value == 0);

            Assert.IsNotNull(flightDetails.GliderFlightDetailsData);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.GliderFlightDetailsData.StartLocationId);
            //check if the towflight data are set based on the glider flight data
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartDateTime, responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);

            towFlight = GetFlight(responsedFlightDetails.TowFlightDetailsData.FlightId);
            Assert.IsNotNull(towFlight);
            Assert.IsNotNull(towFlight.ModifiedOn);
            Assert.IsTrue(towFlight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(towFlight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(towFlight.CreatedOn >= utcNow);
            Assert.AreEqual(towFlight.CreatedByUserId, MyUserDetails.UserId);

            flightDetails = responsedFlightDetails;

            //land the towing flight
            var towingLdgTime = DateTime.Now;
            flightDetails.TowFlightDetailsData.LdgDateTime = towingLdgTime;
            flightDetails.TowFlightDetailsData.LdgLocationId = flightDetails.TowFlightDetailsData.StartLocationId;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.Started, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNotNull(responsedFlightDetails.TowFlightDetailsData);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.AircraftId, responsedFlightDetails.TowFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.PilotPersonId, responsedFlightDetails.TowFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.FlightTypeId, responsedFlightDetails.TowFlightDetailsData.FlightTypeId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.LdgDateTime);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgLocationId, responsedFlightDetails.TowFlightDetailsData.LdgLocationId);
            Assert.AreEqual((int)FlightAirState.Landed, responsedFlightDetails.TowFlightDetailsData.AirStateId);
            Assert.AreEqual(1, responsedFlightDetails.TowFlightDetailsData.NrOfLdgs);

            Assert.IsNotNull(flightDetails.GliderFlightDetailsData);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.GliderFlightDetailsData.StartLocationId);
            //check if the towflight data are set based on the glider flight data
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartDateTime, responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);

            towFlight = GetFlight(responsedFlightDetails.TowFlightDetailsData.FlightId);
            Assert.IsNotNull(towFlight);
            Assert.IsNotNull(towFlight.ModifiedOn);
            Assert.IsTrue(towFlight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(towFlight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(towFlight.CreatedOn >= utcNow);
            Assert.AreEqual(towFlight.CreatedByUserId, MyUserDetails.UserId);

            flightDetails = responsedFlightDetails;

            //land the glider flight
            var gliderLdgTime = DateTime.Now;
            flightDetails.GliderFlightDetailsData.LdgDateTime = gliderLdgTime;
            flightDetails.GliderFlightDetailsData.LdgLocationId = flightDetails.GliderFlightDetailsData.StartLocationId;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.LdgDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.LdgDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.LdgDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.LdgLocationId, responsedFlightDetails.GliderFlightDetailsData.LdgLocationId);
            Assert.AreEqual((int)FlightAirState.Landed, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.AreEqual(1, responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNotNull(responsedFlightDetails.TowFlightDetailsData);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.AircraftId, responsedFlightDetails.TowFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.PilotPersonId, responsedFlightDetails.TowFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.FlightTypeId, responsedFlightDetails.TowFlightDetailsData.FlightTypeId);
            Assert.IsTrue(flightDetails.TowFlightDetailsData.LdgDateTime.HasValue);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.LdgDateTime);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgLocationId, responsedFlightDetails.TowFlightDetailsData.LdgLocationId);
            Assert.AreEqual((int)FlightAirState.Landed, responsedFlightDetails.TowFlightDetailsData.AirStateId);
            Assert.AreEqual(1, responsedFlightDetails.TowFlightDetailsData.NrOfLdgs);

            Assert.IsNotNull(flightDetails.GliderFlightDetailsData);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.GliderFlightDetailsData.StartLocationId);
            //check if the towflight data are set based on the glider flight data
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartDateTime, responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);

            towFlight = GetFlight(responsedFlightDetails.TowFlightDetailsData.FlightId);
            Assert.IsNotNull(towFlight);
            Assert.IsNotNull(towFlight.ModifiedOn);
            Assert.IsTrue(towFlight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(towFlight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(towFlight.CreatedOn >= utcNow);
            Assert.AreEqual(towFlight.CreatedByUserId, MyUserDetails.UserId);

            flightDetails = responsedFlightDetails;

            //set comment
            var comment = "Test-Comment: Speed to high in short final!";
            flightDetails.GliderFlightDetailsData.FlightComment = comment;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.LdgDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.LdgDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.LdgDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.LdgLocationId, responsedFlightDetails.GliderFlightDetailsData.LdgLocationId);
            Assert.AreEqual((int)FlightAirState.Landed, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.AreEqual(1, responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs);
            Assert.AreEqual(comment, responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNotNull(responsedFlightDetails.TowFlightDetailsData);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.AircraftId, responsedFlightDetails.TowFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.PilotPersonId, responsedFlightDetails.TowFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.FlightTypeId, responsedFlightDetails.TowFlightDetailsData.FlightTypeId);
            Assert.IsTrue(flightDetails.TowFlightDetailsData.LdgDateTime.HasValue);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.LdgDateTime);
            Assert.AreEqual(flightDetails.TowFlightDetailsData.LdgLocationId, responsedFlightDetails.TowFlightDetailsData.LdgLocationId);
            Assert.AreEqual((int)FlightAirState.Landed, responsedFlightDetails.TowFlightDetailsData.AirStateId);
            Assert.AreEqual(1, responsedFlightDetails.TowFlightDetailsData.NrOfLdgs);

            Assert.IsNotNull(flightDetails.GliderFlightDetailsData);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.GliderFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.GliderFlightDetailsData.StartLocationId);
            //check if the towflight data are set based on the glider flight data
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartDateTime.Value.ToUniversalTime(), responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartDateTime, responsedFlightDetails.TowFlightDetailsData.StartDateTime);
            Assert.AreEqual(responsedFlightDetails.GliderFlightDetailsData.StartLocationId, responsedFlightDetails.TowFlightDetailsData.StartLocationId);

            flight = GetFlight(responsedFlightDetails.GliderFlightDetailsData.FlightId);
            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.ModifiedOn);
            Assert.IsTrue(flight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(flight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(flight.CreatedOn >= utcNow);
            Assert.AreEqual(flight.CreatedByUserId, MyUserDetails.UserId);

            towFlight = GetFlight(responsedFlightDetails.TowFlightDetailsData.FlightId);
            Assert.IsNotNull(towFlight);
            Assert.IsNotNull(towFlight.ModifiedOn);
            Assert.IsTrue(towFlight.ModifiedOn.Value >= utcNow);
            Assert.AreEqual(towFlight.ModifiedByUserId, MyUserDetails.UserId);
            Assert.IsTrue(towFlight.CreatedOn >= utcNow);
            Assert.AreEqual(towFlight.CreatedByUserId, MyUserDetails.UserId);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void FlightCrewFlightDetailsWebApiTest()
        {
            var utcNow = DateTime.UtcNow;

            //create a minimal glider flight (set only aircraft and trainee, with additional persons
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = GetFirstDoubleSeatGlider().AircraftId;
            gliderData.PilotPersonId = GetFirstGliderTraineePerson(ClubId).PersonId;
            gliderData.CoPilotPersonId = GetFirstGliderPilotPerson(ClubId).PersonId;
            gliderData.InstructorPersonId = GetFirstGliderInstructorPerson(ClubId).PersonId;
            gliderData.PassengerPersonId = GetFirstPerson(ClubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;

            var response = PostAsync(flightDetails, "/api/v1/flights").Result;
            Assert.IsTrue(response.IsSuccessStatusCode,
                          string.Format("Error with Status Code: {0}", response.StatusCode));
            var responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            
            Assert.IsTrue(responsedFlightDetails.Id.IsValid(),
                          string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}",
                                        responsedFlightDetails));
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId,
                            responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual((int) FlightAirState.New, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false ||
                          responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNull(responsedFlightDetails.TowFlightDetailsData);

            //validate flight crew when flight type is not set
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId,
                            responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.CoPilotPersonId, responsedFlightDetails.GliderFlightDetailsData.CoPilotPersonId);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.PassengerPersonId);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);

            flightDetails = responsedFlightDetails;

            //update with Flight Type 
            flightDetails.GliderFlightDetailsData.FlightTypeId =
                GetFirstInstructorRequiredGliderFlightType(ClubId).FlightTypeId;
            flightDetails.GliderFlightDetailsData.InstructorPersonId =
                GetFirstGliderInstructorPerson(ClubId).PersonId;
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetFirstGliderPilotPerson(ClubId).PersonId;
            flightDetails.GliderFlightDetailsData.InstructorPersonId = GetFirstGliderInstructorPerson(ClubId).PersonId;
            flightDetails.GliderFlightDetailsData.PassengerPersonId = GetFirstPerson(ClubId).PersonId;

            response = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responsedFlightDetails = ConvertToModel<FlightDetails>(response);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.AircraftId, responsedFlightDetails.GliderFlightDetailsData.AircraftId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.FlightTypeId, responsedFlightDetails.GliderFlightDetailsData.FlightTypeId);
            Assert.AreEqual((int)FlightAirState.New, responsedFlightDetails.GliderFlightDetailsData.AirStateId);
            Assert.IsTrue(responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.HasValue == false || responsedFlightDetails.GliderFlightDetailsData.NrOfLdgs.Value == 0);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.FlightComment);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CouponNumber);
            Assert.IsNull(responsedFlightDetails.TowFlightDetailsData);

            //validate flight crew when flight type is not set
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.PilotPersonId, responsedFlightDetails.GliderFlightDetailsData.PilotPersonId);
            Assert.AreEqual(flightDetails.GliderFlightDetailsData.InstructorPersonId, responsedFlightDetails.GliderFlightDetailsData.InstructorPersonId);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.CoPilotPersonId);
            Assert.IsNull(responsedFlightDetails.GliderFlightDetailsData.PassengerPersonId);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeleteTowedGliderFlightDetailsWebApiTest()
        {
            //insert a new flight which we can delete later
            var flightDetails = CreateTowedGliderFlightDetails(ClubId);
            var response = PostAsync(flightDetails, "/api/v1/flights").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.IsNotNull(responseDetails.TowFlightDetailsData);
            Assert.IsTrue(responseDetails.TowFlightDetailsData.FlightId.IsValid(), string.Format("Primary key of towing flight not set/mapped after insert or update. Entity-Info: {0}", responseDetails.TowFlightDetailsData));

            var deleteResponse = DeleteAsync("/api/v1/flights/" + responseDetails.FlightId).Result;
            Assert.IsTrue(deleteResponse.IsSuccessStatusCode, string.Format("Error while delete flight with Status Code: {0}", deleteResponse.StatusCode));

            using (var context = DataAccessService.CreateDbContext())
            {
                string sql =
                    string.Format(
                        "SELECT RecordState FROM Flights Where FlightId = '{0}'",
                        responseDetails.TowFlightDetailsData.FlightId);
                var queryResult = context.Database.SqlQuery<int>(sql).ToList();

                Assert.IsTrue(queryResult.Any());
                Assert.IsTrue(queryResult.First() == 99);
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void TowFlightIsSoloFlightDetailsWebApiTest()
        {
            var flightDetails = CreateTowedGliderFlightDetails(ClubId);
            var response = PostAsync(flightDetails, "/api/v1/flights").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.IsNotNull(responseDetails.TowFlightDetailsData);
            Assert.IsTrue(responseDetails.TowFlightDetailsData.FlightId.IsValid(), string.Format("Primary key of towing flight not set/mapped after insert or update. Entity-Info: {0}", responseDetails.TowFlightDetailsData));

            Assert.IsTrue(responseDetails.TowFlightDetailsData.IsSoloFlight);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateAllFlightsDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<GliderFlightOverview>>("/api/v1/flights").Result;

            Assert.IsTrue(response.Any(), "No flights received from server");

            foreach (var flightOverview in response)
            {
                var id = flightOverview.FlightId;

                var flightDetails = GetAsync<FlightDetails>("/api/v1/flights/" + id).Result;

                Assert.IsNotNull(flightDetails, string.Format("FlightDetails is null with FlightId: {0}", id));
                Assert.AreEqual(id, flightDetails.Id, string.Format("Overview.FlightId: {0} not equal with FlightDetails.FlightId: {1}", id, flightDetails.Id));

                if ((flightDetails.GliderFlightDetailsData != null && flightDetails.GliderFlightDetailsData.ProcessStateId >= (int)FlightProcessState.Locked)
                    || (flightDetails.TowFlightDetailsData != null && flightDetails.TowFlightDetailsData.ProcessStateId >= (int)FlightProcessState.Locked)
                    || (flightDetails.MotorFlightDetailsData != null && flightDetails.MotorFlightDetailsData.ProcessStateId >= (int)FlightProcessState.Locked))
                {
                    //can't update record which is locked
                    continue;
                }

                if (flightDetails.GliderFlightDetailsData != null)
                {
                    flightDetails.GliderFlightDetailsData.FlightComment = DateTime.Now.ToShortTimeString();
                }

                if (flightDetails.TowFlightDetailsData != null
                    && flightDetails.TowFlightDetailsData.AircraftId.IsValid()
                    && flightDetails.TowFlightDetailsData.PilotPersonId.IsValid())
                {
                    flightDetails.TowFlightDetailsData.FlightComment = DateTime.Now.ToShortTimeString();
                }
                else if (flightDetails.TowFlightDetailsData != null)
                {
                    flightDetails.TowFlightDetailsData = null;
                }

                var putResult = PutAsync(flightDetails, "/api/v1/flights/" + flightDetails.FlightId).Result;

                if (putResult.IsSuccessStatusCode == false)
                {
                    Assert.IsTrue(putResult.IsSuccessStatusCode,
                                  string.Format("Error: {0}, Error-Message: {1}, while updating FlightDetails {2}",
                                                putResult.StatusCode, putResult.Content.ReadAsStringAsync().Result,
                                                flightDetails));
                }

                //Assert.AreNotEqual(aircraftDetails.Comment, original.Comment);
            }
        }
        #endregion GliderFlights

        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void FlightValidationAndLockDetailsWebApiTest()
        {
            InsertAndUpdateSchoolFlightDetailsWebApiTest();
            using (var context = DataAccessService.CreateDbContext())
            {
                var notValidatedFlights = context.Flights.Where(q => q.ValidatedOn == null);
                Assert.IsTrue(notValidatedFlights.Any());

                var validateFlights = GetAsync("/api/v1/flights/validate").Result;

                var notValidatedFlights2 = context.Flights.Where(q => q.ValidatedOn == null);
                Assert.IsFalse(notValidatedFlights2.Any());

                var lockedFlights = context.Flights.Where(q => q.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked);
                Assert.IsFalse(lockedFlights.Any());

                var lockFlights = GetAsync("/api/v1/flights/lock/force").Result;

                var lockedFlights2 = context.Flights.Where(q => q.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked);
                Assert.IsTrue(lockedFlights2.Any());
            }
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/flights"; }
        }
    }
}
