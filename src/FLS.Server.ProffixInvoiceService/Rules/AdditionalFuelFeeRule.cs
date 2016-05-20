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
    internal class AdditionalFuelFeeRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly AdditionalFuelFee _additionalFuelFee;

        internal AdditionalFuelFeeRule(Flight flight, AdditionalFuelFee additionalFuelFee)
        {
            _flight = flight;
            _additionalFuelFee = additionalFuelFee;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (_additionalFuelFee.UseRuleForAllAircraftsExceptListed)
            {
                if (_additionalFuelFee.AircraftIds.Any())
                {
                    Conditions.Add(new Inverter(new Contains<Guid>(_additionalFuelFee.AircraftIds, _flight.AircraftId)));
                }
            }
            else
            {
                Conditions.Add(new Contains<Guid>(_additionalFuelFee.AircraftIds, _flight.AircraftId));
            }
            
            if (_additionalFuelFee.UseRuleForAllStartLocationsExceptListed)
            {
                if (_additionalFuelFee.MatchedStartLocations.Any())
                {
                    if (_flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_additionalFuelFee.MatchedStartLocations,
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
                    Conditions.Add(new Contains<Guid>(_additionalFuelFee.MatchedStartLocations,
                        _flight.StartLocationId.Value));
                }
            }

        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == _additionalFuelFee.ERPArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == _additionalFuelFee.ERPArticleNumber);
                line.Quantity += Convert.ToDecimal(_flight.Duration.TotalMinutes);

                Logger.Warn($"Invoice line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = _flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = _additionalFuelFee.ERPArticleNumber;
                line.Quantity = Convert.ToDecimal(_flight.Duration.TotalMinutes);
                line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();
                line.InvoiceLineText = $"{_additionalFuelFee.InvoiceLineText} {_flight.AircraftImmatriculation}";
                
                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
