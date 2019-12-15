using System;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class WorkflowServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDailyFlightValidationJobTest()
        {
            WorkflowService.ExecuteDailyFlightValidationJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDailyReportJobTest()
        {
            WorkflowService.ExecuteDailyReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteAircraftStatisticReportJobTest()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month -1, 1);
            var endDate = startDate.AddMonths(1).AddTicks(-1);
            //WorkflowService.ExecuteAircraftStatisticReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDeliveryCreationJobTest()
        {
            WorkflowService.ExecuteDeliveryCreationJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteAircraftDatabaseSyncJobTest()
        {
            WorkflowService.ExecuteAircraftDatabaseSyncJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDeliveryMailExportJobTest()
        {
            WorkflowService.ExecuteDeliveryMailExportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecutePlanningDayMailJobTest()
        {
            var planningDay = new PlanningDayDetails();
            planningDay.Day = DateTime.Now.Date.AddDays(7);
            planningDay.LocationId = GetFirstLocation().LocationId;
            planningDay.Remarks = "Test";
            planningDay.FlightOperatorPersonId = GetFirstPerson().PersonId;
            planningDay.TowingPilotPersonId = GetFirstPerson().PersonId;
            planningDay.InstructorPersonId = GetFirstGliderInstructorPerson(CurrentIdentityUser.ClubId).PersonId;
            PlanningDayService.InsertPlanningDayDetails(planningDay);

            WorkflowService.ExecutePlanningDayMailJob();
        }
        
        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteLicenceNotificationJob()
        {
            var countryId = GetCountry("CH").CountryId;

            //insert glider pilot without medical data
            var personDetails = CreateGliderPilotPersonDetails(countryId);
            var person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = null;
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = null;
            person.GliderInstructorLicenceExpireDate = null;
            person.MotorInstructorLicenceExpireDate = null;
            person.PartMLicenceExpireDate = null;
            InsertPerson(person);

            //insert glider pilot with medical data expiring next 60 days
            personDetails = CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60); 
            person.GliderInstructorLicenceExpireDate = null;
            person.MotorInstructorLicenceExpireDate = null;
            person.PartMLicenceExpireDate = null;
            InsertPerson(person);

            //insert glider pilot with medical data expiring next 30 days
            personDetails = CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(30);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(30);
            person.GliderInstructorLicenceExpireDate = null;
            person.MotorInstructorLicenceExpireDate = null;
            person.PartMLicenceExpireDate = null;
            InsertPerson(person);

            //insert glider instructor with medical data expiring next 60 days
            personDetails = CreateGliderInstructorPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60);
            person.GliderInstructorLicenceExpireDate = DateTime.Today.AddDays(60);
            person.MotorInstructorLicenceExpireDate = null;
            person.PartMLicenceExpireDate = null;
            InsertPerson(person);

            //insert motor instructor with expiring next 60 days
            personDetails = CreateGliderInstructorPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = null;
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = null;
            person.GliderInstructorLicenceExpireDate = null;
            person.MotorInstructorLicenceExpireDate = DateTime.Today.AddDays(60);
            person.PartMLicenceExpireDate = null;
            InsertPerson(person);

            //insert part M with expiring next 60 days
            personDetails = CreateGliderInstructorPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = null;
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = null;
            person.GliderInstructorLicenceExpireDate = null;
            person.MotorInstructorLicenceExpireDate = null;
            person.PartMLicenceExpireDate = DateTime.Today.AddDays(60);
            InsertPerson(person);

            WorkflowService.ExecuteLicenceNotificationJob();
        }
        
    }
}
