using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.System
{
    public class SystemDataDetails : FLSBaseData
    {
        public Guid SystemDataId { get; set; }

        [Required]
        [StringLength(250)]
        public string BaseURL { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportSenderEmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string SystemSenderEmailAddress { get; set; }

        [Required]
        public bool UseSmtpAuthentication { get; set; }

        [Required]
        public bool UseSSLforSmtpConnection { get; set; }
        
        [StringLength(100)]
        public string SmtpUsername { get; set; }

        [StringLength(100)]
        public string SmtpPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public int MaxUserLoginAttempts { get; set; }

        [Required]
        public bool Testmode { get; set; }


        [StringLength(100)]
        public string TestmodeEmailPickupDirectory { get; set; }

        [Required]
        public bool DebugMode { get; set; }

        [Required]
        public bool SendToBccRecipients { get; set; }

        [StringLength(250)]
        public string BccRecipientEmailAddresses { get; set; }

        public override Guid Id
        {
            get { return SystemDataId; }
            set { SystemDataId = value; }
        }
    }
}
