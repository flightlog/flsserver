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
    public class FlightServiceTest
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
        public static void FlightServiceTestInitialize(TestContext testContext)
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
        public void FlightServiceTestInitialize()
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
        public void GetFlightsTest()
        {
            var service = new FlightService();
            var entities = service.GetFlightOverviews();
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
            }
        }

        [TestMethod]
        public void GetFlightDetailsTest()
        {
            var service = new FlightService();
            var entities = service.GetFlightOverviews();
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                var flight = service.GetFlightDetails(entity.Id);
                Console.WriteLine(flight);
            }
        }


    }
}
