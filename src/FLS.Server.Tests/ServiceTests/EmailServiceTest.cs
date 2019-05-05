using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FLS.Common.Extensions;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using FLS.Data.WebApi.Reporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class EmailServiceTest : BaseTest
    {
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
            var planningDay = GetFirstPlanningDayOverview();
            var reservations = AircraftReservationService.GetAircraftReservationsByPlanningDayId(planningDay.PlanningDayId);
            var message = PlanningDayEmailService.CreatePlanningDayTakesPlaceEmail(planningDay, reservations, recipients, CurrentIdentityUser.ClubId);

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
            var message = PlanningDayEmailService.CreatePlanningDayTakesPlaceEmail(planningDay, reservations, recipients, CurrentIdentityUser.ClubId);

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
            var planningDay = GetFirstPlanningDayOverview();
            var message = PlanningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay, recipients, CurrentIdentityUser.ClubId);

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
            var message = PlanningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay, recipients, CurrentIdentityUser.ClubId);

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

            var immatriculation1 = GetFirstOneSeatGlider().Immatriculation;
            immatriculations.Add(immatriculation1);
            var immatriculation2 = GetFirstDoubleSeatGlider().Immatriculation;
            immatriculations.Add(immatriculation2);

            foreach (var aircraftImmatriculation in immatriculations)
            {
                var aircraft = AircraftService.GetAircraft(aircraftImmatriculation);

                if (aircraft != null)
                {
                    filterCriteria.AircraftIds.Add(aircraft.AircraftId);
                }
            }

            var aircraftFlightReport = FlightService.GetAircraftFlightReport(filterCriteria);
            Assert.IsNotNull(aircraftFlightReport);
            Assert.IsTrue(aircraftFlightReport.AircraftFlightReportData.Any());

            var recipients = "johnsmith@corporate.com";

            var message = AircraftReportEmailService.CreateAircraftStatisticInformationEmail(aircraftFlightReport, recipients);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Body.Contains(immatriculation1));
            Assert.IsTrue(message.Body.Contains(immatriculation2));
            Assert.IsFalse(message.Body.Contains("$"));
        }
    }
}
