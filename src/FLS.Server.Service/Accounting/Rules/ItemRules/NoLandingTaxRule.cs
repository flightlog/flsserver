using System;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules.ItemRules
{
    internal class NoLandingTaxRule : BaseAccountingRule
    {
        private readonly long _minFlightTimeInSecondsMatchingValue;
        private readonly long _maxFlightTimeInSecondsMatchingValue;

        internal NoLandingTaxRule(Flight flight, RuleBasedAccountingRuleFilterDetails noLandingTaxAccountingRuleFilter)
            : base(flight, noLandingTaxAccountingRuleFilter)
        {
            _minFlightTimeInSecondsMatchingValue = noLandingTaxAccountingRuleFilter.MinFlightTimeInSecondsMatchingValue ?? 0;
            _maxFlightTimeInSecondsMatchingValue = noLandingTaxAccountingRuleFilter.MaxFlightTimeInSecondsMatchingValue ?? long.MaxValue;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);

            Conditions.Add(new Between<long>(Convert.ToInt64(Flight.FlightDurationZeroBased.TotalSeconds), _minFlightTimeInSecondsMatchingValue, _maxFlightTimeInSecondsMatchingValue, includeMinValue: false, includeMaxValue: true));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {

            ruleBasedDelivery.NoLandingTaxForGliderFlight = AccountingRuleFilter.NoLandingTaxForGlider;
            ruleBasedDelivery.NoLandingTaxForTowFlight = AccountingRuleFilter.NoLandingTaxForTowingAircraft;
            ruleBasedDelivery.NoLandingTaxForFlight = AccountingRuleFilter.NoLandingTaxForAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {ruleBasedDelivery.NoLandingTaxForGliderFlight}, for towing to: {ruleBasedDelivery.NoLandingTaxForTowFlight}");

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }
    }
}
