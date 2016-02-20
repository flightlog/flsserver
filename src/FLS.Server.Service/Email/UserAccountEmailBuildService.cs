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
    public class UserAccountEmailBuildService : EmailBuildService
    {
        public UserAccountEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }

        public MailMessage CreateLostPasswordResetEmail(User user, string lostPasswordResetUrl)
        {
            user.ArgumentNotNull("user");

            var lostPasswordResetModel = new LostPasswordResetModel()
            {
                RecipientName = user.FriendlyName,
                FLSUrl = SystemData.BaseURL,
                UnexpectedReturnAddress = SystemData.SystemSenderEmailAddress,
                LostPasswordResetUrl = lostPasswordResetUrl
            };

            var factory = new MergedEmailFactory(new VelocityTemplateParser("LostPasswordResetModel"));

            string messageSubject = "Passwort-Reset für Flight Logging System Zugang";

            var tokenValues = new Dictionary<string, object>
                {
                    {"LostPasswordResetModel", lostPasswordResetModel}
                };

            return base.BuildEmail("lostpassword", factory, tokenValues, messageSubject, user.NotificationEmail.SanitizeEmailAddress(), user.ClubId);
        }

        public MailMessage CreateEmailConfirmationEmail(User user, string emailConfirmationUrl)
        {
            user.ArgumentNotNull("user");

            var emailConfirmationModel = new EmailConfirmationModel()
            {
                Username = user.UserName,
                RecipientName = user.FriendlyName,
                FLSUrl = SystemData.BaseURL,
                UnexpectedReturnAddress = SystemData.SystemSenderEmailAddress,
                EmailConfirmationUrl = emailConfirmationUrl
            };

            var factory = new MergedEmailFactory(new VelocityTemplateParser("EmailConfirmationModel"));

            string messageSubject = "Email-Bestätigung für neues Benutzerkonto im Flight Logging System";

            var tokenValues = new Dictionary<string, object>
                {
                    {"EmailConfirmationModel", emailConfirmationModel}
                };

            return base.BuildEmail("emailconfirmation", factory, tokenValues, messageSubject, user.NotificationEmail.SanitizeEmailAddress(), user.ClubId);

        }
    }
}
