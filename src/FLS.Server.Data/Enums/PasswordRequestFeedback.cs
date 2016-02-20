using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLS.Server.Data.Enums
{
    public enum PasswordRequestFeedback
    {
        OK = 0,
        UserNotFound = -1,
        MailSendingError = -2
    }
}
