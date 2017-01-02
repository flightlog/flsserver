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
    internal class LandingTaxRule : BaseAccountingRule
    {
        internal LandingTaxRule(Flight flight, RuleBasedAccountingRuleFilterDetails landingTaxAccountingRuleFilter)
            : base(flight, landingTaxAccountingRuleFilter)
        {
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);

            if (ruleBasedDelivery.NoLandingTaxForGliderFlight &&
                Flight.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }

            if (ruleBasedDelivery.NoLandingTaxForTowFlight &&
                Flight.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (ruleBasedDelivery.DeliveryItems.Any(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber))
            {
                var line = ruleBasedDelivery.DeliveryItems.First(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity++;

                Logger.Warn($"Delivery line for landing tax already exists. Add quantity to the existing line! New line value: {line}");
            }
            else
            {
                var line = new DeliveryItemDetails();
                line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
                line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = 1.0m;
                line.UnitType = CostCenterUnitType.PerLanding.ToUnitTypeString();
                line.ItemText = $"{AccountingRuleFilter.ArticleTarget.DeliveryLineText}";

                ruleBasedDelivery.DeliveryItems.Add(line);

                Logger.Debug($"Added new delivery item line to delivery. Line: {line}");
            }
            
            return base.Apply(ruleBasedDelivery);
        }
    }
}
