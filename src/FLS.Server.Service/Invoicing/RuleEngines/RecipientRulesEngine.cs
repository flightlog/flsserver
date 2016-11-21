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
        private readonly Dictionary<string, InvoiceRecipientTarget> _flightTypeToInvoiceRecipientMapping;

        public RecipientRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight, 
            IPersonService personService, Dictionary<string, InvoiceRecipientTarget> flightTypeToInvoiceRecipientMapping)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flightTypeToInvoiceRecipientMapping = flightTypeToInvoiceRecipientMapping;
            _flight = flight;
            _personService = personService;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            var flightTypeToRecipientMappingRule = new FlightTypeToInvoiceRecipientMappingRule(_flightTypeToInvoiceRecipientMapping,
                _flight.FlightType.FlightCode);
            flightTypeToRecipientMappingRule.StopRuleEngineWhenRuleApplied = true;

            var rules = new List<IRule<RuleBasedFlightInvoiceDetails>>();
            rules.Add(flightTypeToRecipientMappingRule);
            rules.Add(new FlightCostPaidByPersonRule(_flight, _personService));
            rules.Add(new FlightCostPaidByPilotRule(_flight, _personService));

            _flightInvoiceDetails.ApplyRules(rules);

            return _flightInvoiceDetails;
        }
    }
}
