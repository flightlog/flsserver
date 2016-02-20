using System;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class AircraftServiceTest : BaseServiceTest
    {
        private AircraftService _aircraftService;
        private AircraftHelper _aircraftHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _aircraftService = UnityContainer.Resolve<AircraftService>();
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();
        }
        
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void AircraftServiceTestInitialize(TestContext testContext)
        {
        }
        
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftsTest()
        {
            var entities = _aircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftDetailsTest()
        {
            var entities = _aircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());

            foreach (var entity in entities)
            {
                var aircraft = _aircraftService.GetAircraftDetails(entity.Id);
                Assert.IsNotNull(aircraft);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetAircraftOverviewsTest()
        {
            var entities = _aircraftService.GetAircraftOverviews();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertGliderAircraftDetailsWithoutAircraftStateTest()
        {
            var aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(1);

            _aircraftService.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = _aircraftService.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void InsertGliderAircraftDetailsWithAircraftStateTest()
        {
            var aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(2);

            var aircraftStates = _aircraftService.GetAircraftStates();
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

            _aircraftService.InsertAircraftDetails(aircraftDetails);
            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = _aircraftService.GetAircraft(aircraftDetails.AircraftId);
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
            var aircraftDetails = _aircraftHelper.CreateTowingAircraftDetails();

            _aircraftService.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", aircraftDetails));

            var loadedAircraft = _aircraftService.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void UpateAircraftDetailsCommentOnlyTest()
        {
            var aircrafts = _aircraftService.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = _aircraftService.GetAircraftDetails(aircraft.AircraftId);
            var original = _aircraftService.GetAircraftDetails(aircraft.AircraftId);
            aircraftDetails.Comment = "Test-Ticks" + DateTime.Now.Ticks;

            _aircraftService.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = _aircraftService.GetAircraftDetails(aircraftDetails.AircraftId);
            Assert.IsTrue(aircraftDetails.AircraftId == original.AircraftId);
            Assert.IsTrue(aircraftDetails.Comment != original.Comment);
            Assert.IsTrue(aircraftDetails.Immatriculation == original.Immatriculation);
            Assert.IsTrue(aircraftDetails.FLARMId == original.FLARMId);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void UpateAircraftDetailsAircraftStateTest()
        {
            var aircrafts = _aircraftService.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = _aircraftService.GetAircraftDetails(aircraft.AircraftId);
            var original = _aircraftService.GetAircraftDetails(aircraft.AircraftId);

            var aircraftStates = _aircraftService.GetAircraftStates();
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

            _aircraftService.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = _aircraftService.GetAircraftDetails(aircraftDetails.AircraftId);
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
            var entities = _aircraftService.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }
    }
}
