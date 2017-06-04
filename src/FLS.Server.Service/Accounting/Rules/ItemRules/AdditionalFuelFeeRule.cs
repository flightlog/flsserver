using System;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;

namespace FLS.Server.Service.Accounting.Rules.ItemRules
{
    internal class AdditionalFuelFeeRule : BaseAccountingRule
    {

        internal AdditionalFuelFeeRule(Flight flight, RuleBasedAccountingRuleFilterDetails additionalFuelFeeAccountingRuleFilter)
            : base(flight, additionalFuelFeeAccountingRuleFilter)
        {
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (ruleBasedDelivery.DeliveryItems.Any(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = ruleBasedDelivery.DeliveryItems.First(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity += GetUnitQuantity(Convert.ToDecimal(Flight.FlightDurationZeroBased.TotalMinutes), FLS.Data.WebApi.Accounting.AccountingUnitType.Min);

                Logger.Warn($"Delivery line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new DeliveryItemDetails();
                line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
                line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = GetUnitQuantity(Convert.ToDecimal(Flight.FlightDurationZeroBased.TotalMinutes), FLS.Data.WebApi.Accounting.AccountingUnitType.Min);
                line.UnitType = GetUnitTypeString();
                line.ItemText = $"{AccountingRuleFilter.ArticleTarget.DeliveryLineText} {Flight.AircraftImmatriculation}";
                
                ruleBasedDelivery.DeliveryItems.Add(line);

                Logger.Debug($"Added new delivery item line to delivery. Line: {line}");
            }

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }
    }
}
