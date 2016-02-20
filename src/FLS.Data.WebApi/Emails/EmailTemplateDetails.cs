using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Emails
{
    public class EmailTemplateDetails : FLSBaseData
    {
        public Guid EmailTemplateId { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailTemplateName { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailTemplateKeyName { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(256)]
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the emails reply to addresses as comma-separated string (if multiple)
        /// </summary>
        [StringLength(256)]
        public string ReplyToAddresses { get; set; }

        [Required]
        [StringLength(256)]
        public string Subject { get; set; }


        public string HtmlBody { get; set; }

        public string TextBody { get; set; }

        public override Guid Id
        {
            get { return EmailTemplateId; }
            set { EmailTemplateId = value; }
        }
    }
}
