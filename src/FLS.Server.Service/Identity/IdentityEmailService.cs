using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FLS.Common.Validators;
using FLS.Server.Service.Email;
using Microsoft.AspNet.Identity;

namespace FLS.Server.Service.Identity
{
    public class IdentityEmailService : IIdentityMessageService
    {
        private readonly IEmailSendService _emailSendService;

        public IdentityEmailService(IEmailSendService emailSendService)
        {
            _emailSendService = emailSendService;
        }

        public Task SendAsync(IdentityMessage message)
        {
            message.ArgumentNotNull("message");

            //TODO: Integrate HTML view into EmailBuildService
            ////http://www.asp.net/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity
            //#region formatter
            //string text = $"Please click on this link to {message.Subject}: {message.Body}";
            //string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            //html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
            //#endregion

            //MailMessage msg = new MailMessage();
            //msg.From = new MailAddress("joe@contoso.com");
            //msg.To.Add(new MailAddress(message.Destination));
            //msg.Subject = message.Subject;
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            var mailMessage = new MailMessage();
            mailMessage.To.Add(message.Destination);
            mailMessage.Subject = message.Subject;
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, Encoding.UTF8, MediaTypeNames.Text.Html));
            //            mailMessage.Body = message.Body;
            _emailSendService.SendEmail(mailMessage);

            return Task.FromResult(0);
        }
    }
}
