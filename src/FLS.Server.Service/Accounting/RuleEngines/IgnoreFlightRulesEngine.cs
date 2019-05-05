using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting.Rules;
using FLS.Server.Service.Accounting.Rules.ItemRules;
using FLS.Server.Service.RulesEngine;
using NLog;
using AccountingRuleFilterType = FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType;

namespace FLS.Server.Service.Accounting.RuleEngines
{
    internal class IgnoreFlightRulesEngine
    {
        private Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private readonly RuleBasedDeliveryDetails _ruleBasedDelivery;
        private readonly Flight _flight;
        private readonly List<RuleBasedAccountingRuleFilterDetails> _accountingRuleFilters;

        public IgnoreFlightRulesEngine(RuleBasedDeliveryDetails ruleBasedDelivery, Flight flight, 
            List<RuleBasedAccountingRuleFilterDetails> accountingRuleFilters)
        {
            _ruleBasedDelivery = ruleBasedDelivery;
            _flight = flight;
            _accountingRuleFilters = accountingRuleFilters;
        }

        public RuleBasedDeliveryDetails Run()
        {
            Logger.Trace($"Start of IgnoreFlightRulesEngine.Run()");
            var rules = new List<IRule<RuleBasedDeliveryDetails>>();

            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.DoNotInvoiceFlightRuleFilter))
            {
                var rule = new DoNotInvoiceFlightRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of DoNotInvoiceFlightRule");
            _ruleBasedDelivery.ApplyRules(rules);

            return _ruleBasedDelivery;
        }
    }
}
