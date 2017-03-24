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
    public class RegistrationEmailBuildService : EmailBuildService
    {
        public RegistrationEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }
        
        public MailMessage CreateTrialFlightRegistrationEmailForTrialPilot(Person trialPilotPerson, DateTime selectedDate, string emailRecipientAddress, Guid clubId, string locationName)
        {
            trialPilotPerson.ArgumentNotNull("trialPilotPerson");

            var model = new TrialFlightRegistrationModel()
            {
                RecipientName = trialPilotPerson.DisplayName,
                SelectedTrialFlightDate = selectedDate.ToShortDateString(),
                WillBeContactedOnDate = selectedDate.AddDays(-3).ToShortDateString(),
                LocationName = locationName
            };

            var factory = new MergedEmailFactory(new VelocityTemplateParser("TrialFlightRegistrationModel"));

            string messageSubject = "Email-Bestätigung für Schnupperflug-Registrierung";

            var tokenValues = new Dictionary<string, object>
                {
                    {"TrialFlightRegistrationModel", model}
                };

            return base.BuildEmail("TrialFlightRegistrationEmailForTrialPilot", factory, tokenValues, messageSubject, emailRecipientAddress.SanitizeEmailAddress(), clubId);

        }
    }
}
