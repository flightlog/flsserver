using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using FLS.Data.WebApi.Accounting;
using FLS.Server.Data;
using FLS.Server.Tests.Infrastructure;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class SGNProffixInvoiceServiceTest : BaseTest
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
        public void SGNProffixInvoiceTestInitialize()
        {
            Console.WriteLine("SGNProffixInvoiceTestInitialize");

            using (var context = DataAccessService.CreateDbContext())
            {
                Club club = context.Clubs.FirstOrDefault(x => x.ClubKey == "SGN");

                if (club == null)
                {
                    var country = context.Countries.FirstOrDefault();

                    club = new Club()
                    {
                        ClubKey = "SGN",
                        Clubname = "SGN",
                        Country = country
                    };

                    context.Clubs.Add(club);

                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    using (var file = new StreamReader(Path.Combine(baseDir, "AccountingRuleFilters_SGN.csv")))
                    {
                        var entityType = typeof(AccountingRuleFilter);
                        var headerLine = file.ReadLine();
                        var columnNames = headerLine.Split(new string[] {";"}, StringSplitOptions.None);

                        while (!file.EndOfStream)
                        {
                            try
                            {
                                var dbEntity = new AccountingRuleFilter();
                                var curLineValues = file.ReadLine().Split(new string[] { ";" }, StringSplitOptions.None);
                                for (int i = 0; i < columnNames.Length; i++)
                                {
                                    var property = entityType.GetProperty(columnNames[i]);
                                    ParseStringValue(property, dbEntity, curLineValues[i]);
                                }

                                dbEntity.Club = club;

                                context.AccountingRuleFilters.Add(dbEntity);
                            }
                            catch (Exception exception)
                            {
                                Logger.Error(exception, "Error while trying to create new AccountingRuleFilter");
                            }
                        }
                    }

                    //Insert FlightTypes for Club
                    var flightType = new FlightType()
                    {
                        FlightCode = "54",
                        FlightTypeName = "Windenstart Extern (WSE)",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 0
                    };
                    context.FlightTypes.Add(flightType);

                    flightType = new FlightType()
                    {
                        FlightCode = "100",
                        FlightTypeName = "Marketingflug",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = true,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 2
                    };
                    context.FlightTypes.Add(flightType);

                    flightType = new FlightType()
                    {
                        FlightCode = "95",
                        FlightTypeName = "Schlepp Fremdflugzeuge",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);

                    flightType = new FlightType()
                    {
                        FlightCode = "90",
                        FlightTypeName = "Schleppflug (FSS, FSG, FSP)",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 0
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "63",
                        FlightTypeName = "Passagierflug",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = true,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 2
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "92",
                        FlightTypeName = "Schleppstart Externer Flugplatz",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 0
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "91 (old)",
                        FlightTypeName = "Schleppcheck",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = true,
                        IsCheckFlight = true,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 0
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "71",
                        FlightTypeName = "Schulung TMG",
                        Club = club,
                        InstructorRequired = true,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "78",
                        FlightTypeName = "Checkflug",
                        Club = club,
                        InstructorRequired = true,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = true,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 2
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "65",
                        FlightTypeName = "Fremdflugzeuge (nicht Platzansässige)",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "94",
                        FlightTypeName = "SGOW Überführungsschlepp",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "62",
                        FlightTypeName = "Passagierflug mit Gutschein",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = true,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = true,
                        MinNrOfAircraftSeatsRequired = 2
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "91",
                        FlightTypeName = "SGN Überführungsschlepp",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "70",
                        FlightTypeName = "Schulung",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = true,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "60",
                        FlightTypeName = "Clubflugzeug",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "93",
                        FlightTypeName = "Schlepp SGOW",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = false,
                        IsForTowFlights = true,
                        IsForMotorFlights = false,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 0
                    };
                    context.FlightTypes.Add(flightType);
                    flightType = new FlightType()
                    {
                        FlightCode = "61",
                        FlightTypeName = "Privatflugzeug",
                        Club = club,
                        InstructorRequired = false,
                        ObserverPilotOrInstructorRequired = false,
                        IsCheckFlight = false,
                        IsPassengerFlight = false,
                        IsSoloFlight = false,
                        IsForGliderFlights = true,
                        IsForTowFlights = false,
                        IsForMotorFlights = true,
                        IsFlightCostBalanceSelectable = false,
                        IsCouponNumberRequired = false,
                        MinNrOfAircraftSeatsRequired = 1
                    };
                    context.FlightTypes.Add(flightType);
                }

                User user = context.Users.FirstOrDefault(x => x.UserName == "sgn");

                if (user == null)
                {
                    user = new User()
                    {
                        UserName = "sgn",
                        FriendlyName = "sgn",
                        NotificationEmail = "sgn@glider-fls.ch",
                        Club = club,
                        AccountState = 1
                    };

                    context.Users.Add(user);
                }

                //TODO: merge into derived classes instead duplicate code

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }

                IdentityService.SetUser(user);
            }
        }

        private void ParseStringValue(PropertyInfo p, object row, string value)
        {
            p.SetValue(row, ParseStringValue(p, value));
        }

        private object ParseStringValue(PropertyInfo p, string value)
        {
            object parsed = null;

            if (value != "NULL")
            {
                value = value.Trim('\"');

                if (p.PropertyType == typeof(string))
                    parsed = value;
                else if (p.PropertyType == typeof(bool))
                {
                    if (value == "1")
                    {
                        parsed = true;
                    }
                    else if (value == "0")
                    {
                        parsed = false;
                    }
                    else
                    {
                        try
                        {
                            parsed = Convert.ToBoolean(value);
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException("The string is not a recognized as a valid boolean value.");
                        }
                    }
                }
                else
                {
                    Type parseMethodType = p.PropertyType;
                    if (parseMethodType.IsGenericType) // normally Nullable<T> - extract parse method of T (NULL is handled above)
                        parseMethodType = parseMethodType.GenericTypeArguments.First();

                    var parseMethod = parseMethodType.GetMethod("Parse", new Type[] { typeof(string) });
                    parsed = parseMethod.Invoke(null, new[] { value });
                }
            }

            return parsed;
        }

        [TestCleanup]
        public void SGNProffixInvoiceTestCleanup()
        {
            Logger.Debug($"Run SGN Proffix Invoice Test Cleanup after Use case: {TestContext.DataRow["UseCase"]}");

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
        //[DeploymentItem(@"TestData\schema.ini")]
        [DeploymentItem(@"TestData\FlightInvoiceTestdata_SGN.xlsx")]
        [DeploymentItem(@"Resources\AccountingRuleFilters_SGN.csv")]
        [DataSource("System.Data.Odbc", @"Dsn=Excel Files;dbq=.\FlightInvoiceTestdata_SGN.xlsx;defaultdir=.; driverid=790;maxbuffersize=2048;pagetimeout=5", "FlightInvoiceTestdata_SGN$", DataAccessMethod.Sequential)]
        public void SGNProffixGliderInvoiceTest()
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
            flightDetails.StartType = Convert.ToInt32(TestContext.DataRow["StartType"].ToString());

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

            var engineTimeInSeconds = Convert.ToInt32(TestContext.DataRow["EngineTimeInSeconds"].ToString());

            if (engineTimeInSeconds > 0)
            {
                flightDetails.GliderFlightDetailsData.EngineStartOperatingCounterInSeconds = 0;
                flightDetails.GliderFlightDetailsData.EngineEndOperatingCounterInSeconds = engineTimeInSeconds;
            }

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
            var invoices = DeliveryService.CreateDeliveriesFromFlights(IdentityService.CurrentAuthenticatedFLSUser.ClubId);

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

                Logger.Info($"ERP article for invoice line number: {i} is: {erpArticle}");

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
                Assert.AreEqual(expectedInvoiceFlightInfo, flightInvoiceDetails.DeliveryInformation, "Wrong invoice information in invoice");
                Assert.AreEqual(expectedInvoiceAdditionalInfo, flightInvoiceDetails.AdditionalInformation, "Wrong additional information in invoice");
                Assert.AreEqual(expectedInvoiceAircraftImmatriculation, flightInvoiceDetails.FlightInformation.AircraftImmatriculation, "Wrong aircraft immatriculation reported in invoice");

                if (expectedInvoiceLineItemsCount != flightInvoiceDetails.DeliveryItems.Count)
                {
                    if (flightInvoiceDetails.DeliveryItems.Count == 0)
                    {
                        Assert.AreEqual(expectedInvoiceLineItemsCount, flightInvoiceDetails.DeliveryItems.Count,
                        $"Number of invoice lines is not as expected. No invoice lines created.");
                    }
                    else
                    {
                        Assert.AreEqual(expectedInvoiceLineItemsCount, flightInvoiceDetails.DeliveryItems.Count,
                            $"Number of invoice lines is not as expected. Created invoice lines are:{Environment.NewLine}{GetInvoiceLinesForLogging(flightInvoiceDetails)}{Environment.NewLine}Expected lines are:{Environment.NewLine}{GetInvoiceLinesForLogging(expectedInvoiceLines)}");
                    }
                }

                foreach (var line in flightInvoiceDetails.DeliveryItems.OrderBy(o => o.Position))
                {
                    Assert.AreEqual(expectedInvoiceLines[line.Position].ERPArticleNumber, line.ArticleNumber, $"Article number in invoice line {line.Position} is wrong.");
                    Assert.AreEqual(expectedInvoiceLines[line.Position].Quantity, line.Quantity, $"Quantity in invoice line {line.Position} is wrong.");
                    Assert.AreEqual(expectedInvoiceLines[line.Position].UnitType, line.UnitType, $"Unittype in invoice line {line.Position} is wrong.");
                }

                var deliveryBooking = new DeliveryBooking()
                {
                    DeliveryId = flightInvoiceDetails.DeliveryId,
                    DeliveryDateTime = DateTime.Now.Date,
                    DeliveryNumber = $"ProffixInvoiceTest {DateTime.Now.ToShortTimeString()}"
                };

                var isFlightInvoiced = DeliveryService.SetDeliveryAsDelivered(deliveryBooking);
                Assert.IsTrue(isFlightInvoiced, $"Flight with Id: {flightInvoiceDetails.FlightInformation.FlightId} could not be set as invoiced");

                //check flight state
                var invoicedFlight = FlightService.GetFlight(flightInvoiceDetails.FlightInformation.FlightId);
                Assert.AreEqual((int)FLS.Data.WebApi.Flight.FlightProcessState.Delivered, invoicedFlight.ProcessStateId, $"Flight process state of master flight {invoicedFlight} was not set correctly after delivering.");

                if (invoicedFlight.TowFlightId.HasValue)
                {
                    Assert.IsNotNull(invoicedFlight.TowFlight, "The invoiced flight has no loaded tow flight reference.");

                    Assert.AreEqual((int) FLS.Data.WebApi.Flight.FlightProcessState.Delivered,
                        invoicedFlight.TowFlight.ProcessStateId,
                        $"Flight state of tow flight {invoicedFlight.TowFlight} was not set correctly after delivering");
                }
            }

            #endregion invoice check
        }

        private string GetInvoiceLinesForLogging(DeliveryDetails flightInvoiceDetails)
        {
            var sb = new StringBuilder();
            foreach (var line in flightInvoiceDetails.DeliveryItems.OrderBy(o => o.Position))
            {
                sb.Append($"{line.Position} {line.ArticleNumber} {line.ItemText} {line.Quantity} {line.UnitType}");
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
