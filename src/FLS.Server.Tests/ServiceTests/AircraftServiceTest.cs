using System;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class AircraftServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftsTest()
        {
            var entities = AircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftDetailsTest()
        {
            var entities = AircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());

            foreach (var entity in entities)
            {
                var aircraft = AircraftService.GetAircraftDetails(entity.Id);
                Assert.IsNotNull(aircraft);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftOverviewsTest()
        {
            var entities = AircraftService.GetAircraftOverviews();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertGliderAircraftDetailsWithoutAircraftStateTest()
        {
            var aircraftDetails = CreateGliderAircraftDetails(1);

            AircraftService.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = AircraftService.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertGliderAircraftDetailsWithAircraftStateTest()
        {
            var aircraftDetails = CreateGliderAircraftDetails(2);

            var aircraftStates = AircraftService.GetAircraftStates();
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

            AircraftService.InsertAircraftDetails(aircraftDetails);
            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = AircraftService.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft); 
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
            Assert.IsNotNull(loadedAircraft.CurrentAircraftAircraftState);
            Assert.IsNotNull(aircraftDetails);
            Assert.IsNotNull(aircraftDetails.AircraftStateData);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertTowingAircraftDetailsWithoutAircraftStateTest()
        {
            var aircraftDetails = CreateTowingAircraftDetails();

            AircraftService.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = AircraftService.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void UpateAircraftDetailsCommentOnlyTest()
        {
            var aircrafts = AircraftService.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = AircraftService.GetAircraftDetails(aircraft.AircraftId);
            var original = AircraftService.GetAircraftDetails(aircraft.AircraftId);
            aircraftDetails.Comment = "Test-Ticks" + DateTime.Now.Ticks;

            AircraftService.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = AircraftService.GetAircraftDetails(aircraftDetails.AircraftId);
            Assert.IsTrue(aircraftDetails.AircraftId == original.AircraftId);
            Assert.IsTrue(aircraftDetails.Comment != original.Comment);
            Assert.IsTrue(aircraftDetails.Immatriculation == original.Immatriculation);
            Assert.IsTrue(aircraftDetails.FLARMId == original.FLARMId);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void UpateAircraftDetailsAircraftStateTest()
        {
            var aircrafts = AircraftService.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = AircraftService.GetAircraftDetails(aircraft.AircraftId);
            var original = AircraftService.GetAircraftDetails(aircraft.AircraftId);

            var aircraftStates = AircraftService.GetAircraftStates();
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

            AircraftService.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = AircraftService.GetAircraftDetails(aircraftDetails.AircraftId);
            Assert.IsTrue(aircraftDetails.AircraftId == original.AircraftId);
            Assert.IsTrue(aircraftDetails.Comment == original.Comment);
            Assert.IsTrue(aircraftDetails.Immatriculation == original.Immatriculation);
            Assert.IsTrue(aircraftDetails.FLARMId == original.FLARMId);

            Assert.IsNotNull(aircraftDetails.AircraftStateData);

            if (original.AircraftStateData != null)
            {
                //TODO: Test history
                //Assert.IsTrue(aircraftDetails.AircraftStateData.ValidFrom == original.AircraftStateData.ValidTo);

                Assert.IsTrue(original.AircraftStateData.ValidFrom < aircraftDetails.AircraftStateData.ValidFrom);
            }

            Assert.IsTrue(aircraftDetails.AircraftStateData.ValidTo.HasValue == false);
            Assert.IsTrue(aircraftDetails.AircraftStateData.AircraftState == aircraftState.AircraftStateId);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void DeleteAircraftTest()
        {
            var entities = AircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }
    }
}
