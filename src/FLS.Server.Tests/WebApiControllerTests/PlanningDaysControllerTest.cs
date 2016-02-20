﻿using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class PlanningDaysControllerTest : BaseAuthenticatedTests
    {
        private LocationHelper _locationHelper;
        private PersonHelper _personHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _personHelper = UnityContainer.Resolve<PersonHelper>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPlanningDaysOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPlanningDaysDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PlanningDayId;

            var result = GetAsync<PlanningDayDetails>("/api/v1/planningdays/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPlanningDaysByRuleWebApiTest()
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

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPlanningDayWebApiTest()
        {
            var planningDay = new PlanningDayDetails();
            planningDay.Day = DateTime.Now.Date.AddDays(1);
            planningDay.LocationId = _locationHelper.GetFirstLocation().LocationId;
            planningDay.Remarks = "Test";
            planningDay.FlightOperatorPersonId = _personHelper.GetFirstPerson().PersonId;
            planningDay.TowingPilotPersonId = _personHelper.GetFirstPerson().PersonId;
            planningDay.InstructorPersonId = _personHelper.GetFirstGliderInstructorPerson(ClubId).PersonId;

            var response = PostAsync(planningDay, "/api/v1/planningdays").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PlanningDayDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdatePlanningDayDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PlanningDayId;

            var result = GetAsync<PlanningDayDetails>("/api/v1/planningdays/" + id).Result;

            Assert.AreEqual(id, result.Id);

            var person = _personHelper.GetDifferentPerson(result.FlightOperatorPersonId);
            Assert.IsNotNull(person);
            result.FlightOperatorPersonId = person.PersonId;

            person = _personHelper.GetDifferentPerson(result.TowingPilotPersonId);
            Assert.IsNotNull(person);
            result.TowingPilotPersonId = person.PersonId;
            result.Remarks = DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(result, "/api/v1/planningdays/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeletePlanningDayWebApiTest()
        {
            var response = GetAsync<IEnumerable<PlanningDayOverview>>("/api/v1/planningdays").Result;

            Assert.IsTrue(response.Any());

            var id = response.Last().PlanningDayId;

            var delResult = DeleteAsync("/api/v1/planningdays/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/planningdays"; }
        }
    }
}
