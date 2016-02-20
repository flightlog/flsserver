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

        protected MailMessage BuildEmail(string templateName, MergedEmailFactory factory, 
            Dictionary<string, object> tokenValues, string messageSubject, string recipientEmailAddress,
            Guid? clubId = null)
        {
            try
            {
                templateName.NotNullOrEmpty("templateName");

                var template = _templateService.GetEmailTemplate(templateName, clubId);
                
                MailMessage message = factory
                    .WithTokenValues(tokenValues)
                    .WithHtmlBodyFromVelocity(template.HtmlBody)
                    .Create();

                message.Subject = messageSubject;
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
