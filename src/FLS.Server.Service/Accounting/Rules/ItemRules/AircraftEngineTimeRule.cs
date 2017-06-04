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
    internal class AircraftEngineTimeRule : BaseAccountingRule
    {
        private readonly long _minEngineTimeInSecondsMatchingValue;
        private readonly long _maxEngineTimeInSecondsMatchingValue;

        internal AircraftEngineTimeRule(Flight flight, RuleBasedAccountingRuleFilterDetails engineTimeAccountingRuleFilter)
            : base(flight, engineTimeAccountingRuleFilter)
        {
            _minEngineTimeInSecondsMatchingValue = engineTimeAccountingRuleFilter.MinEngineTimeInSecondsMatchingValue ?? 0;
            _maxEngineTimeInSecondsMatchingValue = engineTimeAccountingRuleFilter.MaxEngineTimeInSecondsMatchingValue ?? long.MaxValue;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            AccountingRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(ruleBasedDelivery);

            //TODO: engine time filter calculation
            Conditions.Add(new Between<long>(ruleBasedDelivery.ActiveEngineTimeInSeconds, _minEngineTimeInSecondsMatchingValue, _maxEngineTimeInSecondsMatchingValue, includeMinValue:false, includeMaxValue:true));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            var lineQuantity = 0.0m;

            if (_minEngineTimeInSecondsMatchingValue == 0)
            {
                lineQuantity = Convert.ToDecimal(ruleBasedDelivery.ActiveEngineTimeInSeconds);
                ruleBasedDelivery.ActiveEngineTimeInSeconds = 0;
            }
            else
            {
                lineQuantity = Convert.ToDecimal(ruleBasedDelivery.ActiveEngineTimeInSeconds - _minEngineTimeInSecondsMatchingValue);
                ruleBasedDelivery.ActiveEngineTimeInSeconds = _minEngineTimeInSecondsMatchingValue;
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
