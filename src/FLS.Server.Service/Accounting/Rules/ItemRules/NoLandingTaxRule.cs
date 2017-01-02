using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Accounting.Rules.ItemRules
{
    internal class NoLandingTaxRule : BaseAccountingRule
    {
        internal NoLandingTaxRule(Flight flight, RuleBasedAccountingRuleFilterDetails noLandingTaxAccountingRuleFilter)
            : base(flight, noLandingTaxAccountingRuleFilter)
        {
        }
        
        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {

            ruleBasedDelivery.NoLandingTaxForGliderFlight = AccountingRuleFilter.NoLandingTaxForGlider;
            ruleBasedDelivery.NoLandingTaxForTowFlight = AccountingRuleFilter.NoLandingTaxForTowingAircraft;
            ruleBasedDelivery.NoLandingTaxForFlight = AccountingRuleFilter.NoLandingTaxForAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {ruleBasedDelivery.NoLandingTaxForGliderFlight}, for towing to: {ruleBasedDelivery.NoLandingTaxForTowFlight}");

            return base.Apply(ruleBasedDelivery);
        }
    }
}
