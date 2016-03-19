using System;
using System.Collections.Generic;
using System.Linq;
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
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\FlightInvoiceTestdata.csv", "FlightInvoiceTestdata#csv", 
            DataAccessMethod.Sequential), DeploymentItem("FLS.Server.Tests\\FlightInvoiceTestdata.csv")]
        public void ProffixInvoiceTest()
        {
            var useCase = TestContext.DataRow["UseCase"].ToString();

            if (string.IsNullOrWhiteSpace(useCase))
            {
                Logger.Debug($"Use case number is not set, so expect use case is not described correctly. Exit ProffixInvoiceTest for this use case.");
                return;
            }

            var subUseCase = TestContext.DataRow["UC-Variante"].ToString();
            Logger.Debug($"ProffixInvoiceTest for Use Case: {useCase}, UC-Variation: {subUseCase}");

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

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData();
            flightDetails.TowFlightDetailsData.AircraftId = GetAircraft(TestContext.DataRow["TowingAircraftImmatriculation"].ToString()).AircraftId;
            flightDetails.TowFlightDetailsData.FlightComment = TestContext.DataRow["TowFlightComment"].ToString();
            flightDetails.TowFlightDetailsData.StartDateTime = startTime;
            flightDetails.TowFlightDetailsData.LdgDateTime = startTime.AddMinutes(Convert.ToInt32(TestContext.DataRow["TowFlightDuration"]));
            flightDetails.TowFlightDetailsData.PilotPersonId = GetPerson(TestContext.DataRow["TowPilotName"].ToString()).PersonId;
            flightDetails.TowFlightDetailsData.StartLocationId = GetLocation(TestContext.DataRow["StartLocation"].ToString()).LocationId;
            flightDetails.TowFlightDetailsData.LdgLocationId = GetLocation(TestContext.DataRow["TowLdgLocation"].ToString()).LocationId;
            flightDetails.TowFlightDetailsData.FlightTypeId = GetFlightType(TestContext.DataRow["TowFlightCode"].ToString()).FlightTypeId;

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
            var expectedInvoiceRecipientPersonDisplayName = TestContext.DataRow["ExpectedInvoiceRecipientPersonDisplayName"].ToString();
            var expectedInvoiceFlightInfo = TestContext.DataRow["ExpectedInvoiceFlightInfo"].ToString();
            var expectedInvoiceAdditionalInfo = TestContext.DataRow["ExpectedInvoiceAdditionalInfo"].ToString();
            var expectedErpArticleNumberLine1 = TestContext.DataRow["ExpectedErpArticleNumberLine1"].ToString();
            var expectedQuantityLine1 = TestContext.DataRow["ExpectedQuantityLine1"].ToString().ToDecimal();
            var expectedUnitTypeLine1 = TestContext.DataRow["ExpectedUnitTypeLine1"].ToString();
            var expectedErpArticleNumberLine2 = TestContext.DataRow["ExpectedErpArticleNumberLine2"].ToString();
            var expectedQuantityLine2 = TestContext.DataRow["ExpectedQuantityLine2"].ToString().ToDecimal();
            var expectedUnitTypeLine2 = TestContext.DataRow["ExpectedUnitTypeLine2"].ToString();
            var expectedErpArticleNumberLine3 = TestContext.DataRow["ExpectedErpArticleNumberLine3"].ToString();
            var expectedQuantityLine3 = TestContext.DataRow["ExpectedQuantityLine3"].ToString().ToDecimal();
            var expectedUnitTypeLine3 = TestContext.DataRow["ExpectedUnitTypeLine3"].ToString();
            var expectedErpArticleNumberLine4 = TestContext.DataRow["ExpectedErpArticleNumberLine4"].ToString();
            var expectedQuantityLine4 = TestContext.DataRow["ExpectedQuantityLine4"].ToString().ToDecimal();
            var expectedUnitTypeLine4 = TestContext.DataRow["ExpectedUnitTypeLine4"].ToString();
            var expectedErpArticleNumberLine5 = TestContext.DataRow["ExpectedErpArticleNumberLine5"].ToString();
            var expectedQuantityLine5 = TestContext.DataRow["ExpectedQuantityLine5"].ToString().ToDecimal();
            var expectedUnitTypeLine5 = TestContext.DataRow["ExpectedUnitTypeLine5"].ToString();
            var expectedErpArticleNumberLine6 = TestContext.DataRow["ExpectedErpArticleNumberLine6"].ToString();
            var expectedQuantityLine6 = TestContext.DataRow["ExpectedQuantityLine6"].ToString().ToDecimal();
            var expectedUnitTypeLine6 = TestContext.DataRow["ExpectedUnitTypeLine6"].ToString();

            if (expectInvoice == "1")
            {
                Assert.AreEqual(invoices.Count, 1);
            }
            else
            {
                Assert.AreEqual(invoices.Count, 0);
            }

            foreach (var flightInvoiceDetails in invoices)
            {
                Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, expectedInvoiceLineItemsCount);
                Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, expectedInvoiceAircraftImmatriculation);
                Assert.AreEqual(flightInvoiceDetails.InvoiceRecipientPersonDisplayName, expectedInvoiceRecipientPersonDisplayName);
                Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, expectedInvoiceFlightInfo);
                Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, expectedInvoiceAdditionalInfo);

                foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                {
                    if (line.InvoiceLinePosition == 1)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine1);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine1);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine1);
                    }

                    if (line.InvoiceLinePosition == 2)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine2);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine2);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine2);
                    }

                    if (line.InvoiceLinePosition == 3)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine3);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine3);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine3);
                    }

                    if (line.InvoiceLinePosition == 4)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine4);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine4);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine4);
                    }

                    if (line.InvoiceLinePosition == 5)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine5);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine5);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine5);
                    }

                    if (line.InvoiceLinePosition == 6)
                    {
                        Assert.AreEqual(line.ERPArticleNumber, expectedErpArticleNumberLine6);
                        Assert.AreEqual(line.Quantity, expectedQuantityLine6);
                        Assert.AreEqual(line.UnitType, expectedUnitTypeLine6);
                    }
                }

                var flightInvoiceBooking = new FlightInvoiceBooking
                {
                    FlightId = flightInvoiceDetails.FlightId,
                    IncludesTowFlightId = flightInvoiceDetails.IncludesTowFlightId,
                    InvoiceDate = DateTime.Now.Date,
                    InvoiceNumber = $"ProffixInvoiceTest {DateTime.Now.ToShortTimeString()}"
                };

                var isFlightInvoiced = InvoiceService.SetFlightAsInvoiced(flightInvoiceBooking);
                Assert.IsTrue(isFlightInvoiced);
            }

            #endregion invoice check
        }
    }
}
