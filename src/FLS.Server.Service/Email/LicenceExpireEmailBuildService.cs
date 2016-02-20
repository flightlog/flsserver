using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email.Model;
using NLog;

namespace FLS.Server.Service.Email
{
    public class LicenceExpireEmailBuildService : EmailBuildService
    {
        public LicenceExpireEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }

        public MailMessage CreateLicenceExpireEmail(Person person, string licenceName, DateTime licenceExpireDate)
        {
            person.ArgumentNotNull("person");

            var licenceExpireModel = new LicenceExpireModel()
            {
                RecipientName = person.DisplayName,
                FLSUrl = SystemData.BaseURL,
                UnexpectedReturnAddress = SystemData.SystemSenderEmailAddress,
                LicenceName = licenceName,
                ExpireDate = licenceExpireDate.ToShortDateString()
            };

            var factory = new MergedEmailFactory(new VelocityTemplateParser("LicenceExpireModel"));

            string messageSubject = "Lizenz läuft bald ab";

            var tokenValues = new Dictionary<string, object>
                {
                    {"LicenceExpireModel", licenceExpireModel}
                };

            return base.BuildEmail("licenceexpiressoon", factory, tokenValues, messageSubject, person.EmailAddressForCommunication.SanitizeEmailAddress());
        }
    }
}
