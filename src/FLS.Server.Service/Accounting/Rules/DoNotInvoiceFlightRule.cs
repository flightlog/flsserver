using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class DoNotInvoiceFlightRule : BaseAccountingRule
    {
        internal DoNotInvoiceFlightRule(Flight flight, RuleBasedAccountingRuleFilterDetails landingTaxAccountingRuleFilter)
            : base(flight, landingTaxAccountingRuleFilter)
        {
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ruleBasedDelivery.DoNotInvoiceFlight = true;

            return base.Apply(ruleBasedDelivery);
        }
    }
}
