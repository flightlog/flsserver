﻿using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Comparer;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        [TestCategory("Service")]
        public void FlightExchangeServiceTest()
        {
            var fromDate = new DateTime(2000,1,1);
            var flights = FlightService.GetFlightsModifiedSince(fromDate);
            Assert.IsTrue(flights.Any());
            Logger.Debug($"Number of flights to export {flights.Count} since {fromDate}");
        }

        [TestMethod]
        [TestCategory("Service")]
        public void GetCompareOrderedFlightsTest()
        {
            var filter = new PageableSearchFilter<GliderFlightOverviewSearchFilter>();

            var entities = FlightService.GetPagedGliderFlightOverview(0, 10000, filter);
            Assert.IsNotNull(entities);
            Logger.Debug($"Number of flights to {entities.Items.Count}");

            filter = new PageableSearchFilter<GliderFlightOverviewSearchFilter>()
            {
                Sorting = new Dictionary<string, string>()
            };

            filter.Sorting.Add("PilotName", "asc");

            var entitiesNew = FlightService.GetPagedGliderFlightOverview(0, 10000, filter);
            Assert.IsNotNull(entitiesNew);
            Logger.Debug($"Number of flightsNew to {entitiesNew.Items.Count} and total available in DB: {entitiesNew.TotalRows}");
            Assert.AreEqual(entities.Items.Count, entitiesNew.Items.Count, "Number of flights does not match between the two service methods");

            var ignorePropertyList = new string [] { "GliderFlightDurationInSeconds", "TowFlightDurationInSeconds" };

            foreach (var entity in entities.Items)
            {
                var flight = entitiesNew.Items.FirstOrDefault(x => x.FlightId == entity.FlightId);

                var compareResult = ObjectComparer.AreObjectsEqual(entity, flight, true, ignorePropertyList);
                Assert.IsTrue(compareResult, "Flight property values does not match");
            }

        }

        [TestMethod]
        [TestCategory("Service")]
        public void AircraftStatisticReportServiceTest()
        {
            var startDate = new DateTime(2017, 5, 1);
            var endDate = startDate.AddMonths(1).AddTicks(-1);

            var filterCriteria = new AircraftFlightReportFilterCriteria();
            filterCriteria.StatisticStartDateTime = startDate;
            filterCriteria.StatisticEndDateTime = endDate;
            filterCriteria.AircraftIds.Add(Guid.Parse("CA77AA0D-ADFF-4809-960A-D9E42E14CD44")); //HB-2310 Dimona
            var report = FlightService.GetAircraftFlightReport(filterCriteria);
            Assert.IsNotNull(report);
            
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ValidateFlightsTest()
        {
            var flightDetails = CreateGliderFlightDetails(CurrentIdentityUser.ClubId);
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;

            flightDetails.GliderFlightDetailsData.StartDateTime = null;

            var towFlightDetails = CreateTowFlightDetailsData(CurrentIdentityUser.ClubId, "HB-XXX", DateTime.Now);
            towFlightDetails.PilotPersonId = Guid.Empty;
            flightDetails.TowFlightDetailsData = towFlightDetails;
            FlightService.InsertFlightDetails(flightDetails);

            Assert.IsTrue(flightDetails.FlightId.IsValid());

            FlightService.ValidateFlight(flightDetails.FlightId);

            var errors = FlightService.GetFlight(flightDetails.FlightId);

            var overview = FlightService.GetGliderFlightOverviews();

        }

    }
}
