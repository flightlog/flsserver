using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class ActionFilterTest : BaseAuthenticatedTests
    {
        private AircraftHelper _aircraftHelper;
        
        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize: ActionFilterTest.TestInitialize()");
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();
        }

        [TestMethod]
        [TestCategory("ActionFilters")]
        public void ActionFilterValidateModelStateWebApiTest()
        {
            var aircraftDetails = _aircraftHelper.CreateGliderAircraftDetails(1);
            //make model invalid
            aircraftDetails.Immatriculation = string.Empty;
            aircraftDetails.CompetitionSign = "more than 5 characters";

            var response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            var responseContent = response.Content.ReadAsStringAsync().Result;

        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/aircrafts"; }
        }
    }
}
