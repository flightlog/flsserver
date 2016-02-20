using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FLS.Server.Service.Identity
{
    /// <summary>
    /// <see cref="https://code.google.com/p/sharp-voice/wiki/Examples"/>
    /// <seealso cref="https://github.com/StephenPAdams/NVoice"/>
    /// </summary>
    public class GoogleSmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            //You need to make sure your google account is signed up 
            //SharpVoice.Voice v = new SharpVoice.Voice("YourAccount@gmail.com", "YourPassword");
            //v.SendSMS(message.Destination, message.Body);
            return Task.FromResult(0);
        }
    }
}
