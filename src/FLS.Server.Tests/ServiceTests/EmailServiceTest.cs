using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FLS.Common.Extensions;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class EmailServiceTest : BaseServiceTest
    {
        private UserAccountEmailBuildService _passwordEmailService;
        private PlanningDayEmailBuildService _planningDayEmailService;
        private AircraftReservationService _aircraftReservationService;
        private AircraftReportEmailBuildService _aircraftReportEmailService;
        private FlightInformationEmailBuildService _flightInformationEmailService;
        private FlightService _flightService;
        private PlanningDayHelper _planningDayHelper;
        private TestContext _testContextInstance;
        private UserService _userService;
        private FlightHelper _flightHelper;
        private AircraftReservationHelper _aircraftReservationHelper;
        private IdentityService _identityService;
        private AircraftService _aircraftService;
        private UserHelper _userHelper;
        private AircraftHelper _aircraftHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _passwordEmailService = UnityContainer.Resolve<UserAccountEmailBuildService>();
            _planningDayEmailService = UnityContainer.Resolve<PlanningDayEmailBuildService>();
            _aircraftReservationService = UnityContainer.Resolve<AircraftReservationService>();
            _planningDayHelper = UnityContainer.Resolve<PlanningDayHelper>();
            _flightInformationEmailService = UnityContainer.Resolve<FlightInformationEmailBuildService>();
            _flightService = UnityContainer.Resolve<FlightService>();
            _aircraftService = UnityContainer.Resolve<AircraftService>();
            _aircraftReportEmailService = UnityContainer.Resolve<AircraftReportEmailBuildService>();
            _flightHelper = UnityContainer.Resolve<FlightHelper>();
            _aircraftReservationHelper = UnityContainer.Resolve<AircraftReservationHelper>();
            _userService = UnityContainer.Resolve<UserService>();
            _userHelper = UnityContainer.Resolve<UserHelper>();
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();

            var user = _userService.GetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
            Assert.IsNotNull(user);
            _identityService = UnityContainer.Resolve<IdentityService>();
            _identityService.SetUser(user);

            _flightHelper.CreateFlightsForInvoicingTests(user.ClubId);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }
        
        [TestMethod]
        [TestCategory("Service")]
        public void EmailAddressFormatTest()
        {
            var message = new MailMessage();
            string to = "test@gmail.com;john@rediff.com,prashant@mail.com pschuler@test.ch, test@test.ch";
            message.To.Add(to.FormatMultipleEmailAddresses());

            Assert.IsNotNull(message);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void CreatePlanningDayTakesPlaceEmailTest()
        {
            var recipients = "johnsmith@corporate.com";
            var planningDay = _planningDayHelper.GetFirstPlanningDayOverview();
            var reservations = _aircraftReservationService.GetAircraftReservationsByPlanningDayId(planningDay.PlanningDayId);
            var message = _planningDayEmailService.CreatePlanningDayTakesPlaceEmail(planningDay, reservations, recipients, CurrentIdentityUser.ClubId);

            Assert.IsNotNull(message);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void CreatePlanningDayTakesPlaceNoFlightOperatorEmailTest()
        {
            var planningDay = new PlanningDayOverview();
            planningDay.Day = new DateTime(2015, 5, 15);
            planningDay.LocationName = "Speck";
            planningDay.NumberOfAircraftReservations = 0;
            planningDay.TowingPilotName = "Peter Reichert";
            planningDay.PlanningDayId = Guid.NewGuid();

            var reservation = new AircraftReservationOverview()
                {
                    AircraftReservationId = Guid.NewGuid(),
                    Immatriculation = "HB-3256",
                    PilotName = "Roman Schlegel",
                    IsAllDayReservation = true,
                    ReservationTypeName = "Charterflug",
                    LocationName = "Speck",
                    Start = planningDay.Day,
                    End = planningDay.Day,
                    Remarks = "Ich will ohne Instruktor fliegen"
                };

            var reservations = new List<AircraftReservationOverview>();
            reservations.Add(reservation);

            var recipients = "johnsmith@corporate.com";
            var message = _planningDayEmailService.CreatePlanningDayTakesPlaceEmail(planningDay, reservations, recipients, CurrentIdentityUser.ClubId);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Body.Contains("Peter Reichert"));
            Assert.IsTrue(message.Body.Contains("Speck"));
            Assert.IsTrue(message.Body.Contains("15.05.2015"));
            Assert.IsFalse(message.Body.Contains("$"));
        }

        [TestMethod]
        [TestCategory("Service")]
        public void CreatePlanningDayNoReservationsEmailTest()
        {
            var recipients = "johnsmith@corporate.com";
            var planningDay = _planningDayHelper.GetFirstPlanningDayOverview();
            var message = _planningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay, recipients, CurrentIdentityUser.ClubId);

            Assert.IsNotNull(message);
            Assert.IsFalse(message.Body.Contains("$"));
        }

        [TestMethod]
        [TestCategory("Service")]
        public void CreatePlanningDayNoReservationsNoFlightOperatorEmailTest()
        {
            var planningDay = new PlanningDayOverview();
            planningDay.Day = new DateTime(2015, 5, 15);
            planningDay.LocationName = "Speck";
            planningDay.NumberOfAircraftReservations = 0;
            planningDay.TowingPilotName = "Peter Reichert";
            planningDay.PlanningDayId = Guid.NewGuid();
            planningDay.Remarks = "Test-Planningday";

            var recipients = "johnsmith@corporate.com";
            var message = _planningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay, recipients, CurrentIdentityUser.ClubId);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Body.Contains("Peter Reichert"));
            Assert.IsTrue(message.Body.Contains("Test-Planningday"));
            Assert.IsTrue(message.Body.Contains("Speck"));
            Assert.IsTrue(message.Body.Contains("15.05.2015"));
            Assert.IsFalse(message.Body.Contains("$"));
        }

        [TestMethod]
        [TestCategory("Service")]
        public void CreateAircraftFlightReportEmailTest()
        {
            //.CreateFlightsForInvoicingTests();
            var startDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var immatriculations = new List<string>();
            var filterCriteria = new AircraftFlightReportFilterCriteria();
            filterCriteria.StatisticStartDateTime = startDate;
            filterCriteria.StatisticEndDateTime = endDate;

            var immatriculation1 = _aircraftHelper.GetFirstOneSeatGlider().Immatriculation;
            immatriculations.Add(immatriculation1);
            var immatriculation2 = _aircraftHelper.GetFirstDoubleSeatGlider().Immatriculation;
            immatriculations.Add(immatriculation2);

            foreach (var aircraftImmatriculation in immatriculations)
            {
                var aircraft = _aircraftService.GetAircraft(aircraftImmatriculation);

                if (aircraft != null)
                {
                    filterCriteria.AircraftIds.Add(aircraft.AircraftId);
                }
            }

            var aircraftFlightReport = _flightService.GetAircraftFlightReport(filterCriteria);
            Assert.IsNotNull(aircraftFlightReport);
            Assert.IsTrue(aircraftFlightReport.AircraftFlightReportData.Any());

            var recipients = "johnsmith@corporate.com";

            var message = _aircraftReportEmailService.CreateAircraftStatisticInformationEmail(aircraftFlightReport, recipients);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Body.Contains(immatriculation1));
            Assert.IsTrue(message.Body.Contains(immatriculation2));
            Assert.IsFalse(message.Body.Contains("$"));
        }
    }
}
