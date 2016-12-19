using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class InvoiceRecipientRule : BaseInvoiceRule
    {
        private readonly InvoiceRuleFilterDetails _invoiceRecipientRuleFilter;

        internal InvoiceRecipientRule(Flight flight, InvoiceRuleFilterDetails invoiceRecipientRuleFilter)
            : base(flight, invoiceRecipientRuleFilter)
        {
            _invoiceRecipientRuleFilter = invoiceRecipientRuleFilter;
        }
        
        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (InvoiceRuleFilter != null && _invoiceRecipientRuleFilter.RecipientTarget != null)
            {
                flightInvoiceDetails.RecipientDetails.RecipientName = _invoiceRecipientRuleFilter.RecipientTarget.RecipientName;
                flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber = _invoiceRecipientRuleFilter.RecipientTarget.PersonClubMemberNumber;

                //invoice is created for club internal (customer already paid)
                flightInvoiceDetails.IsInvoicedToClubInternal = _invoiceRecipientRuleFilter.IsInvoicedToClubInternal;
            }
            else
            {
                throw new Exception($"Invoice recipient target is null. Can not create invoice for flight with Id: {flightInvoiceDetails.FlightId}");
            }

            return base.Apply(flightInvoiceDetails);
        }
    }
}
