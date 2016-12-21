using FLS.Data.WebApi.Invoicing;

namespace FLS.Server.Service.Invoicing.RuleFilters
{
    public class InvoiceRecipientRuleFilterDetails : InvoiceRuleFilterDetails
    {
        public RecipientDetails RecipientTarget { get; set; }

        public bool IsInvoicedToClubInternal { get; set; }
    }
}