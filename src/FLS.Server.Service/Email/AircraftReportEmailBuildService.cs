using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Service.Email.Model;

namespace FLS.Server.Service.Email
{
    public class AircraftReportEmailBuildService : EmailBuildService
    {
        public AircraftReportEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }
        internal MailMessage CreateAircraftStatisticInformationEmail(AircraftFlightReport reportData, string recipientMailAddresses)
        {
            reportData.ArgumentNotNull("reportData");
            recipientMailAddresses.ArgumentNotNull("recipients");
            
            var factory = new MergedEmailFactory(new VelocityTemplateParser("EmailAircraftFlightReport"));

            var report = new EmailAircraftFlightReport(reportData);
            report.SenderName = SystemData.SystemSenderEmailAddress;
            report.FLSUrl = SystemData.BaseURL;
            //report.RecipientName = recipients.First().DisplayName;

            string messageSubject = "Flugzeug-Statistik-Report";

            var tokenValues = new Dictionary<string, object>
                {
                    {"EmailAircraftFlightReport", report}
                };

            return base.BuildEmail("aircraftstatisticreport", factory, tokenValues, messageSubject, 
                recipientMailAddresses.FormatMultipleEmailAddresses());
        }
    }
}
