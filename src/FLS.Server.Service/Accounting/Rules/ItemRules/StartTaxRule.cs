using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Service.RulesEngine.Conditions;
using System;

namespace FLS.Server.Service.Accounting.Rules.ItemRules
{
    internal class StartTaxRule : BaseAccountingRule
    {
        private readonly long _minFlightTimeInSecondsMatchingValue;
        private readonly long _maxFlightTimeInSecondsMatchingValue;

        internal StartTaxRule(Flight flight, RuleBasedAccountingRuleFilterDetails startTaxAccountingRuleFilter)
            : base(flight, startTaxAccountingRuleFilter)
        {
            _minFlightTimeInSecondsMatchingValue = startTaxAccountingRuleFilter.MinFlightTimeInSecondsMatchingValue ?? 0;
            _maxFlightTimeInSecondsMatchingValue = startTaxAccountingRuleFilter.MaxFlightTimeInSecondsMatchingValue ?? long.MaxValue;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);
            
            Conditions.Add(new Between<long>(Convert.ToInt64(Flight.FlightDurationZeroBased.TotalSeconds), _minFlightTimeInSecondsMatchingValue, _maxFlightTimeInSecondsMatchingValue, includeMinValue: false, includeMaxValue: true));
        }
        
        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (ruleBasedDelivery.DeliveryItems.Any(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber))
            {
                var line = ruleBasedDelivery.DeliveryItems.First(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity++;

                Logger.Warn($"Delivery line for start tax already exists. Add quantity 1 to the existing line! New line value: {line}");
            }
            else
            {
                var line = new DeliveryItemDetails();
                line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
                line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = 1;
                line.UnitType = GetUnitTypeString();
                line.ItemText = $"{AccountingRuleFilter.ArticleTarget.DeliveryLineText}";

                ruleBasedDelivery.DeliveryItems.Add(line);

                Logger.Debug($"Added new delivery item line to delivery. Line: {line}");
            }

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }
    }
}
