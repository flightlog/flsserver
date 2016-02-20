using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class AircraftsControllerTest : BaseAuthenticatedTests
    {
        private AircraftHelper _aircraftHelper;
        
        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize: AircraftsControllerTest.TestInitialize()");
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetGliderAircraftsListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftListItem>>(Uri + "/gliders/listitems").Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<AircraftListItem>>(Uri + "/listitems/gliders").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetTowingAircraftsListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftListItem>>(Uri + "/towingaircrafts/listitems").Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<AircraftListItem>>(Uri + "/listitems/towingaircrafts").Result;
            Assert.IsTrue(response1.Any());

            Assert.AreEqual(response.Count(), response1.Count());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftsOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetGliderAircraftOverviewsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/gliders").Result;
            Assert.IsTrue(response.Any()); 
            
            var response1 = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/gliders/overview").Result;
            Assert.IsTrue(response1.Any()); 
            
            var response2 = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/overview/gliders").Result;
            Assert.IsTrue(response2.Any());

            Assert.AreEqual(response.Count(), response1.Count());
            Assert.AreEqual(response.Count(), response2.Count());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetTowingAircraftOverviewsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/towingaircrafts").Result;
            Assert.IsTrue(response.Any());

            var response1 = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/towingaircrafts/overview").Result;
            Assert.IsTrue(response1.Any());

            var response2 = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/overview/towingaircrafts").Result;
            Assert.IsTrue(response2.Any());

            Assert.AreEqual(response.Count(), response1.Count());
            Assert.AreEqual(response.Count(), response2.Count());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftOverviewsByAircraftTypeWebApiTest()
        {
            var typesToCheck = new List<int>();
            typesToCheck.Add((int) AircraftType.Glider);
            typesToCheck.Add((int) AircraftType.MotorAircraft);

            foreach (var aircraftType in typesToCheck)
            {
                var response = GetAsync<IEnumerable<AircraftOverview>>(string.Format("/api/v1/aircrafts/type/{0}", aircraftType)).Result;
                Assert.IsTrue(response.Any());

                var response1 = GetAsync<IEnumerable<AircraftOverview>>(string.Format("/api/v1/aircrafts/overview/type/{0}", aircraftType)).Result;
                Assert.IsTrue(response1.Any());

                var response2 = GetAsync<IEnumerable<AircraftOverview>>(string.Format("/api/v1/aircrafts/overview/aircrafttype/{0}", aircraftType)).Result;
                Assert.IsTrue(response2.Any());

                Assert.AreEqual(response.Count(), response1.Count());
                Assert.AreEqual(response.Count(), response2.Count());

                if (aircraftType == (int) AircraftType.Glider)
                {
                    var response3 = GetAsync<IEnumerable<AircraftOverview>>(Uri + "/gliders").Result;
                    Assert.IsTrue(response3.Any());
                    Assert.IsTrue(response3.Count() >= response.Count()); //glider includes also motorgliders --> >= and not ==
                }
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftsDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;
            
            Assert.IsTrue(response.Any());

            var id = response.First().AircraftId;

            var result = GetAsync<AircraftDetails>("/api/v1/aircrafts/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAircraftsDetailsByImmatriculationWebApiTest()
        {
            var response = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;

            Assert.IsTrue(response.Any());

            var immatriculation = response.First().Immatriculation;

            var result = GetAsync<AircraftDetails>("/api/v1/aircrafts/immatriculation/" + immatriculation).Result;

            Assert.AreEqual(immatriculation, result.Immatriculation);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertAircraftWebApiTest()
        {
            var aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(1);

            var response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<AircraftDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var firstAircraft = aircraftDetails;

            aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(2);

            if (aircraftDetails.Immatriculation == firstAircraft.Immatriculation)
            {
                aircraftDetails.Immatriculation = aircraftDetails.Immatriculation + "2";
            }

            var aircraftStates = GetAsync<IEnumerable<AircraftStateListItem>>("/api/v1/aircraftstates").Result;
            Assert.IsTrue(aircraftStates.Any());
            var aircraftState = aircraftStates.FirstOrDefault();

            Assert.IsNotNull(aircraftState);
            var newAircraftState = new AircraftStateData
            {
                AircraftId = aircraftDetails.AircraftId,
                AircraftState = aircraftState.AircraftStateId,
                NoticedByPersonId = null,
                Remarks = "Test-AircraftAircraftState",
                ValidFrom = DateTime.Now
            };

            aircraftDetails.AircraftStateData = newAircraftState;

            response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responseDetails = ConvertToModel<AircraftDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var details = GetModelFromResponse<AircraftDetails>(response);

            Assert.IsNotNull(aircraftDetails);
            Assert.IsNotNull(aircraftDetails.AircraftStateData);

            Assert.AreNotEqual(details.AircraftId, Guid.Empty);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertAircraftWithAircraftStateWebApiTest()
        {
            var aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(1);
            
            var aircraftStates = GetAsync<IEnumerable<AircraftStateListItem>>("/api/v1/aircraftstates").Result;
            Assert.IsTrue(aircraftStates.Any());
            var aircraftState = aircraftStates.FirstOrDefault();

            Assert.IsNotNull(aircraftState);
            var newAircraftState = new AircraftStateData
            {
                AircraftId = aircraftDetails.AircraftId,
                AircraftState = aircraftState.AircraftStateId,
                NoticedByPersonId = null,
                Remarks = "Test-AircraftAircraftState",
                ValidFrom = DateTime.Now
            };

            aircraftDetails.AircraftStateData = newAircraftState;

            var response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<AircraftDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var details = GetModelFromResponse<AircraftDetails>(response);

            Assert.IsNotNull(aircraftDetails);
            Assert.IsNotNull(aircraftDetails.AircraftStateData);

            Assert.AreNotEqual(details.AircraftId, Guid.Empty);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateAircraftWebApiTest()
        {
            var aircrafts = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;
            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = GetAsync<AircraftDetails>("/api/v1/aircrafts/" + aircraft.AircraftId).Result;

            var original = GetAsync<AircraftDetails>("/api/v1/aircrafts/" + aircraft.AircraftId).Result;
            aircraftDetails.Comment = "Test" + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(aircraftDetails, "/api/v1/aircrafts/" + aircraftDetails.AircraftId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            var aircraftStates = GetAsync<IEnumerable<AircraftStateListItem>>("/api/v1/aircraftstates").Result;
            Assert.IsTrue(aircraftStates.Any());
            var aircraftState = aircraftStates.FirstOrDefault();
            Assert.IsNotNull(aircraftState);

            if (aircraftDetails.AircraftStateData != null)
            {
                aircraftDetails.AircraftStateData.ValidFrom = DateTime.Now;
                aircraftDetails.AircraftStateData.AircraftState = aircraftState.AircraftStateId;
                aircraftDetails.AircraftStateData.Remarks = "New state on " + DateTime.Now;
            }
            else
            {
                var newAircraftState = new AircraftStateData
                {
                    AircraftId = aircraftDetails.AircraftId,
                    AircraftState = aircraftState.AircraftStateId,
                    NoticedByPersonId = null,
                    Remarks = "Test-AircraftAircraftState",
                    ValidFrom = DateTime.Now
                };

                aircraftDetails.AircraftStateData = newAircraftState;
            }

            putResult = PutAsync(aircraftDetails, "/api/v1/aircrafts/" + aircraftDetails.AircraftId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            Assert.AreNotEqual(aircraftDetails.Comment, original.Comment);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeleteAircraftWebApiTest()
        {
            var aircrafts = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;
            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);

            var delResult = DeleteAsync("/api/v1/aircrafts/" + aircraft.AircraftId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);

            var aircraftsNew = GetAsync<IEnumerable<AircraftOverview>>(Uri).Result;

            Assert.IsTrue(aircraftsNew.Count() < aircrafts.Count());
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircrafts"; }
        }
    }
}
