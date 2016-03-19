using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Emails;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class EmailTemplatesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetEmailTemplateOverviewWebApiTest()
        {
            InsertEmailTemplateDetails();
            var response = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetEmailTemplateDetailsWebApiTest()
        {
            InsertEmailTemplateDetails();

            LoginAsClubAdmin();
            var response = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().EmailTemplateId;

            var result = GetAsync<EmailTemplateDetails>("/api/v1/emailTemplates/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertEmailTemplateDetailsWebApiTest()
        {
            var emailTemplate = new EmailTemplateDetails();
            var ticks = DateTime.Now.Ticks;
            emailTemplate.EmailTemplateName = "EmailTemplate @ " + ticks;
            emailTemplate.EmailTemplateKeyName = "EmailTemplate" + ticks;
            emailTemplate.FromAddress = "test@glider-fls.ch";
            emailTemplate.ReplyToAddresses = "test@glider-fls.ch";
            emailTemplate.Description = "Dies ist ein Test-Template mit UnitTest erstellt";
            emailTemplate.Subject = "Test-Email-Template: " + ticks;
            emailTemplate.HtmlBody = "<html><body>Dies ist ein Text</body></html>";
            
            Assert.AreEqual(emailTemplate.Id, Guid.Empty);

            LoginAsSystemAdmin();
            var response = PostAsync(emailTemplate, "/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<EmailTemplateDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateEmailTemplateDetailsWebApiTest()
        {
            //insert new template first
            var emailTemplate = new EmailTemplateDetails();
            var ticks = DateTime.Now.Ticks;
            emailTemplate.EmailTemplateName = "EmailTemplate @ " + ticks;
            emailTemplate.EmailTemplateKeyName = "EmailTemplate" + ticks;
            emailTemplate.FromAddress = "test@glider-fls.ch";
            emailTemplate.ReplyToAddresses = "test@glider-fls.ch";
            emailTemplate.Description = "Dies ist ein Test-Template mit UnitTest erstellt";
            emailTemplate.Subject = "Test-Email-Template: " + ticks;
            emailTemplate.HtmlBody = "<html><body>Dies ist ein Text</body></html>";

            Assert.AreEqual(emailTemplate.Id, Guid.Empty);

            LoginAsSystemAdmin();
            var response = PostAsync(emailTemplate, "/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<EmailTemplateDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var overview = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(overview.Any());

            responseDetails.Description = responseDetails.Description + " Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(responseDetails, "/api/v1/emailTemplates/" + responseDetails.EmailTemplateId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            var overview2 = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(overview.Count() == overview2.Count());

            LoginAsClubAdmin();

            var overviewClub = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(overviewClub.Any());
            Assert.IsTrue(overview.Count() == overviewClub.Count());
            Assert.IsTrue(overviewClub.Any(q => q.EmailTemplateId == responseDetails.EmailTemplateId));

            var clubEmailTemplateDetails = GetAsync<EmailTemplateDetails>("/api/v1/emailTemplates/" + responseDetails.EmailTemplateId).Result;

            Assert.AreEqual(responseDetails.EmailTemplateId, clubEmailTemplateDetails.Id);

            clubEmailTemplateDetails.Description = "Club template";
            clubEmailTemplateDetails.HtmlBody = "Dies ist der Body des Club-Templates";

            var clubPutResult = PutAsync(clubEmailTemplateDetails, "/api/v1/emailTemplates/" + clubEmailTemplateDetails.EmailTemplateId).Result;

            Assert.IsTrue(clubPutResult.IsSuccessStatusCode);

            var overviewClub2 = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(overviewClub2.Any());
            Assert.IsTrue(overview.Count() == overviewClub2.Count()); //should still be the same count as the system template is replaced with the club related
            Assert.IsTrue(overviewClub2.Any(q => q.IsSystemTemplate == false));

        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeleteEmailTemplateDetailsWebApiTest()
        {
            LoginAsSystemAdmin();
            var response = GetAsync<IEnumerable<EmailTemplateOverview>>("/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().EmailTemplateId;

            var delResult = DeleteAsync("/api/v1/emailTemplates/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        private EmailTemplateDetails InsertEmailTemplateDetails()
        {
            var emailTemplate = new EmailTemplateDetails();
            var ticks = DateTime.Now.Ticks;
            emailTemplate.EmailTemplateName = "Template #" + ticks;
            emailTemplate.EmailTemplateKeyName = "Template" + ticks;
            emailTemplate.FromAddress = "test@glider-fls.ch";
            emailTemplate.ReplyToAddresses = "test@glider-fls.ch";
            emailTemplate.Description = "Dies ist ein Test-Template";
            emailTemplate.Subject = "Email-Template: " + ticks;
            emailTemplate.HtmlBody = "<html><body>Dies ist ein Text</body></html>";

            Assert.AreEqual(emailTemplate.Id, Guid.Empty);

            LoginAsSystemAdmin();
            var response = PostAsync(emailTemplate, "/api/v1/emailTemplates").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<EmailTemplateDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            return emailTemplate;
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/emailTemplates"; }
        }
    }
}
