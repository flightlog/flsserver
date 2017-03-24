using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Registrations;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class RegistrationsControllerTest : BaseServerTest
    {

        [TestMethod]
        [TestCategory("WebApi")]
        public void TrialFlightRegistrationDetailsWebApiTest()
        {
            var dates = GetAsync<IEnumerable<DateTime>>("/api/v1/trialflightsregistrations/availabledates/fgzo").Result;

            Assert.IsTrue(dates.Any());

            var registration = new TrialFlightRegistrationDetails()
            {
                Lastname = "Schulthess",
                Firstname = "Didier",
                AddressLine1 = "Hauptstrasse 15",
                ZipCode = "8888",
                City = "Hauptstadt",
                PrivateEmail = "didier@pilot.ch",
                InvoiceAddressIsSame = true,
                SelectedDay = dates.First(),
                ClubKey = "FGZO"
            };
            
            var response = PostAsync(registration, "/api/v1/trialflightsregistrations").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
        }
        
        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/trialflightsregistrations"; }
        }
    }
}
