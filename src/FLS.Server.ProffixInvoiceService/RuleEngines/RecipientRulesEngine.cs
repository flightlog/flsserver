using System.Collections.Generic;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.RuleFilters;
using FLS.Server.ProffixInvoiceService.Rules;

namespace FLS.Server.ProffixInvoiceService.RuleEngines
{
    internal class RecipientRulesEngine
    {
        private readonly ProffixFlightInvoiceDetails _flightInvoiceDetails;
        private readonly InvoiceMapping _invoiceMapping;
        private readonly Flight _flight;
        private readonly IPersonService _personService;

        public RecipientRulesEngine(ProffixFlightInvoiceDetails flightInvoiceDetails, InvoiceMapping invoiceMapping, Flight flight, IPersonService personService)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _invoiceMapping = invoiceMapping;
            _flight = flight;
            _personService = personService;
        }

        public ProffixFlightInvoiceDetails Run()
        {
            var flightTypeToRecipientMappingRule = new FlightTypeToInvoiceRecipientMappingRule(_invoiceMapping.FlightTypeToInvoiceRecipientMapping,
                _flight.FlightType.FlightCode);
            flightTypeToRecipientMappingRule.StopRuleEngineWhenRuleApplied = true;

            var rules = new List<IRule<ProffixFlightInvoiceDetails>>();
            rules.Add(flightTypeToRecipientMappingRule);
            rules.Add(new FlightCostPaidByPersonRule(_flight, _personService));
            rules.Add(new FlightCostPaidByPilotRule(_flight, _personService));

            _flightInvoiceDetails.ApplyRules(rules);

            return _flightInvoiceDetails;
        }
    }
}
