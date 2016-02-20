using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FLS.Common.Extensions;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class WorkflowServiceTest : BaseServiceTest
    {
        private TestContext _testContextInstance;
        private UserService _userService;
        private FlightHelper _flightHelper;
        private PersonHelper _personHelper;
        private LocationHelper _locationHelper;
        private IdentityService _identityService;
        private WorkflowService _workflowService;

        [TestInitialize]
        public void TestInitialize()
        {
            _flightHelper = UnityContainer.Resolve<FlightHelper>();
            _personHelper = UnityContainer.Resolve<PersonHelper>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
            _userService = UnityContainer.Resolve<UserService>();
            _workflowService = UnityContainer.Resolve<WorkflowService>();

            var user = _userService.GetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
            Assert.IsNotNull(user);
            _identityService = UnityContainer.Resolve<IdentityService>();
            _identityService.SetUser(user);

            _flightHelper.CreateFlightsForInvoicingTests(user.ClubId);
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
        public void ExecuteDailyFlightValidationJobTest()
        {
            _workflowService.ExecuteDailyFlightValidationJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDailyReportJobTest()
        {
            _workflowService.ExecuteDailyReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteAircraftStatisticReportJobTest()
        {
            _workflowService.ExecuteAircraftStatisticReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecutePlanningDayMailJobTest()
        {
            _workflowService.ExecutePlanningDayMailJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteInvoiceReportJobTest()
        {
            _workflowService.ExecuteInvoiceReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteLicenceNotificationJob()
        {
            var countryId = _locationHelper.GetCountry("CH").CountryId;

            //insert glider pilot without medical data
            var personDetails = _personHelper.CreateGliderPilotPersonDetails(countryId);
            var person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = null;
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = null;
            person.GliderInstructorLicenceExpireDate = null;
            _personHelper.InsertPerson(person);

            //insert glider pilot with medical data expiring next 60 days
            personDetails = _personHelper.CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60); 
            person.GliderInstructorLicenceExpireDate = null;
            _personHelper.InsertPerson(person);

            //insert glider pilot with medical data expiring next 30 days
            personDetails = _personHelper.CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(30);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(30);
            person.GliderInstructorLicenceExpireDate = null;
            _personHelper.InsertPerson(person);

            //insert glider instructor with medical data expiring next 60 days
            personDetails = _personHelper.CreateGliderInstructorPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60);
            person.GliderInstructorLicenceExpireDate = DateTime.Today.AddDays(60);
            _personHelper.InsertPerson(person);

            _workflowService.ExecuteLicenceNotificationJob();
        }
        
    }
}
