using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using NLog;

namespace FLS.Server.Tests.Mocks.Services
{
    public class MockEmailSendService : IEmailSendService
    {
        protected SystemData SystemData { get; set; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public MockEmailSendService(DataAccessService dataAccessService)
        {
            using (var context = dataAccessService.CreateDbContext())
            {
                SystemData = context.SystemDatas.FirstOrDefault();
            }
        }

        public void SendEmail(MailMessage eMailMessage)
        {
            try
            {
                SystemData.NotNull("SystemData");
                eMailMessage.ArgumentNotNull("eMailMessage");

                if (eMailMessage.From == null || string.IsNullOrWhiteSpace(eMailMessage.From.Address))
                {
                    eMailMessage.From = new MailAddress(SystemData.SystemSenderEmailAddress);
                }
                
                try
                {
                    if (SystemData.SendToBccRecipients && string.IsNullOrWhiteSpace(SystemData.BccRecipientEmailAddresses) == false)
                    {
                        eMailMessage.Bcc.Add(SystemData.BccRecipientEmailAddresses.FormatMultipleEmailAddresses());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Could not add BCC recipient email addresses. Error: {ex.Message}");
                }

                //http://stackoverflow.com/questions/1264672/how-to-save-mailmessage-object-to-disk-as-eml-or-msg-file
                var mailClient = new SmtpClient(SystemData.SmtpServer, SystemData.SmtpPort)
                {
                    EnableSsl = SystemData.UseSSLforSmtpConnection,
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory
                };

                if (SystemData.UseSmtpAuthentication)
                {
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = new NetworkCredential(SystemData.SmtpUsername, SystemData.SmtpPassword);
                }
                else
                {
                    mailClient.UseDefaultCredentials = true;
                }

                if (string.IsNullOrWhiteSpace(SystemData.TestmodeEmailPickupDirectory) == false)
                {
                    mailClient.PickupDirectoryLocation = SystemData.TestmodeEmailPickupDirectory;
                }
                else
                {
                    mailClient.PickupDirectoryLocation = @"c:\temp";
                }

                //we store the message in c:\temp instead of sending it over network
                mailClient.Send(eMailMessage);

                Logger.Info($"Dummy-Sent email to recipient: {eMailMessage.To} with subject: {eMailMessage.Subject}, and Body: {eMailMessage.Body}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to send email. Error: {e.Message}");
                throw;
            }
        }
    }
}
