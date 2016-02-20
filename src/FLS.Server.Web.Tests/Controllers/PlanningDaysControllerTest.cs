using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.PlanningDay;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Web.Tests.Controllers
{
    [TestClass]
    public class PlanningDaysControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        public void GetPlanningDaysOverviewTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void GetPlanningDaysDetailsTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PlanningDayId;

            var result = GetAsync<PlanningDayDetails>("/api/v1/planningdays/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public void CreatePlanningDaysByRuleTest()
        {
            var locationResponse = GetAsync<IEnumerable<LocationOverview>>("/api/v1/locations").Result;
            Assert.IsTrue(locationResponse.Any());
            var id = locationResponse.First().LocationId;

            PlanningDayCreatorRule rule = new PlanningDayCreatorRule();
            rule.StartDate = new DateTime(2015, 4, 1);
            rule.EndDate = new DateTime(2015, 7, 14);
            rule.EverySaturday = true;
            rule.EverySunday = true;
            rule.LocationId = id;

            var response = PostAsync(rule, "/api/v1/planningdays/create/rule").Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return "/api/v1/planningdays"; }
        }
    }
}
