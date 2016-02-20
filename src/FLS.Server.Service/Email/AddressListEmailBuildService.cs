using System.Net.Mail;
using AutoMapper.QueryableExtensions.Impl;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Email
{
    public class AddressListEmailBuildService : EmailBuildService
    {
        public AddressListEmailBuildService(DataAccessService dataAccessService, IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }

        public MailMessage CreateAddressListEmail(User user, byte[] excel)
        {
            user.ArgumentNotNull("user");

            MailMessage message = new MailMessage();
            message.From = new MailAddress("fls@glider-fls.ch");
            message.To.Add(user.NotificationEmail.SanitizeEmailAddress());
            message.Subject = "Adressliste";
            message.Body = "Anbei die Adressliste im Anhang des Emails";
            Attachment attachment = new Attachment(excel.ToMemoryStream(), "Addresslist.xlsx", "application/vnd.ms-excel");
            message.Attachments.Add(attachment);

            return message;
        }
    }
}
