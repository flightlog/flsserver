using System;
using System.Diagnostics;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace FLS.Server.Service.Tests
{
    [TestClass]
    public class AircraftServiceTest : BaseServiceTest
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private Logger Logger
        {
            get { return _logger; }
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
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void AircraftServiceTestInitialize()
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
        public void GetAircraftsTest()
        {
            var service = new AircraftService();
            var entities = service.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        public void GetAircraftDetailsTest()
        {
            var service = new AircraftService();
            var entities = service.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());

            foreach (var entity in entities)
            {
                var aircraft = service.GetAircraftDetails(entity.Id);
                Assert.IsNotNull(aircraft);
            }
        }

        [TestMethod]
        public void GetAircraftOverviewsTest()
        {
            var service = new AircraftService();
            var entities = service.GetAircraftOverviews();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }

        [TestMethod]
        public void InsertGliderAircraftDetailsWithoutAircraftStateTest()
        {
            var service = new AircraftService();

            var aircraftDetails = FLSDataHelper.Instance.CreateGliderAircraftDetails(1);

            service.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.AircraftId != Guid.Empty);

            var loadedAircraft = service.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        public void InsertGliderAircraftDetailsWithAircraftStateTest()
        {
            var service = new AircraftService();

            var aircraftDetails = FLSDataHelper.Instance.CreateGliderAircraftDetails(2);

            var aircraftStates = service.GetAircraftStates();
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

            service.InsertAircraftDetails(aircraftDetails);
            Assert.IsTrue(aircraftDetails.AircraftId != Guid.Empty);

            var loadedAircraft = service.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft); 
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
            Assert.IsNotNull(loadedAircraft.CurrentAircraftAircraftState);
            Assert.IsNotNull(aircraftDetails);
            Assert.IsNotNull(aircraftDetails.AircraftStateData);
        }

        [TestMethod]
        public void InsertTowingAircraftDetailsWithoutAircraftStateTest()
        {
            var service = new AircraftService();

            var aircraftDetails = FLSDataHelper.Instance.CreateTowingAircraftDetails();

            service.InsertAircraftDetails(aircraftDetails);

            Assert.IsTrue(aircraftDetails.AircraftId != Guid.Empty);

            var loadedAircraft = service.GetAircraft(aircraftDetails.AircraftId);
            Assert.IsNotNull(loadedAircraft);
            Assert.IsTrue(aircraftDetails.AircraftId == loadedAircraft.Id);
            Assert.IsTrue(aircraftDetails.Immatriculation == loadedAircraft.Immatriculation);
        }

        [TestMethod]
        public void UpateAircraftDetailsCommentOnlyTest()
        {
            var service = new AircraftService();
            var aircrafts = service.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = service.GetAircraftDetails(aircraft.AircraftId);
            var original = service.GetAircraftDetails(aircraft.AircraftId);
            aircraftDetails.Comment = "Test-Ticks" + DateTime.Now.Ticks;

            service.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = service.GetAircraftDetails(aircraftDetails.AircraftId);
            Assert.IsTrue(aircraftDetails.AircraftId == original.AircraftId);
            Assert.IsTrue(aircraftDetails.Comment != original.Comment);
            Assert.IsTrue(aircraftDetails.Immatriculation == original.Immatriculation);
            Assert.IsTrue(aircraftDetails.FLARMId == original.FLARMId);
        }

        [TestMethod]
        public void UpateAircraftDetailsAircraftStateTest()
        {
            var service = new AircraftService();
            var aircrafts = service.GetAircrafts((int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);

            Assert.IsNotNull(aircrafts);
            Assert.IsTrue(aircrafts.Any());
            var aircraft = aircrafts.FirstOrDefault();

            Assert.IsNotNull(aircraft);
            var aircraftDetails = service.GetAircraftDetails(aircraft.AircraftId);
            var original = service.GetAircraftDetails(aircraft.AircraftId);

            var aircraftStates = service.GetAircraftStates();
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

            service.UpdateAircraftDetails(aircraftDetails);
            aircraftDetails = service.GetAircraftDetails(aircraftDetails.AircraftId);
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
        public void DeleteAircraftTest()
        {
            var service = new AircraftService();
            var entities = service.GetAircrafts();
            Assert.IsNotNull(entities);
            Assert.IsTrue(entities.Any());
        }
    }
}
