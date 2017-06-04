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
    internal class AircraftFlightTimeRule : BaseAccountingRule
    {
        private readonly long _minFlightTimeInSecondsMatchingValue;
        private readonly long _maxFlightTimeInSecondsMatchingValue;

        internal AircraftFlightTimeRule(Flight flight, RuleBasedAccountingRuleFilterDetails flightTimeAccountingRuleFilter)
            : base(flight, flightTimeAccountingRuleFilter)
        {
            _minFlightTimeInSecondsMatchingValue = flightTimeAccountingRuleFilter.MinFlightTimeInSecondsMatchingValue ?? 0;
            _maxFlightTimeInSecondsMatchingValue = flightTimeAccountingRuleFilter.MaxFlightTimeInSecondsMatchingValue ?? long.MaxValue;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);

            Conditions.Add(new Between<long>(ruleBasedDelivery.ActiveFlightTimeInSeconds, _minFlightTimeInSecondsMatchingValue, _maxFlightTimeInSecondsMatchingValue, includeMinValue:false, includeMaxValue:true));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            var lineQuantity = 0.0m;

            if (_minFlightTimeInSecondsMatchingValue == 0)
            {
                lineQuantity = Convert.ToDecimal(ruleBasedDelivery.ActiveFlightTimeInSeconds);
                ruleBasedDelivery.ActiveFlightTimeInSeconds = 0;
            }
            else
            {
                lineQuantity = Convert.ToDecimal(ruleBasedDelivery.ActiveFlightTimeInSeconds - _minFlightTimeInSecondsMatchingValue);
                ruleBasedDelivery.ActiveFlightTimeInSeconds = _minFlightTimeInSecondsMatchingValue;
            }

            if (ruleBasedDelivery.DeliveryItems.Any(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = ruleBasedDelivery.DeliveryItems.First(x => x.ArticleNumber == AccountingRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity += GetUnitQuantity(lineQuantity, FLS.Data.WebApi.Accounting.AccountingUnitType.Sec); 

                Logger.Warn($"Delivery line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new DeliveryItemDetails();
                line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
                line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = GetUnitQuantity(lineQuantity, FLS.Data.WebApi.Accounting.AccountingUnitType.Sec);
                line.UnitType = GetUnitTypeString();

                if (AccountingRuleFilter.IncludeThresholdText)
                {
                    if (AccountingRuleFilter.IncludeFlightTypeName)
                    {
                        line.ItemText =
                            $"{Flight.AircraftImmatriculation} {AccountingRuleFilter.ArticleTarget.DeliveryLineText} {Flight.FlightType.FlightTypeName} {AccountingRuleFilter.ThresholdText}";
                    }
                    else
                    {
                        line.ItemText = $"{Flight.AircraftImmatriculation} {AccountingRuleFilter.ArticleTarget.DeliveryLineText} {AccountingRuleFilter.ThresholdText}";
                    }
                }
                else
                {
                    if (AccountingRuleFilter.IncludeFlightTypeName)
                    {
                        line.ItemText =
                            $"{Flight.AircraftImmatriculation} {AccountingRuleFilter.ArticleTarget.DeliveryLineText} {Flight.FlightType.FlightTypeName}";
                    }
                    else
                    {
                        line.ItemText = $"{Flight.AircraftImmatriculation} {AccountingRuleFilter.ArticleTarget.DeliveryLineText}";
                    }
                }

                ruleBasedDelivery.DeliveryItems.Add(line);

                Logger.Debug($"Added new delivery item line to Delivery. Line: {line}");
            }

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }
    }
}
