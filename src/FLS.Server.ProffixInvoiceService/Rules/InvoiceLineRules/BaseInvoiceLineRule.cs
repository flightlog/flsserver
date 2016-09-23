using System;
using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.Conditions;
using FLS.Server.ProffixInvoiceService.RuleFilters;

namespace FLS.Server.ProffixInvoiceService.Rules.InvoiceLineRules
{
    internal abstract class BaseInvoiceLineRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly BaseInvoiceLineRuleFilter _invoiceLineRuleFilter;

        protected Flight Flight { get { return _flight; } }
        protected BaseInvoiceLineRuleFilter BaseInvoiceLineRuleFilter { get { return _invoiceLineRuleFilter; } }
        internal BaseInvoiceLineRule(Flight flight, BaseInvoiceLineRuleFilter invoiceLineRuleFilter)
        {
            _flight = flight;
            _invoiceLineRuleFilter = invoiceLineRuleFilter;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            ICondition condition = null;

            if (BaseInvoiceLineRuleFilter.IsRuleForSelfstartedGliderFlights)
            {
                condition = new Equals<int>(_flight.StartTypeId.GetValueOrDefault(), (int)AircraftStartType.SelfStart);
            }

            if (BaseInvoiceLineRuleFilter.IsRuleForGliderFlights)
            {
                condition = new Or(condition,
                    new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.GliderFlight));
            }

            if (BaseInvoiceLineRuleFilter.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }

            if (condition != null)
            {
                Conditions.Add(condition);
                condition = null;
            }

            if (_invoiceLineRuleFilter.UseRuleForAllAircraftsExceptListed)
            {
                if (_invoiceLineRuleFilter.AircraftImmatriculations.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_invoiceLineRuleFilter.Aircrafts, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_invoiceLineRuleFilter.Aircrafts, _flight.AircraftId));
            }


            //Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, _invoiceLineRuleFilter.MinFlightTimeMatchingValue, _invoiceLineRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            if (_invoiceLineRuleFilter.UseRuleForAllFlightTypesExceptListed)
            {
                if (_invoiceLineRuleFilter.MatchedFlightTypeCodes.Any())
                {
                    condition = null;

                    if (_invoiceLineRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in _flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (_flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes,
                                    _flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(new Inverter(new Or(condition, new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode))));
                }
            }
            else
            {
                condition = null;

                if (_invoiceLineRuleFilter.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in _flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (_flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes,
                                _flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition, new Contains<string>(_invoiceLineRuleFilter.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
            }

            if (_invoiceLineRuleFilter.UseRuleForAllStartLocationsExceptListed)
            {
                if (_invoiceLineRuleFilter.MatchedStartLocations.Any())
                {
                    if (_flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_invoiceLineRuleFilter.MatchedStartLocationIds,
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
                    Conditions.Add(new Contains<Guid>(_invoiceLineRuleFilter.MatchedStartLocationIds,
                        _flight.StartLocationId.Value));
                }
            }

            if (_invoiceLineRuleFilter.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_invoiceLineRuleFilter.MatchedLdgLocations.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_invoiceLineRuleFilter.MatchedLdgLocationIds,
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
                    Conditions.Add(new Contains<Guid>(_invoiceLineRuleFilter.MatchedLdgLocationIds,
                        _flight.LdgLocationId.Value));
                }
            }

            if (_invoiceLineRuleFilter.UseRuleForAllClubMemberNumbersExceptListed)
            {
                if (_invoiceLineRuleFilter.MatchedClubMemberNumbers.Any())
                {
                    if (string.IsNullOrWhiteSpace(flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber))
                    {
                        Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                    }
                    else
                    {

                        Conditions.Add(new Inverter(new Contains<string>(_invoiceLineRuleFilter.MatchedClubMemberNumbers,
                            flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber)));
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber))
                {
                    Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                }
                else
                {

                    Conditions.Add(new Contains<string>(_invoiceLineRuleFilter.MatchedClubMemberNumbers,
                        flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber));
                }
            }

        }
        
    }
}
