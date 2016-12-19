using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class InvoiceRecipientRuleFilterDetails : InvoiceRuleFilterDetails
    {
        public InvoiceRecipientRuleFilterDetails()
        {
        }

        public InvoiceRecipientRuleFilterDetails(InvoiceRuleFilterDetails baseInvoiceRuleFilterDetails, 
            RecipientDetails recipientTarget, bool isInvoicedToClubInternal)
            : base(baseInvoiceRuleFilterDetails)
        {
            RecipientTarget = recipientTarget;
            IsInvoicedToClubInternal = isInvoicedToClubInternal;
        }

        public RecipientDetails RecipientTarget { get; set; }

        public bool IsInvoicedToClubInternal { get; set; }
    }
}