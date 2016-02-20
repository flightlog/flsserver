using System;
using System.Linq;
using FLS.Data.WebApi.Dashboard;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class DashboardsControllerTest : BaseAuthenticatedTests
    {
        private FlightHelper _flightHelper;
        private PersonHelper _personHelper;
        private ClubHelper _clubHelper;
        private AircraftHelper _aircraftHelper;
        private LocationHelper _locationHelper;
        private DataAccessService _dataAccessService;

        [TestInitialize]
        public void TestInitialize()
        {
            _flightHelper = UnityContainer.Resolve<FlightHelper>();
            _personHelper = UnityContainer.Resolve<PersonHelper>();
            _clubHelper = UnityContainer.Resolve<ClubHelper>();
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
            _dataAccessService = UnityContainer.Resolve<DataAccessService>();
            _flightHelper.SetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetDashboardsWebApiTest()
        {
            var flight = _flightHelper.CreateGliderFlight(ClubId, DateTime.UtcNow.AddMonths(-2));
            var flight2 = _flightHelper.CreateGliderFlight(ClubId, DateTime.UtcNow.AddDays(-3));

            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.Pilot);

            using (var context = _dataAccessService.CreateDbContext())
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
