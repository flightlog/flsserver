using System.Collections.Generic;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting.Rules;
using FLS.Server.Service.RulesEngine;

namespace FLS.Server.Service.Accounting.RuleEngines
{
    internal class RecipientRulesEngine
    {
        private readonly RuleBasedDeliveryDetails _ruleBasedDelivery;
        private readonly Flight _flight;
        private readonly IPersonService _personService;
        private readonly List<RuleBasedAccountingRuleFilterDetails> _accountingRecipientRuleFilters;

        public RecipientRulesEngine(RuleBasedDeliveryDetails ruleBasedDelivery, Flight flight, 
            IPersonService personService, List<RuleBasedAccountingRuleFilterDetails> accountingRecipientRuleFilters)
        {
            _ruleBasedDelivery = ruleBasedDelivery;
            _accountingRecipientRuleFilters = accountingRecipientRuleFilters;
            _flight = flight;
            _personService = personService;
        }

        public RuleBasedDeliveryDetails Run()
        {
            var rules = new List<IRule<RuleBasedDeliveryDetails>>();

            foreach (var accountingRecipientRuleFilter in _accountingRecipientRuleFilters)
            {
                var rule = new DeliveryRecipientRule(_flight, accountingRecipientRuleFilter);
                rule.StopRuleEngineWhenRuleApplied = true;
                rules.Add(rule);
            }

            rules.Add(new FlightCostPaidByPersonRule(_flight, _personService));
            rules.Add(new FlightCostPaidByPilotRule(_flight, _personService));

            _ruleBasedDelivery.ApplyRules(rules);

            return _ruleBasedDelivery;
        }
    }
}
