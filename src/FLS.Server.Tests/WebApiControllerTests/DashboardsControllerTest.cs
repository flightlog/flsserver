using System;
using System.Linq;
using FLS.Data.WebApi.Dashboard;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class DashboardsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetDashboardsWebApiTest()
        {
            var flight = CreateGliderFlight(ClubId, DateTime.UtcNow.AddMonths(-2));
            var flight2 = CreateGliderFlight(ClubId, DateTime.UtcNow.AddDays(-3));

            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.Pilot);

            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == MyUserDetails.UserId);
                Assert.IsNotNull(user);
                user.PersonId = flight.Pilot.PersonId;

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }

            var response = GetAsync<DashboardDetails>(Uri).Result;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.SafetyDashboardDetails);
            Assert.IsNotNull(response.GliderPilotFlightStatisticDashboardDetails);
            Assert.IsNotNull(response.MotorPilotFlightStatisticDashboardDetails);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/dashboards"; }
        }
    }
}
