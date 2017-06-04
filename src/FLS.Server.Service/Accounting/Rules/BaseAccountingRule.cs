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
        private readonly Flight _flight;
        private readonly RuleBasedAccountingRuleFilterDetails _accountingRuleFilter;

        protected Flight Flight { get { return _flight; } }
        protected RuleBasedAccountingRuleFilterDetails AccountingRuleFilter { get { return _accountingRuleFilter; } }
        internal BaseAccountingRule(Flight flight, RuleBasedAccountingRuleFilterDetails ruleBasedAccountingRuleFilter)
        {
            _flight = flight;
            _accountingRuleFilter = ruleBasedAccountingRuleFilter;
            Logger = LogManager.GetLogger("FLS.Server.Service.Accounting.Rules.BaseAccountingRule");
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ICondition condition = null;

            if (AccountingRuleFilter.IsRuleForGliderFlights)
            {
                condition = new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.GliderFlight);
            }
            else
            {
                condition = new Inverter(new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.GliderFlight));
            }

            if (AccountingRuleFilter.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }
            else
            {
                condition = new And(condition, new Inverter(new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight)));
            }

            if (AccountingRuleFilter.IsRuleForMotorFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.MotorFlight));
            }
            else
            {
                condition = new And(condition, new Inverter(new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.MotorFlight)));
            }

            Conditions.Add(condition);

            if (_accountingRuleFilter.UseRuleForAllAircraftsExceptListed)
            {
                if (_accountingRuleFilter.MatchedAircraftIds != null && _accountingRuleFilter.MatchedAircraftIds.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_accountingRuleFilter.MatchedAircraftIds, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_accountingRuleFilter.MatchedAircraftIds, _flight.AircraftId));
            }

            if (_accountingRuleFilter.UseRuleForAllStartTypesExceptListed)
            {
                if (_accountingRuleFilter.MatchedStartTypes != null && _accountingRuleFilter.MatchedStartTypes.Any())
                {
                    Conditions.Add(new Inverter(new Contains<int>(_accountingRuleFilter.MatchedStartTypes, _flight.StartTypeId.GetValueOrDefault())));
                }
            }
            else
            {
                Conditions.Add(new Contains<int>(_accountingRuleFilter.MatchedStartTypes, _flight.StartTypeId.GetValueOrDefault()));
            }

            //Conditions.Add(new Between<double>(ruleBasedDelivery.ActiveFlightTime, _accountingRuleFilter.MinFlightTimeMatchingValue, _accountingRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            if (_accountingRuleFilter.UseRuleForAllFlightTypesExceptListed)
            {
                if (_accountingRuleFilter.MatchedFlightTypeCodes != null && _accountingRuleFilter.MatchedFlightTypeCodes.Any())
                {
                    condition = null;

                    if (_accountingRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in _flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (_flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes,
                                    _flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(new Inverter(new Or(condition, new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode))));
                }
            }
            else
            {
                condition = null;

                if (_accountingRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in _flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (_flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes,
                                _flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition, new Contains<string>(_accountingRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
            }

            if (_accountingRuleFilter.UseRuleForAllStartLocationsExceptListed)
            {
                if (_accountingRuleFilter.MatchedLdgLocationIds != null && _accountingRuleFilter.MatchedLdgLocationIds.Any())
                {
                    if (_flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we account something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_accountingRuleFilter.MatchedStartLocationIds,
                                _flight.StartLocationId.Value)));
                    }
                }
            }
            else
            {
                if (_flight.StartLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no start location set. May we account something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(_accountingRuleFilter.MatchedStartLocationIds,
                        _flight.StartLocationId.Value));
                }
            }

            if (_accountingRuleFilter.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_accountingRuleFilter.MatchedLdgLocationIds != null && _accountingRuleFilter.MatchedLdgLocationIds.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we account something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_accountingRuleFilter.MatchedLdgLocationIds,
                                _flight.LdgLocationId.Value)));
                    }
                }
            }
            else
            {
                if (_flight.LdgLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no landing location set. May we account something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(_accountingRuleFilter.MatchedLdgLocationIds,
                        _flight.LdgLocationId.Value));
                }
            }

            if (_flight.FlightCrews.Any() == false)
            {
                Logger.Warn($"Flight has no flight crew. May we account something wrong! Adding false condition to not fit rule!");
                Conditions.Add(new Equals<bool>(false, true));
            }
            else
            {
                // club members and flight crew types must be combined
                var flightCrewTypes = Enum.GetValues(typeof(FLS.Data.WebApi.Flight.FlightCrewType)).Cast<int>().ToList();
                var flightCrewTypeSelection = flightCrewTypes;
                if (_accountingRuleFilter.UseRuleForAllFlightCrewTypesExceptListed)
                {
                    if (_accountingRuleFilter.MatchedFlightCrewTypes != null && _accountingRuleFilter.MatchedFlightCrewTypes.Any())
                    {
                        flightCrewTypeSelection = flightCrewTypes.Except(_accountingRuleFilter.MatchedFlightCrewTypes).ToList();
                    }
                }
                else
                {
                    flightCrewTypeSelection = _accountingRuleFilter.MatchedFlightCrewTypes;
                }

                if (_accountingRuleFilter.UseRuleForAllClubMemberNumbersExceptListed)
                {
                    if (_accountingRuleFilter.MatchedClubMemberNumbers != null && _accountingRuleFilter.MatchedClubMemberNumbers.Any())
                    {
                        //there are some excluded member numbers we have to filter for
                        var persons = _flight.FlightCrews
                                            .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                                            .Select(x => x.Person);
                        var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                        Conditions.Add(new Inverter(new IntersectAny<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                            personClubs.Select(pc => pc.MemberNumber))));
                    }
                }
                else
                {
                    var persons = _flight.FlightCrews
                                        .Where(x => flightCrewTypeSelection.Contains(x.FlightCrewTypeId))
                                        .Select(x => x.Person);
                    var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                    Conditions.Add(new IntersectAny<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                        personClubs.Select(pc => pc.MemberNumber)));
                }
            }
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
                $"{base.ToString()}, Rule-Filter: {_accountingRuleFilter.RuleFilterName} of type: {this.GetType()} has conditions: '{string.Join(",", Conditions.Select(x => x))}' for Flight: {Flight}";
        }
    }
}
