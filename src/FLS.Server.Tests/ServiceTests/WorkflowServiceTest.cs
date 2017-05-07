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
            WorkflowService.ExecuteAircraftStatisticReportJob();
        }

        [TestMethod]
        [TestCategory("Service")]
        public void ExecuteDeliveryCreationJobTest()
        {
            WorkflowService.ExecuteDeliveryCreationJob();
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
            InsertPerson(person);

            //insert glider pilot with medical data expiring next 60 days
            personDetails = CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60); 
            person.GliderInstructorLicenceExpireDate = null;
            InsertPerson(person);

            //insert glider pilot with medical data expiring next 30 days
            personDetails = CreateGliderPilotPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(30);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(30);
            person.GliderInstructorLicenceExpireDate = null;
            InsertPerson(person);

            //insert glider instructor with medical data expiring next 60 days
            personDetails = CreateGliderInstructorPersonDetails(countryId);
            person = personDetails.ToPerson(CurrentIdentityUser.ClubId);
            person.MedicalLaplExpireDate = DateTime.Today.AddDays(60);
            person.MedicalClass1ExpireDate = null;
            person.MedicalClass2ExpireDate = DateTime.Today.AddDays(60);
            person.GliderInstructorLicenceExpireDate = DateTime.Today.AddDays(60);
            InsertPerson(person);

            WorkflowService.ExecuteLicenceNotificationJob();
        }
        
    }
}
