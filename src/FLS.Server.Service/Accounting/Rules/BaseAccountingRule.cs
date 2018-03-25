using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;
using NLog;

namespace FLS.Server.Service.Accounting.Rules
{
    internal abstract class BaseAccountingRule : BaseRule<RuleBasedDeliveryDetails>
    {
        protected Flight Flight { get; }
        protected RuleBasedAccountingRuleFilterDetails AccountingRuleFilter { get; }

        internal BaseAccountingRule(Flight flight, RuleBasedAccountingRuleFilterDetails ruleBasedAccountingRuleFilter)
        {
            Flight = flight;
            AccountingRuleFilter = ruleBasedAccountingRuleFilter;
            Logger = LogManager.GetLogger("FLS.Server.Service.Accounting.Rules.BaseAccountingRule");
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            InitializeFlightAircraftTypeConditions(ruleBasedDelivery);

            InitializeAircraftConditions(ruleBasedDelivery);

            InitializeStartConditions(ruleBasedDelivery);

            //Conditions.Add(new Between<double>(ruleBasedDelivery.ActiveFlightTime, AccountingRuleFilter.MinFlightTimeMatchingValue, AccountingRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            InitializeFlightTypeConditions(ruleBasedDelivery);

            InitializeStartLocationConditions(ruleBasedDelivery);

            InitializeLdgLocationConditions(ruleBasedDelivery);

            InitializeFlightCrewConditions(ruleBasedDelivery);

            InitializeAircraftHomebaseConditions(ruleBasedDelivery);
        }

        protected virtual void InitializeAircraftHomebaseConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter.UseRuleForAllAircraftsOnHomebaseExceptListed)
            {
                if (AccountingRuleFilter.MatchedAircraftHomebaseIds != null &&
                    AccountingRuleFilter.MatchedAircraftHomebaseIds.Any())
                {
                    if (Flight.Aircraft != null)
                    {
                        if (Flight.Aircraft.HomebaseId.HasValue)
                        {
                            Conditions.Add(
                                new Inverter(new Contains<Guid>(AccountingRuleFilter.MatchedAircraftHomebaseIds,
                                    Flight.Aircraft.HomebaseId.Value)));
                        }
                        else
                        {
                            Logger.Debug($"Aircraft of flight has no homebase location set. Rule matches!");
                            Conditions.Add(new Equals<bool>(Flight.Aircraft.HomebaseId.HasValue, false));
                        }
                    }
                    else
                    {
                        Logger.Warn($"Flight has no Aircraft set. May we account something wrong!");
                    }
                }
            }
            else
            {
                if (Flight.Aircraft != null)
                {
                    if (Flight.Aircraft.HomebaseId.HasValue)
                    {
                        Conditions.Add(new Contains<Guid>(AccountingRuleFilter.MatchedAircraftHomebaseIds,
                            Flight.Aircraft.HomebaseId.Value));
                    }
                    else
                    {
                        Logger.Debug(
                            $"Aircraft of flight has no homebase location set. Set condition to always FALSE! May we account something wrong!");

                        //add condition which is always false
                        Conditions.Add(new Equals<bool>(false, true));
                    }
                }
                else
                {
                    Logger.Warn($"Flight has no Aircraft set. May we account something wrong!");
                }
            }
        }

        protected virtual void InitializeFlightCrewConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (Flight.FlightCrews.Any() == false)
            {
                Logger.Warn(
                    $"Flight has no flight crew. May we account something wrong! Adding false condition to not fit rule!");
                Conditions.Add(new Equals<bool>(false, true));
            }
            else
            {
                // club members and flight crew types must be combined
                var flightCrewTypes = Enum.GetValues(typeof(FLS.Data.WebApi.Flight.FlightCrewType)).Cast<int>().ToList();
                var flightCrewTypeSelection = flightCrewTypes;
                if (AccountingRuleFilter.UseRuleForAllFlightCrewTypesExceptListed)
                {
                    if (AccountingRuleFilter.MatchedFlightCrewTypes != null &&
                        AccountingRuleFilter.MatchedFlightCrewTypes.Any())
                    {
                        flightCrewTypeSelection = flightCrewTypes.Except(AccountingRuleFilter.MatchedFlightCrewTypes).ToList();
                    }
                }
                else
                {
                    flightCrewTypeSelection = AccountingRuleFilter.MatchedFlightCrewTypes;
                }

                // club member number filtering
                if (AccountingRuleFilter.UseRuleForAllClubMemberNumbersExceptListed)
                {
                    if (AccountingRuleFilter.MatchedClubMemberNumbers != null &&
                        AccountingRuleFilter.MatchedClubMemberNumbers.Any())
                    {
                        //there are some excluded member numbers we have to filter for
                        var persons = Flight.FlightCrews
                            .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                            .Select(x => x.Person);
                        var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                        Conditions.Add(new Inverter(new IntersectAny<string>(AccountingRuleFilter.MatchedClubMemberNumbers,
                            personClubs.Select(pc => pc.MemberNumber))));
                    }
                }
                else
                {
                    var persons = Flight.FlightCrews
                        .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                        .Select(x => x.Person);
                    var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                    Conditions.Add(new IntersectAny<string>(AccountingRuleFilter.MatchedClubMemberNumbers,
                        personClubs.Select(pc => pc.MemberNumber)));
                }

                // club member state filtering
                if (AccountingRuleFilter.UseRuleForAllMemberStatesExceptListed)
                {
                    if (AccountingRuleFilter.MatchedMemberStates != null && AccountingRuleFilter.MatchedMemberStates.Any())
                    {
                        //there are some excluded member states we have to filter for
                        var persons = Flight.FlightCrews
                            .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                            .Select(x => x.Person);
                        var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                        Conditions.Add(new Inverter(new IntersectAny<Guid>(AccountingRuleFilter.MatchedMemberStates,
                            personClubs.Select(pc => pc.MemberStateId.GetValueOrDefault()))));
                    }
                }
                else
                {
                    var persons = Flight.FlightCrews
                        .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                        .Select(x => x.Person);
                    var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                    Conditions.Add(new IntersectAny<Guid>(AccountingRuleFilter.MatchedMemberStates,
                        personClubs.Select(pc => pc.MemberStateId.GetValueOrDefault())));
                }

                //// club person category filtering
                //if (AccountingRuleFilter.UseRuleForAllPersonCategoriesExceptListed)
                //{
                //    if (AccountingRuleFilter.MatchedPersonCategories != null && AccountingRuleFilter.MatchedPersonCategories.Any())
                //    {
                //        //there are some excluded person category we have to filter for
                //        var persons = Flight.FlightCrews
                //            .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                //            .Select(x => x.Person);
                //        var personPersonCategories = persons.Where(pers => pers.PersonPersonCategories.Any(q => AccountingRuleFilter.MatchedPersonCategories.Contains(q.PersonCategoryId))).Select(p => p.);

                //        Conditions.Add(new Inverter(new IntersectAny<Guid>(AccountingRuleFilter.MatchedPersonCategories,
                //            personPersonCategories.Select(pc => pc.))));
                //    }
                //}
                //else
                //{
                //    var persons = _flight.FlightCrews
                //                        .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                //                        .Select(x => x.Person);
                //    var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                //    Conditions.Add(new IntersectAny<Guid>(AccountingRuleFilter.MatchedMemberStates,
                //        personClubs.Select(pc => pc.MemberStateId.GetValueOrDefault())));
                //}
            }
        }

        protected virtual void InitializeLdgLocationConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter.UseRuleForAllLdgLocationsExceptListed)
            {
                if (AccountingRuleFilter.MatchedLdgLocationIds != null && AccountingRuleFilter.MatchedLdgLocationIds.Any())
                {
                    if (Flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we account something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(AccountingRuleFilter.MatchedLdgLocationIds,
                                Flight.LdgLocationId.Value)));
                    }
                }
            }
            else
            {
                if (Flight.LdgLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no landing location set. May we account something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(AccountingRuleFilter.MatchedLdgLocationIds,
                        Flight.LdgLocationId.Value));
                }
            }
        }

        protected virtual void InitializeStartLocationConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter.UseRuleForAllStartLocationsExceptListed)
            {
                if (AccountingRuleFilter.MatchedLdgLocationIds != null && AccountingRuleFilter.MatchedLdgLocationIds.Any())
                {
                    if (Flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we account something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(AccountingRuleFilter.MatchedStartLocationIds,
                                Flight.StartLocationId.Value)));
                    }
                }
            }
            else
            {
                if (Flight.StartLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no start location set. May we account something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(AccountingRuleFilter.MatchedStartLocationIds,
                        Flight.StartLocationId.Value));
                }
            }
        }

        protected virtual void InitializeFlightTypeConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ICondition condition;

            if (AccountingRuleFilter.UseRuleForAllFlightTypesExceptListed)
            {
                if (AccountingRuleFilter.MatchedFlightTypeCodes != null && AccountingRuleFilter.MatchedFlightTypeCodes.Any())
                {
                    condition = null;

                    if (AccountingRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in Flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (Flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes,
                                    Flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(
                        new Inverter(new Or(condition,
                            new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes, Flight.FlightType.FlightCode))));
                }
            }
            else
            {
                condition = null;

                if (AccountingRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in Flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (Flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes,
                                Flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition,
                    new Contains<string>(AccountingRuleFilter.MatchedFlightTypeCodes, Flight.FlightType.FlightCode)));
            }
        }

        protected virtual void InitializeAircraftConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter.UseRuleForAllAircraftsExceptListed)
            {
                if (AccountingRuleFilter.MatchedAircraftIds != null && AccountingRuleFilter.MatchedAircraftIds.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(AccountingRuleFilter.MatchedAircraftIds, Flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(AccountingRuleFilter.MatchedAircraftIds, Flight.AircraftId));
            }
        }

        protected virtual void InitializeStartConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter.UseRuleForAllStartTypesExceptListed)
            {
                if (AccountingRuleFilter.MatchedStartTypes != null && AccountingRuleFilter.MatchedStartTypes.Any())
                {
                    Conditions.Add(
                        new Inverter(new Contains<int>(AccountingRuleFilter.MatchedStartTypes,
                            Flight.StartTypeId.GetValueOrDefault())));
                }
            }
            else
            {
                Conditions.Add(new Contains<int>(AccountingRuleFilter.MatchedStartTypes,
                    Flight.StartTypeId.GetValueOrDefault()));
            }
        }

        protected virtual void InitializeFlightAircraftTypeConditions(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            int ruleAircraftTypeValue = Convert.ToInt32(AccountingRuleFilter.IsRuleForGliderFlights)*1;
            ruleAircraftTypeValue += Convert.ToInt32(AccountingRuleFilter.IsRuleForTowingFlights) * 2;
            ruleAircraftTypeValue += Convert.ToInt32(AccountingRuleFilter.IsRuleForMotorFlights) * 4;

            var condition = new Equals<int>(Flight.FlightAircraftType & ruleAircraftTypeValue, Flight.FlightAircraftType);
            
            Conditions.Add(condition);
        }

        protected Decimal GetUnitQuantity(decimal quantity, FLS.Data.WebApi.Accounting.AccountingUnitType baseUnitType)
        {
            var outputUnitType =
                (FLS.Data.WebApi.Accounting.AccountingUnitType)
                AccountingRuleFilter.AccountingUnitTypeId.GetValueOrDefault(0);
            if (baseUnitType == outputUnitType) return quantity;

            var returnQuantity = 0.0m;

            switch (outputUnitType)
            {
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Min:
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Sec) returnQuantity = quantity / 60;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Min) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs
                        || baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight)
                            throw new InvalidCastException("Can not convert from landings or starts into minutes!");
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Sec:
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Sec) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Min) returnQuantity = quantity * 60;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs
                        || baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight)
                        throw new InvalidCastException("Can not convert from landings or starts into seconds!");
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs:
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Sec
                        || baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Min)
                        throw new InvalidCastException("Can not convert from seconds or minutes into landings!");
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight:
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight) returnQuantity = quantity;
                    if (baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Sec
                        || baseUnitType == FLS.Data.WebApi.Accounting.AccountingUnitType.Min)
                        throw new InvalidCastException("Can not convert from seconds or minutes into starts!");
                    break;
                default:
                    returnQuantity = quantity;
                    break;
            }

            return returnQuantity;
        }

        protected string GetUnitTypeString()
        {
            string unitTypeString;

            switch ((FLS.Data.WebApi.Accounting.AccountingUnitType)
                        AccountingRuleFilter.AccountingUnitTypeId.GetValueOrDefault(0))
            {
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Sec:
                    unitTypeString = "Sekunden";
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Min:
                    unitTypeString = "Minuten";
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.StartOrFlight:
                    unitTypeString = "Start";
                    break;
                case FLS.Data.WebApi.Accounting.AccountingUnitType.Ldgs:
                    unitTypeString = "Landung";
                    break;
                default:
                    unitTypeString = string.Empty;
                    break;
            }

            return unitTypeString;
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}, Rule-Filter: {AccountingRuleFilter.RuleFilterName} of type: {this.GetType()} has conditions: '{string.Join(",", Conditions.Select(x => x))}' for Flight: {Flight}";
        }
    }
}
