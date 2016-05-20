using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.Conditions;
using FLS.Server.ProffixInvoiceService.RuleFilters;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    internal class NoLandingTaxRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly NoLandingTax _noLandingTax;

        internal NoLandingTaxRule(Flight flight, NoLandingTax noLandingTax)
        {
            _flight = flight;
            _noLandingTax = noLandingTax;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            ICondition condition = null;

            if (_noLandingTax.IsRuleForSelfstartedGliderFlights)
            {
                condition = new Equals<int>(_flight.StartTypeId.GetValueOrDefault(), (int) AircraftStartType.SelfStart);
            }

            if (_noLandingTax.IsRuleForGliderFlights)
            {
                condition = new Or(condition,
                    new Equals<int>(_flight.FlightAircraftType, (int) FlightAircraftTypeValue.GliderFlight));
            }

            if (_noLandingTax.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }
            
            if (condition != null)
            {
                Conditions.Add(condition);
            }

            if (_noLandingTax.UseRuleForAllAircraftsExceptListed)
            {
                if (_noLandingTax.AircraftIds.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_noLandingTax.AircraftIds, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_noLandingTax.AircraftIds, _flight.AircraftId));
            }

            if (_noLandingTax.UseRuleForAllFlightTypesExceptListed)
            {
                if (_noLandingTax.MatchedFlightTypeCodes.Any())
                {
                    condition = null;

                    if (_noLandingTax.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in _flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_noLandingTax.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (_flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_noLandingTax.MatchedFlightTypeCodes,
                                    _flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(new Inverter(new Or(condition, new Contains<string>(_noLandingTax.MatchedFlightTypeCodes, _flight.FlightType.FlightCode))));
                }
            }
            else
            {
                condition = null;

                if (_noLandingTax.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in _flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_noLandingTax.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (_flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_noLandingTax.MatchedFlightTypeCodes,
                                _flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition, new Contains<string>(_noLandingTax.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
            }

            if (_noLandingTax.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_noLandingTax.MatchedLdgLocations.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_noLandingTax.MatchedLdgLocations,
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
                    Conditions.Add(new Contains<Guid>(_noLandingTax.MatchedLdgLocations,
                        _flight.LdgLocationId.Value));
                }
            }

        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {

            flightInvoiceDetails.NoLandingTaxForGliderFlight = _noLandingTax.NoLandingTaxForGlider;
            flightInvoiceDetails.NoLandingTaxForTowFlight = _noLandingTax.NoLandingTaxForTowingAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {flightInvoiceDetails.NoLandingTaxForGliderFlight}, for towing to: {flightInvoiceDetails.NoLandingTaxForTowFlight}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
