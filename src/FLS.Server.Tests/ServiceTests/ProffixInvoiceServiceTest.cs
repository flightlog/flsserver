using System;
using System.Linq;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class ProffixInvoiceServiceTest : BaseServiceTest
    {
        private TestContext _testContextInstance;
        private UserService _userService;
        private FlightHelper _flightHelper;
        private PersonHelper _personHelper;
        private LocationHelper _locationHelper;
        private IdentityService _identityService;
        private WorkflowService _workflowService;
        private InvoiceService _invoiceService;

        [TestInitialize]
        public void TestInitialize()
        {
            _flightHelper = UnityContainer.Resolve<FlightHelper>();
            _personHelper = UnityContainer.Resolve<PersonHelper>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
            _userService = UnityContainer.Resolve<UserService>();
            _workflowService = UnityContainer.Resolve<WorkflowService>();
            _invoiceService = UnityContainer.Resolve<InvoiceService>();

            var user = _userService.GetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
            Assert.IsNotNull(user);
            _identityService = UnityContainer.Resolve<IdentityService>();
            _identityService.SetUser(user);

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
        public void ProffixInvoiceFullTest()
        {
            var flightsDictionary = _flightHelper.CreateFlightsForProffixInvoicingTests(_identityService.CurrentAuthenticatedFLSUser.ClubId, DateTime.Today.AddMonths(-1));

            var fromDate = new DateTime(DateTime.Now.AddDays(-10).Year, 1, 1);
            var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var invoices = _invoiceService.GetFlightInvoiceDetails(fromDate, toDate,
                _identityService.CurrentAuthenticatedFLSUser.ClubId);

            foreach (var flightInvoiceDetails in invoices)
            {
                #region UC1: create local charter flight with 1 seat glider and less then 10 min. towing
                //UC1: local charter flight with 1 seat glider and less then 10 min. towing
                //HB-1824 Privat Charter Clubflugzeug
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC1"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 5);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-1824",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Charter Clubflugzeug");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "0"); //keine Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1064"); //HB-1824 Privat Charter Clubflugzeug
                            Assert.AreEqual(line.Quantity, 42);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1066"); //Schlepp Privat HB-KCB ab 0. Min.
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1037"); //Landetaxen Speck
                            Assert.AreEqual(line.Quantity, 1);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                        else if (line.InvoiceLinePosition == 5)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC1: create local charter flight with 1 seat glider and less then 10 min. towing

                #region UC2: create local charter flight with 1 seat glider and more then 10 min. towing
                //UC2: create local charter flight with 1 seat glider and more then 10 min. towing
                //HB-2464 Privat Charter Clubflugzeug
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC2"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 6);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-2464",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Charter Clubflugzeug");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "0"); //keine Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1071"); //HB-2464 Privat Charter Clubflugzeug
                            Assert.AreEqual(line.Quantity, 355);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1066"); //Schlepp Privat HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 10);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1067"); //Schlepp Privat HB-KCB ab 10. Min.
                            Assert.AreEqual(line.Quantity, 12);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 22);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 5)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1037"); //Landetaxen Speck
                            Assert.AreEqual(line.Quantity, 1);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                        else if (line.InvoiceLinePosition == 6)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC2: create local charter flight with 1 seat glider and more then 10 min. towing

                #region UC3a: create local charter flight with 2 seat glider and more then 10 min. towing, pilot pays
                //UC3a: create local charter flight with 2 seat glider and more then 10 min. towing, pilot pays
                //HB-3407 Charter Clubflugzeug
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC3a"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 6);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-3407",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Charter Clubflugzeug");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "0"); //keine Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1060"); //HB-3407 Privat Charter Clubflugzeug
                            Assert.AreEqual(line.Quantity, 65);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1066"); //Schlepp Privat HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 10);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1067"); //Schlepp Privat HB-KCB ab 10. Min.
                            Assert.AreEqual(line.Quantity, 1);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 11);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 5)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1037"); //Landetaxen Speck
                            Assert.AreEqual(line.Quantity, 1);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                        else if (line.InvoiceLinePosition == 6)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC3a: create local charter flight with 2 seat glider and more then 10 min. towing, pilot pays

                #region UC4: create local private charter flight with 2 seat private glider and 10 min. towing
                //UC4: create local private charter flight with 2 seat private glider and 10 min. towing
                //HB-3254 Charter Privatflugzeug
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC4"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 4);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-3254",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Charter Privatflugzeug");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "0"); //keine Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1066"); //Schlepp Privat HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 10);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 10);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1037"); //Landetaxen Speck
                            Assert.AreEqual(line.Quantity, 1);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC4: create local private charter flight with 2 seat private glider and 10 min. towing

                #region UC5: create local trainee flight with 2 seat glider and less then 10 min. towing
                //UC5: create local trainee flight with 2 seat glider and less then 10 min. towing
                //HB - 3256 Schulung Grundschulung Doppelsteuer
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC5"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 5);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-3256",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Grundschulung Doppelsteuer");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "1"); //Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1059"); //HB-3256 Schulung Grundschulung Doppelsteuer
                            Assert.AreEqual(line.Quantity, 22);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "19"); //Fluglehrer-Honorar Kellner Hansli
                            Assert.AreEqual(line.Quantity, 22);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1068"); //Schlepp Schulung HB-KCB ab 0. Min.
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 5)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC5: create local trainee flight with 2 seat glider and less then 10 min. towing

                #region UC6: create local trainee flight with 2 seat glider and more then 10 min. towing
                //UC6: create local trainee flight with 2 seat glider and more then 10 min. towing
                //HB - 3256 Schulung Grundschulung Doppelsteuer
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC6"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 6);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-3256",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Grundschulung Doppelsteuer");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "1"); //Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1059"); //HB-3256 Schulung Grundschulung Doppelsteuer
                            Assert.AreEqual(line.Quantity, 185);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "19"); //Fluglehrer-Honorar Kellner Hansli
                            Assert.AreEqual(line.Quantity, 185);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1068"); //Schlepp Schulung HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 10);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1069"); //Schlepp Schulung HB-KCB ab 10. Min.
                            Assert.AreEqual(line.Quantity, 4);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 5)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 14);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 6)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC6: create local trainee flight with 2 seat glider and more then 10 min. towing


                #region UC7: create local solo trainee flight with 1 seat glider and less then 10 min. towing
                //UC7: create local solo trainee flight with 1 seat glider and less then 10 min. towing
                //HB - 1824 Schulung Grundschulung Solo
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC7"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 5);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-1824",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Grundschulung Solo");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "1"); //Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1065"); //HB-1824 Schulung Grundschulung Solo
                            Assert.AreEqual(line.Quantity, 14);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "19"); //Fluglehrer-Honorar Kellner Hansli
                            Assert.AreEqual(line.Quantity, 14);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1068"); //Schlepp Schulung HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 6);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 6);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 6)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC7: create local trainee flight with 2 seat glider and more then 10 min. towing


                #region UC8: create local solo further education flight with 1 seat glider and less then 10 min. towing
                //UC8: create local solo further education flight with 1 seat glider and less then 10 min. towing
                //HB - 2464 Weiterbildung Solo
                if (flightInvoiceDetails.FlightId == flightsDictionary["UC8"])
                {
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceLineItems.Count, 5);
                    Assert.AreEqual(flightInvoiceDetails.AircraftImmatriculation, "HB-2464",
                        "invoiced aircraft is not as expected");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(flightInvoiceDetails.InvoiceRecipientPersonDisplayName));
                    Assert.AreEqual(flightInvoiceDetails.FlightInvoiceInfo, "Weiterbildung Solo");
                    Assert.AreEqual(flightInvoiceDetails.AdditionalInfo, "1"); //Schulung

                    foreach (var line in flightInvoiceDetails.FlightInvoiceLineItems.OrderBy(o => o.InvoiceLinePosition))
                    {
                        Assert.AreEqual(line.AdditionalInfo, null);

                        if (line.InvoiceLinePosition == 1)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1072"); //HB-2464 Schulung Weiterbildung Solo
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 2)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "19"); //Fluglehrer-Honorar Kellner Hansli
                            Assert.AreEqual(line.Quantity, 8);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 3)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1068"); //Schlepp Schulung HB-KCB 1. bis 10. Min.
                            Assert.AreEqual(line.Quantity, 5);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 4)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1086"); //Treibstoffzuschlag HB-KCB
                            Assert.AreEqual(line.Quantity, 5);
                            Assert.AreEqual(line.UnitType, "Minuten");
                        }
                        else if (line.InvoiceLinePosition == 6)
                        {
                            Assert.AreEqual(line.ERPArticleNumber, "1003"); //VFS-Gebühr
                            Assert.AreEqual(line.Quantity, 2);
                            Assert.AreEqual(line.UnitType, "Landung");
                        }
                    }
                }
                #endregion UC8: create local solo further education flight with 1 seat glider and less then 10 min. towing
            }
        }
        
    }
}
