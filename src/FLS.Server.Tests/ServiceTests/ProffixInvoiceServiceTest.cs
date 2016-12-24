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
using FLS.Data.WebApi.Invoicing;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class ProffixInvoiceServiceTest : BaseTest
    {
        private class ExpectedFlightInvoiceLineItem
        {
            public int InvoiceLinePosition { get; set; }

            public string ERPArticleNumber { get; set; }

            public string InvoiceLineText { get; set; }

            public string AdditionalInfo { get; set; }

            public decimal Quantity { get; set; }

            public string UnitType { get; set; }
        }

        [TestInitialize]
        public void ProffixInvoiceTestInitialize()
        { 
            Console.WriteLine("ProffixInvoiceTestInitialize");
        }

        [TestCleanup]
        public void ProffixInvoiceTestCleanup()
        {
            Logger.Debug($"Run Proffix Invoice Test Cleanup after Use case: {TestContext.DataRow["UseCase"]}");

            //set not invoiced flights to valid flights, so that it will not invoiced during next run in ProffixInvoiceTest test data line
            using (var context = DataAccessService.CreateDbContext())
            {
                var lockedFlights = context.Flights.Where(x => x.ProcessStateId == (int) FLS.Data.WebApi.Flight.FlightProcessState.Locked);

                if (lockedFlights.Any())
                {
                    foreach (var lockedFlight in lockedFlights)
                    {
                        lockedFlight.ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.NotProcessed;
                        lockedFlight.DoNotUpdateMetaData = true;
                        Logger.Debug($"Set flight process state to not processed for FlightId: {lockedFlight.FlightId}");
                    }

                    context.SaveChanges();
                }
                else
                {
                    Logger.Debug("Nothing cleaned up");
                }

            }

            //Thread.Sleep(1000);
        }

        //http://stackoverflow.com/questions/24012253/datadriven-mstests-csv-with-semicolon-separator
        //important: schema.ini must be saved as US-ASCII (in VS)
        [TestMethod]
        [DeploymentItem(@"TestData\schema.ini")]
        [DeploymentItem(@"TestData\FlightInvoiceTestdata.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\TestData\FlightInvoiceTestdata.csv", "FlightInvoiceTestdata#csv", 
            DataAccessMethod.Sequential)]
        public void ProffixInvoiceTest()
        {
            
            var useCase = TestContext.DataRow["UseCase"].ToString();

            if (string.IsNullOrWhiteSpace(useCase))
            {
                Logger.Debug($"Use case number is not set, so expect use case is not described correctly. Exit ProffixInvoiceTest for this use case.");
                return;
            }

            var subUseCase = TestContext.DataRow["UC-Variante"].ToString();
            var includeInTest = TestContext.DataRow["IncludeInTest"].ToString();

            Logger.Debug($"ProffixInvoiceTest for Use Case: {useCase}, UC-Variation: {subUseCase}");

            if (includeInTest != "1")
            {
                Logger.Debug($"Use case {useCase}{subUseCase} is excluded from Test. Exit ProffixInvoiceTest for this use case.");
                return;
            }

            #region Flight preparation
            var startTime = DateTime.Today.AddDays(-34).AddHours(10);
            var flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

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

            if (Convert.ToInt32(TestContext.DataRow["StartType"]) == (int)AircraftStartType.TowingByAircraft)
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
            SetFlightAsLocked(flightDetails);
            #endregion Flight preparation

            #region invoice check
            var fromDate = new DateTime(DateTime.Now.AddDays(-10).Year, 1, 1);
            var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var invoices = InvoiceService.GetFlightInvoiceDetails(fromDate, toDate,
                IdentityService.CurrentAuthenticatedFLSUser.ClubId);

            var expectInvoice = TestContext.DataRow["ExpectInvoice"].ToString();
            var expectedInvoiceLineItemsCount = Convert.ToInt32(TestContext.DataRow["ExpectedInvoiceLineItemsCount"].ToString());
            var expectedInvoiceAircraftImmatriculation = TestContext.DataRow["ExpectedInvoiceAircraftImmatriculation"].ToString();
            var expectedInvoiceRecipientName = TestContext.DataRow["ExpectedInvoiceRecipientName"].ToString();
            var expectedInvoiceFlightInfo = TestContext.DataRow["ExpectedInvoiceFlightInfo"].ToString();
            var expectedInvoiceAdditionalInfo = TestContext.DataRow["ExpectedInvoiceAdditionalInfo"].ToString();

            var expectedInvoiceLines = new Dictionary<int, ExpectedFlightInvoiceLineItem>();

            for (int i = 1; i < 10; i++)
            {
                var erpArticle = TestContext.DataRow[$"ExpectedErpArticleNumberLine{i}"].ToString();

                if (string.IsNullOrWhiteSpace(erpArticle)) continue;

                var expectedInvoiceLine = new ExpectedFlightInvoiceLineItem()
                {
                    InvoiceLinePosition = i,
                    ERPArticleNumber = erpArticle,
                    Quantity = TestContext.DataRow[$"ExpectedQuantityLine{i}"].ToString().ToDecimal(),
                    UnitType = TestContext.DataRow[$"ExpectedUnitTypeLine{i}"].ToString()
                };
                expectedInvoiceLines.Add(i, expectedInvoiceLine);
            }
            
            Assert.AreEqual(expectedInvoiceLineItemsCount, expectedInvoiceLines.Count, 0, "Value in column ExpectedInvoiceLineItemsCount in test data does not fit with expected line values. Check FlightInvoiceTestdata.csv");

            if (expectInvoice == "1")
            {
                Assert.AreEqual(1, invoices.Count, "Number of expected invoices is not 1");
            }
            else
            {
                Assert.AreEqual(0, invoices.Count, "Number of expected invoices is not 0");
            }

            foreach (var flightInvoiceDetails in invoices)
            {
                Assert.AreEqual(expectedInvoiceRecipientName, flightInvoiceDetails.RecipientDetails.RecipientName, "Wrong recipient person in invoice");
                Assert.AreEqual(expectedInvoiceFlightInfo, flightInvoiceDetails.FlightInvoiceInfo, "Wrong invoice information in invoice");
                Assert.AreEqual(expectedInvoiceAdditionalInfo, flightInvoiceDetails.AdditionalInfo, "Wrong additional information in invoice");
                Assert.AreEqual(expectedInvoiceAircraftImmatriculation, flightInvoiceDetails.AircraftImmatriculation, "Wrong aircraft immatriculation reported in invoice");

                if (expectedInvoiceLineItemsCount != flightInvoiceDetails.FlightInvoiceLineItems.Count)
                {
                    if (flightInvoiceDetails.FlightInvoiceLineItems.Count == 0)
                    {
                        Assert.AreEqual(expectedInvoiceLineItemsCount, flightInvoiceDetails.FlightInvoiceLineItems.Count,
                        $"Number of invoice lines is not as expected. No invoice lines created.");
                    }
                    else
                    {
                        Assert.AreEqual(expectedInvoiceLineItemsCount, flightInvoiceDetails.FlightInvoiceLineItems.Count,
                            $"Number of invoice lines is not as expected. Created invoice lines are:{Environment.NewLine}{GetInvoiceLinesForLogging(flightInvoiceDetails)}{Environment.NewLine}Expected lines are:{Environment.NewLine}{GetInvoiceLinesForLogging(expectedInvoiceLines)}");
                    }
                }

                foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                {
                    Assert.AreEqual(expectedInvoiceLines[line.InvoiceLinePosition].ERPArticleNumber, line.ArticleNumber, $"Article number in invoice line {line.InvoiceLinePosition} is wrong.");
                    Assert.AreEqual(expectedInvoiceLines[line.InvoiceLinePosition].Quantity, line.Quantity, $"Quantity in invoice line {line.InvoiceLinePosition} is wrong.");
                    Assert.AreEqual(expectedInvoiceLines[line.InvoiceLinePosition].UnitType, line.UnitType, $"Unittype in invoice line {line.InvoiceLinePosition} is wrong.");
                }

                var flightInvoiceBooking = new FlightInvoiceBooking
                {
                    FlightId = flightInvoiceDetails.FlightId,
                    InvoiceDate = DateTime.Now.Date,
                    InvoiceNumber = $"ProffixInvoiceTest {DateTime.Now.ToShortTimeString()}"
                };

                var isFlightInvoiced = InvoiceService.SetFlightAsInvoiced(flightInvoiceBooking);
                Assert.IsTrue(isFlightInvoiced, $"Flight with Id: {flightInvoiceDetails.FlightId} could not be set as invoiced");

                //check flight state
                var invoicedFlight = FlightService.GetFlight(flightInvoiceDetails.FlightId);
                Assert.AreEqual((int)FLS.Data.WebApi.Flight.FlightProcessState.Invoiced, invoicedFlight.ProcessStateId, $"Flight process state of master flight {invoicedFlight} was not set correctly after invoicing");

                if (invoicedFlight.TowFlightId.HasValue)
                {
                    Assert.IsNotNull(invoicedFlight.TowFlight, "The invoiced flight has no loaded tow flight reference.");

                    Assert.AreEqual((int) FLS.Data.WebApi.Flight.FlightProcessState.Invoiced,
                        invoicedFlight.TowFlight.ProcessStateId,
                        $"Flight state of tow flight {invoicedFlight.TowFlight} was not set correctly after invoicing");

                    Assert.AreEqual(flightInvoiceBooking.InvoiceNumber,
                        invoicedFlight.TowFlight.InvoiceNumber,
                        $"Invoice number of tow flight {invoicedFlight.TowFlight} was not set correctly after invoicing");

                    Assert.AreEqual(flightInvoiceBooking.InvoiceDate,
                        invoicedFlight.TowFlight.InvoicedOn,
                        $"Invoice date of tow flight {invoicedFlight.TowFlight} was not set correctly after invoicing");
                }
            }

            #endregion invoice check
        }

        private string GetInvoiceLinesForLogging(FlightInvoiceDetails flightInvoiceDetails)
        {
            var sb = new StringBuilder();
            foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
            {
                sb.Append($"{line.InvoiceLinePosition} {line.ArticleNumber} {line.InvoiceLineText} {line.Quantity} {line.UnitType}");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private string GetInvoiceLinesForLogging(Dictionary<int, ExpectedFlightInvoiceLineItem> expectedFlightInvoiceLines)
        {
            var sb = new StringBuilder();
            foreach (var lineNr in expectedFlightInvoiceLines.Keys)
            {
                sb.Append($"{expectedFlightInvoiceLines[lineNr].InvoiceLinePosition} {expectedFlightInvoiceLines[lineNr].ERPArticleNumber} {expectedFlightInvoiceLines[lineNr].InvoiceLineText} {expectedFlightInvoiceLines[lineNr].Quantity} {expectedFlightInvoiceLines[lineNr].UnitType}");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
