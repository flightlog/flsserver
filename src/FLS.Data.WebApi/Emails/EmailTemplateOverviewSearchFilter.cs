using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Emails
{
    public class EmailTemplateOverviewSearchFilter
    {
        public string EmailTemplateName { get; set; }

        public string Description { get; set; }

        public bool? IsSystemTemplate { get; set; }

        public bool? IsCustomizable { get; set; }
    }
}
