using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Emails
{
    public class EmailTemplateOverview : FLSBaseData
    {
        public Guid EmailTemplateId { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailTemplateName { get; set; }

        public string Description { get; set; }

        public bool IsSystemTemplate { get; set; }

        public bool IsCustomizable { get; set; }

        public override Guid Id
        {
            get { return EmailTemplateId; }
            set { EmailTemplateId = value; }
        }
    }
}
