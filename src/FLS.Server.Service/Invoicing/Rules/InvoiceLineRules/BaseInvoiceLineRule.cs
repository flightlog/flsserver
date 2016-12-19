using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using NLog;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal abstract class BaseInvoiceLineRule : BaseInvoiceRule
    {
        protected InvoiceLineRuleFilterDetails InvoiceLineRuleFilterDetails
        {
            get
            {
                return BaseInvoiceRuleFilter as InvoiceLineRuleFilterDetails;
            }
        }

        internal BaseInvoiceLineRule(Flight flight, InvoiceLineRuleFilterDetails invoiceLineRuleFilterDetails)
            : base(flight, invoiceLineRuleFilterDetails)
        {
        }
    }
}
