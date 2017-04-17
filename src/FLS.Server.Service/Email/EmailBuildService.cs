using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using NLog;

namespace FLS.Server.Service.Email
{
    public class EmailBuildService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IEmailSendService _emailSendService;
        private readonly TemplateService _templateService;
        protected SystemData SystemData { get; set; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public EmailBuildService(DataAccessService dataAccessService, IEmailSendService emailSendService, 
            TemplateService templateService)
        {
            _dataAccessService = dataAccessService;
            _emailSendService = emailSendService;
            _templateService = templateService;

            using (var context = _dataAccessService.CreateDbContext())
            {
                SystemData = context.SystemDatas.FirstOrDefault();
            }
        }

        public void SendEmail(MailMessage eMailMessage)
        {
            try
            {
                _emailSendService.SendEmail(eMailMessage);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to send email. Error: {e.Message}");
                throw;
            }
        }

        public void SendSystemErrorEmail(string subject, string errorMessage, string stackTrace, string additionalInfo)
        {
            MailMessage message = new MailMessage(SystemData.SystemSenderEmailAddress,
                SystemData.SystemSenderEmailAddress);
            message.Subject = subject;
            message.Body =
                $"A system error occured in FLS.{Environment.NewLine}Error:{errorMessage}{Environment.NewLine}Additional information:{additionalInfo}{Environment.NewLine}Stacktrace:{stackTrace}.";

            SendEmail(message);
        }

        protected MailMessage BuildEmail(string templateName, MergedEmailFactory factory, 
            Dictionary<string, object> tokenValues, string recipientEmailAddress,
            Guid? clubId = null)
        {
            try
            {
                templateName.NotNullOrEmpty("templateName");

                var template = _templateService.GetEmailTemplate(templateName, clubId);

                if (template == null)
                {
                    Logger.Error($"Email-template with name: {templateName} not found!");
                    throw new ApplicationException($"Email-template with name: { templateName } not found!");
                }
                
                MailMessage message = factory
                    .WithTokenValues(tokenValues)
                    .WithSubject(template.Subject)
                    .WithHtmlBodyFromVelocity(template.HtmlBody)
                    .Create();

                message.From = new MailAddress(SystemData.SystemSenderEmailAddress);

                message.To.Add(recipientEmailAddress);

                return message;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while building email message. Exception-Message: {ex.Message}");
                throw;
            }
        }
    }
}
