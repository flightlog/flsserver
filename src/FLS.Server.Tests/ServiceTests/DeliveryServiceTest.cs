using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.Testing;
using FLS.Server.Data.Extensions;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class DeliveryServiceTest : BaseTest
    {
        //http://stackoverflow.com/questions/24012253/datadriven-mstests-csv-with-semicolon-separator
        //important: schema.ini must be saved as US-ASCII (in VS)
        [TestMethod]
        [DeploymentItem(@"TestData\FlightInvoiceTestdata.xlsx")]
        [DataSource("System.Data.Odbc", @"Dsn=Excel Files;dbq=.\FlightInvoiceTestdata.xlsx;defaultdir=.; driverid=790;maxbuffersize=2048;pagetimeout=5", "FlightInvoiceTestdata$", DataAccessMethod.Sequential)]
        public void DeliveryCreationTestingTest()
        {

            var useCase = TestContext.DataRow["UseCase"].ToString();

            if (string.IsNullOrWhiteSpace(useCase))
            {
                Logger.Debug(
                    $"Use case number is not set, so expect use case is not described correctly. Exit test for this use case.");
                return;
            }

            var subUseCase = TestContext.DataRow["UC-Variante"].ToString();
            var includeInTest = TestContext.DataRow["IncludeInTest"].ToString();

            Logger.Debug($"Use Case: {useCase}, UC-Variation: {subUseCase}");

            if (includeInTest != "1")
            {
                Logger.Debug($"Use case {useCase}{subUseCase} is excluded from Test. Exit test for this use case.");
                return;
            }

            #region Flight preparation

            var startTime = DateTime.Today.AddDays(-34).AddHours(10);
            var flightDetails = new FlightDetails();
            flightDetails.StartType = (int) AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData();
            flightDetails.GliderFlightDetailsData.AircraftId =
                GetAircraft(TestContext.DataRow["GliderImmatriculation"].ToString()).AircraftId;
            flightDetails.GliderFlightDetailsData.FlightComment = TestContext.DataRow["GliderFlightComment"].ToString();
            flightDetails.GliderFlightDetailsData.StartDateTime = startTime;
            flightDetails.GliderFlightDetailsData.LdgDateTime =
                startTime.AddMinutes(Convert.ToInt32(TestContext.DataRow["GliderFlightDuration"]));
            flightDetails.GliderFlightDetailsData.PilotPersonId =
                GetPerson(TestContext.DataRow["GliderPilotName"].ToString()).PersonId;
            flightDetails.GliderFlightDetailsData.StartLocationId =
                GetLocation(TestContext.DataRow["StartLocation"].ToString()).LocationId;
            flightDetails.GliderFlightDetailsData.LdgLocationId =
                GetLocation(TestContext.DataRow["GliderLdgLocation"].ToString()).LocationId;
            flightDetails.GliderFlightDetailsData.FlightTypeId =
                GetFlightType(TestContext.DataRow["GliderFlightCode"].ToString()).FlightTypeId;

            var displayname = TestContext.DataRow["GliderInstructorName"].ToString();

            if (string.IsNullOrWhiteSpace(displayname) == false)
            {
                var instructor = GetPerson(displayname);
                //don't throw an exception
                if (instructor != null)
                {
                    flightDetails.GliderFlightDetailsData.InstructorPersonId = instructor.PersonId;
                }
            }

            displayname = TestContext.DataRow["CopilotName"].ToString();

            if (string.IsNullOrWhiteSpace(displayname) == false)
            {
                var copilot = GetPerson(displayname);
                //don't throw an exception
                if (copilot != null)
                {
                    flightDetails.GliderFlightDetailsData.CoPilotPersonId = copilot.PersonId;
                }
            }

            displayname = TestContext.DataRow["PassengerName"].ToString();

            if (string.IsNullOrWhiteSpace(displayname) == false)
            {
                var passenger = GetPerson(displayname);
                //don't throw an exception
                if (passenger != null)
                {
                    flightDetails.GliderFlightDetailsData.PassengerPersonId = passenger.PersonId;
                }
            }

            if (Convert.ToInt32(TestContext.DataRow["StartType"]) == (int) AircraftStartType.TowingByAircraft)
            {
                flightDetails.TowFlightDetailsData = new TowFlightDetailsData();
                flightDetails.TowFlightDetailsData.AircraftId =
                    GetAircraft(TestContext.DataRow["TowingAircraftImmatriculation"].ToString()).AircraftId;
                flightDetails.TowFlightDetailsData.FlightComment = TestContext.DataRow["TowFlightComment"].ToString();
                flightDetails.TowFlightDetailsData.StartDateTime = startTime;
                flightDetails.TowFlightDetailsData.LdgDateTime =
                    startTime.AddMinutes(Convert.ToInt32(TestContext.DataRow["TowFlightDuration"]));
                flightDetails.TowFlightDetailsData.PilotPersonId =
                    GetPerson(TestContext.DataRow["TowPilotName"].ToString()).PersonId;
                flightDetails.TowFlightDetailsData.StartLocationId =
                    GetLocation(TestContext.DataRow["StartLocation"].ToString()).LocationId;
                flightDetails.TowFlightDetailsData.LdgLocationId =
                    GetLocation(TestContext.DataRow["TowLdgLocation"].ToString()).LocationId;
                flightDetails.TowFlightDetailsData.FlightTypeId =
                    GetFlightType(TestContext.DataRow["TowFlightCode"].ToString()).FlightTypeId;
            }

            FlightService.InsertFlightDetails(flightDetails);

            #endregion Flight preparation

            #region delivery creation test

            var deliveryCreationTestDetails = new DeliveryCreationTestDetails();
            deliveryCreationTestDetails.FlightId = flightDetails.FlightId;
            deliveryCreationTestDetails.DeliveryCreationTestName = "Glider flight delivery creation test";

            var expectedDelivery = DeliveryService.CreateDeliveryDetailsForTest(flightDetails.FlightId);
            Assert.IsNotNull(expectedDelivery.CreatedDeliveryDetails);

            deliveryCreationTestDetails.ExpectedDeliveryDetails = expectedDelivery.CreatedDeliveryDetails;
            deliveryCreationTestDetails.ExpectedMatchedAccountingRuleFilterIds =
                expectedDelivery.MatchedAccountingRuleFilterIds;

            DeliveryService.InsertDeliveryCreationTestDetails(deliveryCreationTestDetails);
            var testResult = DeliveryService.RunDeliveryCreationTest(deliveryCreationTestDetails.DeliveryCreationTestId);
            
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.LastDeliveryCreationTestResult);
            Assert.IsNotNull(testResult.LastDeliveryCreationTestResult.LastTestSuccessful);
            Assert.IsTrue(testResult.LastDeliveryCreationTestResult.LastTestSuccessful.Value);

            #endregion delivery creation test
        }


        [TestMethod]
        public void WhereDateTimeFilterTest()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var flights = context.Flights
                    .OrderByPropertyNames("StartDateTime:asc");

                var dateTimeFilter = new DateTimeFilter()
                {
                    From = DateTime.MinValue,
                    To = DateTime.Now
                };


                flights = flights.WhereDateTimeFilter(x => x.StartDateTime, dateTimeFilter);

                var filteredFlights = flights.ToList();

                var flights2 = context.Flights
                    .OrderByPropertyNames("StartDateTime:asc");

                var dateTimeFilter2 = new DateTimeFilter()
                {
                    To = DateTime.Now,
                    From = DateTime.Now
                };


                flights2 = flights2.WhereDateTimeFilter(x => x.StartDateTime, dateTimeFilter2);

                var filteredFlights2 = flights2.ToList();
            }
        }

        [TestMethod]
        public void GetDeliveryNotProcessedTest()
        {
            var deliveries = DeliveryService.GetDeliveryDetailsList(false);

        }
    }
}
