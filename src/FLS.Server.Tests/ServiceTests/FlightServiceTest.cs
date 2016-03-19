using System;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class FlightServiceTest : BaseTest
    {

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
            var entities = FlightService.GetGliderFlightOverviews();
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
            var entities = FlightService.GetGliderFlightOverviews();
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                var flight = FlightService.GetFlightDetails(entity.Id);
                Console.WriteLine(flight);
            }
        }


    }
}
