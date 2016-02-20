using System;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class FlightServiceTest : BaseServiceTest
    {
        private FlightService _flightService;

        [TestInitialize]
        public void TestInitialize()
        {
            _flightService = UnityContainer.Resolve<FlightService>();
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
        public static void FlightServiceTestInitialize(TestContext testContext)
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

        //[TestMethod]
        //public void CreateClubTest()
        //{
        //    FLS.Server.TestInfrastructure.DatabasePreparer.Instance.PrepareDatabaseForTests();
        //    var club = FLS.Server.TestInfrastructure.ClubHelper.CreateClub();
        //    Assert.IsNotNull(club);
        //}

        [TestMethod]
        [TestCategory("Service")]
        public void GetFlightsTest()
        {
            var entities = _flightService.GetGliderFlightOverviews();
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetFlightDetailsTest()
        {
            var entities = _flightService.GetGliderFlightOverviews();
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                var flight = _flightService.GetFlightDetails(entity.Id);
                Console.WriteLine(flight);
            }
        }


    }
}
