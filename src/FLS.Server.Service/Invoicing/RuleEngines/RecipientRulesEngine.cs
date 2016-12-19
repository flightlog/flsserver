using System.Collections.Generic;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Invoicing.Rules;
using FLS.Server.Service.RulesEngine;

namespace FLS.Server.Service.Invoicing.RuleEngines
{
    internal class RecipientRulesEngine
    {
        private readonly RuleBasedFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;
        private readonly IPersonService _personService;
        private readonly List<InvoiceRuleFilterDetails> _invoiceRecipientRuleFilters;

        public RecipientRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight, 
            IPersonService personService, List<InvoiceRuleFilterDetails> invoiceRecipientRuleFilters)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _invoiceRecipientRuleFilters = invoiceRecipientRuleFilters;
            _flight = flight;
            _personService = personService;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            var rules = new List<IRule<RuleBasedFlightInvoiceDetails>>();

            foreach (var invoiceRecipientRuleFilter in _invoiceRecipientRuleFilters)
            {
                var rule = new InvoiceRecipientRule(_flight, invoiceRecipientRuleFilter);
                rule.StopRuleEngineWhenRuleApplied = true;
                rules.Add(rule);
            }

            rules.Add(new FlightCostPaidByPersonRule(_flight, _personService));
            rules.Add(new FlightCostPaidByPilotRule(_flight, _personService));

            _flightInvoiceDetails.ApplyRules(rules);

            return _flightInvoiceDetails;
        }
    }
}
