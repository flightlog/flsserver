using System;
using System.Linq;
using System.Text;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;
using NLog;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal abstract class BaseInvoiceRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly InvoiceRuleFilterDetails _invoiceRuleFilter;

        protected Flight Flight { get { return _flight; } }
        protected InvoiceRuleFilterDetails InvoiceRuleFilter { get { return _invoiceRuleFilter; } }
        internal BaseInvoiceRule(Flight flight, InvoiceRuleFilterDetails invoiceRuleFilterDetails)
        {
            _flight = flight;
            _invoiceRuleFilter = invoiceRuleFilterDetails;
            Logger = LogManager.GetLogger("FLS.Server.Service.Invoicing.Rules.BaseInvoiceRule");
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            ICondition condition = null;

            //TODO: condition matches even all "IsRuleFor..." are set to 0
            if (InvoiceRuleFilter.IsRuleForSelfstartedGliderFlights)
            {
                condition = new Equals<int>(_flight.StartTypeId.GetValueOrDefault(), (int)AircraftStartType.SelfStart);
            }

            if (InvoiceRuleFilter.IsRuleForGliderFlights)
            {
                condition = new Or(condition,
                    new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.GliderFlight));
            }

            if (InvoiceRuleFilter.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }

            if (condition != null)
            {
                Conditions.Add(condition);
                condition = null;
            }

            if (_invoiceRuleFilter.UseRuleForAllAircraftsExceptListed)
            {
                if (_invoiceRuleFilter.AircraftImmatriculations.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_invoiceRuleFilter.Aircrafts, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_invoiceRuleFilter.Aircrafts, _flight.AircraftId));
            }


            //Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, _invoiceLineRuleFilter.MinFlightTimeMatchingValue, _invoiceLineRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            if (_invoiceRuleFilter.UseRuleForAllFlightTypesExceptListed)
            {
                if (_invoiceRuleFilter.MatchedFlightTypeCodes.Any())
                {
                    condition = null;

                    if (_invoiceRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in _flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (_flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes,
                                    _flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(new Inverter(new Or(condition, new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode))));
                }
            }
            else
            {
                condition = null;

                if (_invoiceRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in _flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (_flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes,
                                _flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition, new Contains<string>(_invoiceRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
            }

            if (_invoiceRuleFilter.UseRuleForAllStartLocationsExceptListed)
            {
                if (_invoiceRuleFilter.MatchedStartLocations.Any())
                {
                    if (_flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_invoiceRuleFilter.MatchedStartLocationIds,
                                _flight.StartLocationId.Value)));
                    }
                }
            }
            else
            {
                if (_flight.StartLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no start location set. May we invoice something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(_invoiceRuleFilter.MatchedStartLocationIds,
                        _flight.StartLocationId.Value));
                }
            }

            if (_invoiceRuleFilter.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_invoiceRuleFilter.MatchedLdgLocations.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_invoiceRuleFilter.MatchedLdgLocationIds,
                                _flight.LdgLocationId.Value)));
                    }
                }
            }
            else
            {
                if (_flight.LdgLocationId.HasValue == false)
                {
                    Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                }
                else
                {
                    Conditions.Add(new Contains<Guid>(_invoiceRuleFilter.MatchedLdgLocationIds,
                        _flight.LdgLocationId.Value));
                }
            }

            if (_invoiceRuleFilter.UseRuleForAllClubMemberNumbersExceptListed)
            {
                if (_invoiceRuleFilter.MatchedClubMemberNumbers.Any())
                {
                    if (_flight.FlightCrews.Any() == false)
                    {
                        Logger.Warn($"Flight has no flight crew. May we invoice something wrong!");
                    }
                    else
                    {
                        var persons = _flight.FlightCrews.Select(x => x.Person);
                        var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == flightInvoiceDetails.ClubId));

                        Conditions.Add(new Inverter(new IntersectAny<string>(_invoiceRuleFilter.MatchedClubMemberNumbers,
                            personClubs.Select(pc => pc.MemberNumber))));
                    }
                    //if (string.IsNullOrWhiteSpace(flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber))
                    //{
                    //    Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                    //}
                    //else
                    //{

                    //    Conditions.Add(new Inverter(new Contains<string>(_invoiceRuleFilter.MatchedClubMemberNumbers,
                    //        flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber)));
                    //}
                }
            }
            else
            {
                //if (string.IsNullOrWhiteSpace(flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber))
                //{
                //    Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                //}
                //else
                //{

                //    Conditions.Add(new Contains<string>(_invoiceRuleFilter.MatchedClubMemberNumbers,
                //        flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber));
                //}

                var persons = _flight.FlightCrews.Select(x => x.Person);
                var personClubs = persons.Select(p => p.PersonClubs.First(q => q.ClubId == flightInvoiceDetails.ClubId));

                Conditions.Add(new IntersectAny<string>(_invoiceRuleFilter.MatchedClubMemberNumbers,
                    personClubs.Select(pc => pc.MemberNumber)));
            }

            if (_invoiceRuleFilter.UseRuleForAllFlightCrewTypesExceptListed)
            {
                if (_invoiceRuleFilter.MatchedFlightCrewTypes.Any())
                {
                    if (_flight.FlightCrews.Any() == false)
                    {
                        Logger.Warn($"Flight has no flight crew. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(new Inverter(new IntersectAny<int>(_invoiceRuleFilter.MatchedFlightCrewTypes,
                            _flight.FlightCrews.Select(x => x.FlightCrewTypeId))));
                    }
                }
            }
            else
            {
                if (_flight.FlightCrews.Any() == false)
                {
                    Logger.Warn($"Flight has no flight crew. May we invoice something wrong!");
                }
                else
                {
                    Conditions.Add(new IntersectAny<int>(_invoiceRuleFilter.MatchedFlightCrewTypes,
                        _flight.FlightCrews.Select(x => x.FlightCrewTypeId)));
                }
            }
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}, Rule-Filter: {_invoiceRuleFilter.RuleFilterName} of type: {this.GetType()} has conditions: '{string.Join(",", Conditions.Select(x => x))}' for Flight: {Flight}";
        }
    }
}
