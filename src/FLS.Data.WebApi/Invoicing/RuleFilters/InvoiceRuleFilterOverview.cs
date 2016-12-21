using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class InvoiceRuleFilterOverview : FLSBaseData
    {

        public Guid InvoiceRuleFilterId { get; set; }

        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public int InvoiceRuleFilterTypeId { get; set; }
        

        public override Guid Id
        {
            get { return InvoiceRuleFilterId; }
            set { InvoiceRuleFilterId = value; }
            
        }
    }
}