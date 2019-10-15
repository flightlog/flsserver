using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using NLog;

namespace FLS.Server.Service.Email
{
    public class EmailSendService : IEmailSendService
    {
        protected SystemData SystemData { get; set; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public EmailSendService(DataAccessService dataAccessService)
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
                    if (SystemData.SendToBccRecipients &&
                        string.IsNullOrWhiteSpace(SystemData.BccRecipientEmailAddresses) == false)
                    {
                        eMailMessage.Bcc.Add(SystemData.BccRecipientEmailAddresses.FormatMultipleEmailAddresses());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Could not add BCC recipient email addresses. Error: {ex.Message}");
                }

                var mailClient = new SmtpClient(SystemData.SmtpServer, SystemData.SmtpPort)
                {
                    EnableSsl = SystemData.UseSSLforSmtpConnection
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

                if (SystemData.Testmode)
                {
                    //in TestMode we store emails locally in a directory
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

                    if (string.IsNullOrWhiteSpace(SystemData.TestmodeEmailPickupDirectory) == false)
                    {
                        mailClient.PickupDirectoryLocation = SystemData.TestmodeEmailPickupDirectory;
                    }
                    else
                    {
                        mailClient.PickupDirectoryLocation = @"c:\temp";
                    }
                }

                mailClient.Send(eMailMessage);

                if (SystemData.DebugMode || SystemData.Testmode)
                {
                    //log email body in debug mode
                    Logger.Info(
                        $"Sent email to recipient: {eMailMessage.To} with subject: {eMailMessage.Subject}, and Body: {eMailMessage.Body}");
                }
                else
                {
                    Logger.Info($"Sent email to recipient: {eMailMessage.To} with subject: {eMailMessage.Subject}");
                }
            }
            catch (SmtpFailedRecipientsException recipientsException)
            {
                Logger.Error(recipientsException,
                    $"Failed recipients status code error {recipientsException.StatusCode} while trying to send email. Message: {recipientsException.Message}");
                throw;
            }
            catch (SmtpException smtpException)
            {
                Logger.Error(smtpException,
                    $"SMTP-Error {smtpException.StatusCode} while trying to send email. SMTP-Message: {smtpException.Message}");
                throw;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to send email. Error: {e.Message}");
                throw;
            }
        }
    }
}
