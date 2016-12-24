using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class InvoicesControllerTest : BaseAuthenticatedTests
    {
        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetAllFlightInvoiceDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<FlightInvoiceDetails>>("/api/v1/invoices").Result;

            Assert.IsTrue(response.Any());
        }

        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetFlightInvoiceDetailsWebApiTest()
        {
            var fromDate = new DateTime(2015, 1, 1);
            var toDate = new DateTime(2015, 1, 31);
            var url = string.Format("/api/v1/invoices/daterange/{0}/{1}", fromDate.ToString("yyyy-MM-dd"),
                                    toDate.ToString("yyyy-MM-dd"));
            var response = GetAsync<IEnumerable<FlightInvoiceDetails>>(url).Result;

            Assert.IsTrue(response.Any());
        }

        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void SetFlightAsInvoicedWebApiTest()
        {
            var fromDate = new DateTime(2015, 1, 1);
            var toDate = new DateTime(2015, 1, 31);
            var url = string.Format("/api/v1/invoices/daterange/{0}/{1}", fromDate.ToString("yyyy-MM-dd"),
                                    toDate.ToString("yyyy-MM-dd"));
            var response = GetAsync<IEnumerable<FlightInvoiceDetails>>(url).Result;

            Assert.IsTrue(response.Any());

            foreach (var flightInvoiceDetails in response)
            {
                var flightInvoiceBooking = new FlightInvoiceBooking
                    {
                        FlightId = flightInvoiceDetails.FlightId,
                        InvoiceNumber = "2015-01-001",
                        InvoiceDate = DateTime.Now
                    };

                var postResponce = PostAsync(flightInvoiceBooking, "/api/v1/invoices/invoiced").Result;

                Assert.IsTrue(postResponce.IsSuccessStatusCode);

            }
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/invoices"; }
        }
    }
}
