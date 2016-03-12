using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Service.Email.Model;

namespace FLS.Server.Service.Email
{
    public class InvoiceEmailBuildService : EmailBuildService
    {
        public InvoiceEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }

        internal MailMessage CreateNoInvoiceEmail(string recipientMailAddresses, DateTime from, DateTime to)
        {
            recipientMailAddresses.ArgumentNotNull("recipients");

            var message = new MailMessage();

            message.Subject = $"Keine Flüge für die Verrechnung von FLS zwischen {from.ToShortDateString()} und {to.ToShortDateString()} ";

            message.From = new MailAddress(SystemData.SystemSenderEmailAddress);    

            message.To.Add(recipientMailAddresses.FormatMultipleEmailAddresses());

            message.Body = "Es sind keine Flüge verfügbar für die Verrechnung.";

            return message;
        }

        internal MailMessage CreateInvoiceEmail(string recipientMailAddresses, byte[] attachmentBytes, DateTime from, DateTime to)
        {
            recipientMailAddresses.ArgumentNotNull("recipients");
            
            var message = new MailMessage();

            message.Subject = $"Rechnungs-Export von FLS zwischen {from.ToShortDateString()} und {to.ToShortDateString()}";
            message.From = new MailAddress(SystemData.SystemSenderEmailAddress);

            message.To.Add(recipientMailAddresses.FormatMultipleEmailAddresses());
            var attachment = new Attachment(attachmentBytes.ToMemoryStream(),
                $"Segelflug-Rechnungen {DateTime.Today.ToString("yyyy-MM-dd")}.zip", MediaTypeNames.Application.Zip);
            message.Attachments.Add(attachment);

            message.Body = "Die Flüge für die Verrechnung sind in der angehängten ZIP-Datei.";

            return message;
        }
    }
}
