using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Comparer;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Reporting;
using FLS.Data.WebApi.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        [TestCategory("Service")]
        public void TakeOffTestForOnePreparedFlight()
        {
            var location = GetFirstLocation();

            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.Allowed",
                SettingValue = JsonConvert.SerializeObject(true)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var interestedLocationList = new List<string>()
            {
                location.IcaoCode
            };

            settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.InterestedLocations",
                SettingValue = JsonConvert.SerializeObject(interestedLocationList)
            };
            
            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var flightDetails = CreateGliderFlightDetails(CurrentIdentityUser.ClubId);
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;
            flightDetails.GliderFlightDetailsData.StartDateTime = null;
            flightDetails.GliderFlightDetailsData.StartLocationId = location.LocationId;
            FlightService.InsertFlightDetails(flightDetails);

            var aircraft = AircraftService.GetAircraft(flightDetails.GliderFlightDetailsData.AircraftId);

            Assert.IsTrue(flightDetails.FlightId.IsValid());
            Assert.IsFalse(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartLocationId.HasValue);

            var takeOffDetails = new TakeOffDetails();

            takeOffDetails.Immatriculation = aircraft.Immatriculation;
            takeOffDetails.TakeOffLocationIcaoCode = location.IcaoCode;
            takeOffDetails.TakeOffTimeUtc = DateTime.UtcNow;

            var updatedFlights = FlightService.TakeOff(takeOffDetails);

            Assert.IsTrue(updatedFlights.Any());

            foreach (var updatedFlight in updatedFlights)
            {
                var updatedFlightDetails = FlightService.GetFlightDetails(updatedFlight.FlightId);

                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
                Assert.IsTrue(updatedFlightDetails.FlightDate.HasValue);
                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartLocationId.HasValue);
                Assert.AreEqual((int)FlightAirState.Started, updatedFlightDetails.GliderFlightDetailsData.AirStateId);
                Assert.AreEqual(takeOffDetails.TakeOffTimeUtc.Date, updatedFlightDetails.FlightDate.Value.Date);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void TakeOffTestForReservationFlight()
        {
            var location = GetLocation("LSZK");

            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.Allowed",
                SettingValue = JsonConvert.SerializeObject(true)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var interestedLocationList = new List<string>()
            {
                location.IcaoCode
            };

            settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.InterestedLocations",
                SettingValue = JsonConvert.SerializeObject(interestedLocationList)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var interestedImmatriculations = new List<string>()
            {
                "HB-3256",
                "HB-1841",
                "HB-3300",
                "HB-1824",
                "HB-3407"
            };

            settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.InterestedImmatriculations",
                SettingValue = JsonConvert.SerializeObject(interestedImmatriculations)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var aircraft = AircraftService.GetAircraft(interestedImmatriculations[0]);
            Assert.IsNotNull(aircraft);

            DeleteFlights(FlightAirState.New);
            DeleteFlights(FlightAirState.FlightPlanOpen);
            DeleteReservations(DateTime.Today);

            var reservation = new AircraftReservationDetails();
            reservation.AircraftId = aircraft.AircraftId;
            reservation.Start = DateTime.UtcNow.Date;
            reservation.End = DateTime.UtcNow.Date;
            reservation.Remarks = "Test for Takeoff";
            reservation.IsAllDayReservation = true;
            reservation.PilotPersonId = GetFirstPerson().PersonId;
            reservation.LocationId = location.LocationId;
            reservation.SecondCrewPersonId = GetFirstPerson().PersonId;
            reservation.ReservationTypeId = GetFlightType("60").FlightTypeId;

            AircraftReservationService.InsertAircraftReservationDetails(reservation);

            var takeOffDetails = new TakeOffDetails();

            takeOffDetails.Immatriculation = aircraft.Immatriculation;
            takeOffDetails.TakeOffLocationIcaoCode = location.IcaoCode;
            takeOffDetails.TakeOffTimeUtc = DateTime.UtcNow;

            var updatedFlights = FlightService.TakeOff(takeOffDetails);

            Assert.IsTrue(updatedFlights.Any());

            foreach (var updatedFlight in updatedFlights)
            {
                var updatedFlightDetails = FlightService.GetFlightDetails(updatedFlight.FlightId);

                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
                Assert.IsTrue(updatedFlightDetails.FlightDate.HasValue);
                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartLocationId.HasValue);
                Assert.AreEqual((int)FlightAirState.Started, updatedFlightDetails.GliderFlightDetailsData.AirStateId);
                Assert.AreEqual(takeOffDetails.TakeOffTimeUtc.Date, updatedFlightDetails.FlightDate.Value.Date);
            }
        }

        [TestMethod]
        [TestCategory("Service")]
        public void LandingTestForOneFlight()
        {
            #region Create flight and Start date time
            var location = GetFirstLocation();

            var settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.Allowed",
                SettingValue = JsonConvert.SerializeObject(true)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var interestedLocationList = new List<string>()
            {
                location.IcaoCode
            };

            settingDetails = new SettingDetails()
            {
                ClubId = CurrentIdentityUser.ClubId,
                SettingKey = "FLSOgnAnalyser.InterestedLocations",
                SettingValue = JsonConvert.SerializeObject(interestedLocationList)
            };

            SettingService.InsertOrUpdateSettingDetails(settingDetails);

            var flightDetails = CreateGliderFlightDetails(CurrentIdentityUser.ClubId);
            flightDetails.GliderFlightDetailsData.CoPilotPersonId = GetDifferentPerson(GetFirstPerson().PersonId).PersonId;
            flightDetails.GliderFlightDetailsData.StartDateTime = null;
            flightDetails.GliderFlightDetailsData.StartLocationId = location.LocationId;
            FlightService.InsertFlightDetails(flightDetails);

            var aircraft = AircraftService.GetAircraft(flightDetails.GliderFlightDetailsData.AircraftId);

            Assert.IsTrue(flightDetails.FlightId.IsValid());
            Assert.IsFalse(flightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
            Assert.IsTrue(flightDetails.GliderFlightDetailsData.StartLocationId.HasValue);

            var takeOffDetails = new TakeOffDetails();

            takeOffDetails.Immatriculation = aircraft.Immatriculation;
            takeOffDetails.TakeOffLocationIcaoCode = location.IcaoCode;
            takeOffDetails.TakeOffTimeUtc = DateTime.UtcNow;

            var updatedFlights = FlightService.TakeOff(takeOffDetails);

            Assert.IsTrue(updatedFlights.Any());

            foreach (var updatedFlight in updatedFlights)
            {
                var updatedFlightDetails = FlightService.GetFlightDetails(updatedFlight.FlightId);

                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
                Assert.IsTrue(updatedFlightDetails.FlightDate.HasValue);
                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartLocationId.HasValue);
                Assert.AreEqual((int)FlightAirState.Started, updatedFlightDetails.GliderFlightDetailsData.AirStateId);
                Assert.AreEqual(takeOffDetails.TakeOffTimeUtc.Date, updatedFlightDetails.FlightDate.Value.Date);

                Assert.IsFalse(updatedFlightDetails.GliderFlightDetailsData.LdgDateTime.HasValue);
            }
            #endregion 

            var landingDetails = new LandingDetails();

            landingDetails.Immatriculation = aircraft.Immatriculation;
            landingDetails.LandingLocationIcaoCode = location.IcaoCode;
            landingDetails.LandingTimeUtc = DateTime.UtcNow;

            updatedFlights = FlightService.Landing(landingDetails);

            Assert.IsTrue(updatedFlights.Any());

            foreach (var updatedFlight in updatedFlights)
            {
                var updatedFlightDetails = FlightService.GetFlightDetails(updatedFlight.FlightId);

                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartDateTime.HasValue);
                Assert.IsTrue(updatedFlightDetails.FlightDate.HasValue);
                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.StartLocationId.HasValue);
                Assert.AreEqual(takeOffDetails.TakeOffTimeUtc.Date, updatedFlightDetails.FlightDate.Value.Date);

                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.LdgDateTime.HasValue);
                Assert.IsTrue(updatedFlightDetails.GliderFlightDetailsData.LdgLocationId.HasValue);
                Assert.AreEqual((int)FlightAirState.Landed, updatedFlightDetails.GliderFlightDetailsData.AirStateId);
            }
        }
    }
}
