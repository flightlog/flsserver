using System;
using System.Collections.Generic;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class FlightTypeToInvoiceRecipientMappingRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Dictionary<string, InvoiceRecipientTarget> _invoiceRecipientTargets;
        private readonly string _flightCode;

        internal FlightTypeToInvoiceRecipientMappingRule(Dictionary<string, InvoiceRecipientTarget> invoiceRecipientTargets, string flightCode)
        {
            _invoiceRecipientTargets = invoiceRecipientTargets;
            _flightCode = flightCode;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new ContainsKey<string, InvoiceRecipientTarget>(_invoiceRecipientTargets, _flightCode));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            var invoiceRecipientTarget = _invoiceRecipientTargets[_flightCode];

            if (invoiceRecipientTarget != null)
            {
                flightInvoiceDetails.RecipientDetails.RecipientName = invoiceRecipientTarget.DisplayName;
                flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber = invoiceRecipientTarget.MemberNumber;

                //invoice is created for club internal (customer already paid)
                flightInvoiceDetails.IsInvoicedToClubInternal = true;
            }
            else
            {
                throw new Exception($"Invoice recipient target is null. Can not create invoice for flight with Id: {flightInvoiceDetails.FlightId}");
            }

            return base.Apply(flightInvoiceDetails);
        }
    }
}
