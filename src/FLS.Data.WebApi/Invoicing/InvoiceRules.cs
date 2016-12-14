using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing.RuleFilters;

namespace FLS.Data.WebApi.Invoicing
{
    public class InvoiceRules
    {
        public InvoiceRules()
        {
            InvoiceRecipientRuleFilters = new List<InvoiceRecipientRuleFilter>();
            InvoiceLineBaseRuleFilters = new List<BaseRuleFilter>();
        }

        public List<InvoiceRecipientRuleFilter> InvoiceRecipientRuleFilters { get; set; }

        public List<BaseRuleFilter> InvoiceLineBaseRuleFilters { get; set; }

        public override string ToString()
        {
            return $"InvoiceRules with {InvoiceLineBaseRuleFilters.Count} InvoiceLineBaseRuleFilters and {InvoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(AircraftRuleFilter)).Count()} AircraftRuleFilter";
        }
    }
}
