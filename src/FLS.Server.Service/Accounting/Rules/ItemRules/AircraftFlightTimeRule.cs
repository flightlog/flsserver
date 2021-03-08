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
            var lineFlightTimeInSec = ruleBasedDelivery.ActiveFlightTimeInSeconds;

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

            var discount = 0;
            long currentFlightTimeCredit = long.MaxValue;
            PersonFlightTimeCredit matchedCredit = null;

            if (ruleBasedDelivery.PersonFlightTimeCredits != null)
            {
                foreach (var credit in ruleBasedDelivery.PersonFlightTimeCredits)
                {
                    matchedCredit = MatchesPersonFlightTimeCredit(credit);

                    if (matchedCredit != null)
                    {
                        var balance = credit.PersonFlightTimeCreditTransactions.First(x => x.IsCurrent);

                        if (balance.NoFlightTimeLimit == false && balance.CurrentFlightTimeBalanceInSeconds.GetValueOrDefault(0) == 0)
                        {
                            currentFlightTimeCredit = 0;
                            continue;
                        }

                        //rule applies
                        ruleBasedDelivery.MatchedPersonFlightTimeCredit = matchedCredit;
                        discount = credit.DiscountInPercent;

                        var newTransaction = new PersonFlightTimeCreditTransaction(balance);
                        newTransaction.CurrentFlightTimeBalanceInSeconds = balance.CurrentFlightTimeBalanceInSeconds;
                        newTransaction.OldFlightTimeBalanceInSeconds = balance.CurrentFlightTimeBalanceInSeconds;
                        ruleBasedDelivery.NewPersonFlightTimeCreditTransaction = newTransaction;
                        ruleBasedDelivery.OldPersonFlightTimeCreditTransaction = balance;

                        if (credit.NoFlightTimeLimit == false)
                        {
                            if (credit.PersonFlightTimeCreditTransactions.Any())
                            {
                                currentFlightTimeCredit = balance.CurrentFlightTimeBalanceInSeconds.Value;
                            }
                        }
                        else
                        {
                            newTransaction.CurrentFlightTimeBalanceInSeconds = null;
                            newTransaction.OldFlightTimeBalanceInSeconds = null;
                        }

                        break;
                    }
                }
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

                //handle flight time credit
                if (matchedCredit != null && currentFlightTimeCredit > 0)
                {
                    if (lineFlightTimeInSec > currentFlightTimeCredit)
                    {
                        var overCreditLineQuantity = lineQuantity - currentFlightTimeCredit;
                        lineQuantity = currentFlightTimeCredit;

                        if (ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.NoFlightTimeLimit == false)
                        {
                            //set only if flight time limmit
                            ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.CurrentFlightTimeBalanceInSeconds = 0;
                        }

                        ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.FlightTimeBalanceInSeconds = -currentFlightTimeCredit;

                        line.Quantity = GetUnitQuantity(lineQuantity, FLS.Data.WebApi.Accounting.AccountingUnitType.Sec);
                        line.DiscountInPercent = discount;
                        var itemText = line.ItemText;

                        line = new DeliveryItemDetails();
                        line.Position = ruleBasedDelivery.DeliveryItems.Count + 1;
                        line.ArticleNumber = AccountingRuleFilter.ArticleTarget.ArticleNumber;
                        line.Quantity = GetUnitQuantity(overCreditLineQuantity, FLS.Data.WebApi.Accounting.AccountingUnitType.Sec);
                        line.UnitType = GetUnitTypeString();
                        line.DiscountInPercent = 0;
                        line.ItemText = itemText;

                        ruleBasedDelivery.DeliveryItems.Add(line);
                    }
                    else
                    {
                        line.DiscountInPercent = discount;

                        if (ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.NoFlightTimeLimit == false)
                        {
                            //set only if flight time limit
                            ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.CurrentFlightTimeBalanceInSeconds = currentFlightTimeCredit - lineFlightTimeInSec;
                        }

                        ruleBasedDelivery.NewPersonFlightTimeCreditTransaction.FlightTimeBalanceInSeconds = -lineFlightTimeInSec;
                    }
                }
            }

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }

        private PersonFlightTimeCredit MatchesPersonFlightTimeCredit(PersonFlightTimeCredit credit)
        {
            if (credit.UseRuleForAllAircraftsExceptListed)
            {
                if (credit.MatchedAircraftImmatriculations != null && credit.MatchedAircraftImmatriculations.Any())
                {
                    if (credit.MatchedAircraftImmatriculations.Contains(Flight.AircraftImmatriculation) == false)
                    {
                        //rule applies
                        return credit;
                    }
                }
            }
            else
            {
                if (credit.MatchedAircraftImmatriculations.Contains(Flight.AircraftImmatriculation))
                {
                    //rule applies
                    return credit;
                }
            }

            return null;
        }
    }
}
