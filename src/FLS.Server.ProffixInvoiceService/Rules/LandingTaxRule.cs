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
    internal class LandingTaxRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly LandingTax _landingTax;

        internal LandingTaxRule(Flight flight, LandingTax landingTax)
        {
            _flight = flight;
            _landingTax = landingTax;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            ICondition condition = null;

            if (flightInvoiceDetails.NoLandingTaxForGliderFlight &&
                _flight.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }

            if (flightInvoiceDetails.NoLandingTaxForTowFlight &&
                _flight.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }

            if (_landingTax.IsRuleForSelfstartedGliderFlights)
            {
                condition = new Equals<int>(_flight.StartTypeId.GetValueOrDefault(), (int) AircraftStartType.SelfStart);
            }

            if (_landingTax.IsRuleForGliderFlights)
            {
                condition = new Or(condition,
                    new Equals<int>(_flight.FlightAircraftType, (int) FlightAircraftTypeValue.GliderFlight));
            }

            if (_landingTax.IsRuleForTowingFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.TowFlight));
            }

            if (_landingTax.IsRuleForMotorFlights)
            {
                condition = new Or(condition, new Equals<int>(_flight.FlightAircraftType, (int)FlightAircraftTypeValue.MotorFlight));
            }

            if (condition != null)
            {
                Conditions.Add(condition);
            }

            if (_landingTax.UseRuleForAllAircraftsExceptListed)
            {
                if (_landingTax.AircraftIds.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_landingTax.AircraftIds, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_landingTax.AircraftIds, _flight.AircraftId));
            }

            if (_landingTax.UseRuleForAllFlightTypesExceptListed)
            {
                if (_landingTax.MatchedFlightTypeCodes.Any())
                {
                    Conditions.Add(new Inverter(new Contains<string>(_landingTax.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
                }
            }
            else
            {
                Conditions.Add(new Contains<string>(_landingTax.MatchedFlightTypeCodes, _flight.FlightType.FlightCode));
            }

            if (_landingTax.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_landingTax.MatchedLdgLocations.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_landingTax.MatchedLdgLocations,
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
                    Conditions.Add(new Contains<Guid>(_landingTax.MatchedLdgLocations,
                        _flight.LdgLocationId.Value));
                }
            }

        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == _landingTax.ERPArticleNumber))
            {
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == _landingTax.ERPArticleNumber);
                line.Quantity++;

                Logger.Warn($"Invoice line for landing tax already exists. Add quantity to the existing line! New line value: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = _flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = _landingTax.ERPArticleNumber;
                line.Quantity = 1.0m;
                line.UnitType = CostCenterUnitType.PerLanding.ToUnitTypeString();
                line.InvoiceLineText = $"{_landingTax.InvoiceLineText}";

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
