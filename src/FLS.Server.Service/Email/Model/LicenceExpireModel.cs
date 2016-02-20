using System;

namespace FLS.Server.Service.Email.Model
{
    public class LicenceExpireModel
    {
        public string UnexpectedReturnAddress { get; set; }
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }

        public string LicenceName { get; set; }
        public string ExpireDate { get; set; }
    }
}
