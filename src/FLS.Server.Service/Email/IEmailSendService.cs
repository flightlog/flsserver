using System.Net.Mail;

namespace FLS.Server.Service.Email
{
    public interface IEmailSendService
    {
        void SendEmail(MailMessage eMailMessage);
    }
}
