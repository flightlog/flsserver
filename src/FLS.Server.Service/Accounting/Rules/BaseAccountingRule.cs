using System;
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

            //TODO: condition matches even all "IsRuleFor..." are set to 0
            if (AccountingRuleFilter.IsRuleForSelfstartedGliderFlights)
            {
                condition = new Equals<int>(_flight.StartTypeId.GetValueOrDefault(), (int)AircraftStartType.SelfStart);
            }

            if (AccountingRuleFilter.IsRuleForGliderFlights)
            {
                condition = new Or(condition,
                    new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.GliderFlight));
            }

            if (AccountingRuleFilter.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }

            if (condition != null)
            {
                Conditions.Add(condition);
                condition = null;
            }

            if (_accountingRuleFilter.UseRuleForAllAircraftsExceptListed)
            {
                if (_accountingRuleFilter.MatchedAircraftImmatriculations.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_accountingRuleFilter.MatchedAircraftIds, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_accountingRuleFilter.MatchedAircraftIds, _flight.AircraftId));
            }


            //Conditions.Add(new Between<double>(ruleBasedDelivery.ActiveFlightTime, _accountingRuleFilter.MinFlightTimeMatchingValue, _accountingRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            if (_accountingRuleFilter.UseRuleForAllFlightTypesExceptListed)
            {
                if (_accountingRuleFilter.MatchedFlightTypeCodes.Any())
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
                if (_accountingRuleFilter.MatchedStartLocations.Any())
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
                if (_accountingRuleFilter.MatchedLdgLocations.Any())
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

            if (_accountingRuleFilter.UseRuleForAllClubMemberNumbersExceptListed)
            {
                if (_accountingRuleFilter.MatchedClubMemberNumbers.Any())
                {
                    if (_flight.FlightCrews.Any() == false)
                    {
                        Logger.Warn($"Flight has no flight crew. May we account something wrong!");
                    }
                    else
                    {
                        var persons = _flight.FlightCrews.Select(x => x.Person);
                        var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                        Conditions.Add(new Inverter(new IntersectAny<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                            personClubs.Select(pc => pc.MemberNumber))));
                    }
                    //if (string.IsNullOrWhiteSpace(ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber))
                    //{
                    //    Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                    //}
                    //else
                    //{

                    //    Conditions.Add(new Inverter(new Contains<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                    //        ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber)));
                    //}
                }
            }
            else
            {
                //if (string.IsNullOrWhiteSpace(ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber))
                //{
                //    Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                //}
                //else
                //{

                //    Conditions.Add(new Contains<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                //        ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber));
                //}

                var persons = _flight.FlightCrews.Select(x => x.Person);
                var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == ruleBasedDelivery.ClubId));

                Conditions.Add(new IntersectAny<string>(_accountingRuleFilter.MatchedClubMemberNumbers,
                    personClubs.Select(pc => pc.MemberNumber)));
            }

            if (_accountingRuleFilter.UseRuleForAllFlightCrewTypesExceptListed)
            {
                if (_accountingRuleFilter.MatchedFlightCrewTypes.Any())
                {
                    if (_flight.FlightCrews.Any() == false)
                    {
                        Logger.Warn($"Flight has no flight crew. May we account something wrong!");
                    }
                    else
                    {
                        Conditions.Add(new Inverter(new IntersectAny<int>(_accountingRuleFilter.MatchedFlightCrewTypes,
                            _flight.FlightCrews.Select(x => x.FlightCrewTypeId))));
                    }
                }
            }
            else
            {
                if (_flight.FlightCrews.Any() == false)
                {
                    Logger.Warn($"Flight has no flight crew. May we account something wrong!");
                }
                else
                {
                    Conditions.Add(new IntersectAny<int>(_accountingRuleFilter.MatchedFlightCrewTypes,
                        _flight.FlightCrews.Select(x => x.FlightCrewTypeId)));
                }
            }
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}, Rule-Filter: {_accountingRuleFilter.RuleFilterName} of type: {this.GetType()} has conditions: '{string.Join(",", Conditions.Select(x => x))}' for Flight: {Flight}";
        }
    }
}
