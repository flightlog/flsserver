using System;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;

namespace FLS.Server.Service.Accounting.Rules.ItemRules
{
    internal class InstructorFeeRule : BaseAccountingRule
    {
        internal InstructorFeeRule(Flight flight, RuleBasedAccountingRuleFilterDetails instructorFeeAccountingRuleFilter)
            : base(flight, instructorFeeAccountingRuleFilter)
        {
        }
        
        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            var line = new DeliveryItemDetails();
            line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
            line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
            line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

            line.ItemText = $"Fluglehrer-Honorar {Flight.InstructorDisplayName}";

            if (Flight.FlightCostBalanceTypeId.HasValue &&
                                Flight.FlightCostBalanceTypeId.Value ==
                                (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee)
            {
                //no instructor fee for this flight, so set quantity to 0
                line.Quantity = 0;
            }
            else
            {
                line.Quantity = Convert.ToDecimal(Flight.FlightDurationZeroBased.TotalMinutes);
            }

            ruleBasedDelivery.DeliveryItems.Add(line);

            Logger.Debug($"Added new delivery item line to delivery. Line: {line}");

            return base.Apply(ruleBasedDelivery);
        }
    }
}
