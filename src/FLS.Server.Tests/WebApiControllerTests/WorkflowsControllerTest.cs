using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class WorkflowsControllerTest : BaseAuthenticatedTests
    {
        protected override void PostSetup(TestServer server)
        {
            LoginAsWorkflow();
        }

        [TestMethod]
        public void ExecuteWorkflowsWebApiTest()
        {
            var response = GetAsync(RoutePrefix).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteDailyReportJobWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "dailyreports").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteMonthlyReportJobWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "monthlyreports").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecutePreMonthlyReportJobWebApiTest()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2);
            var response = GetAsync(RoutePrefix + "monthlyreports/" + startDate.Year + "/" + startDate.Month).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteFlightValidationWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "flightvalidation").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecutePlanningDayMailsWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "planningdaymails").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteTestMailWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "testmails").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteDeliveryCreationJobWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "deliverycreation").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ExecuteDeliveryMailExportJobWebApiTest()
        {
            var response = GetAsync(RoutePrefix + "deliverymailexport").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/workflows/"; }
        }
    }
}
