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
    internal class VsfFeeRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly VsfFee _vsfFee;

        internal VsfFeeRule(Flight flight, VsfFee vsfFee)
        {
            _flight = flight;
            _vsfFee = vsfFee;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (_vsfFee.UseRuleForAllLdgLocationsExceptListed)
            {
                if (_vsfFee.MatchedLdgLocations.Any())
                {
                    if (_flight.LdgLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no landing location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_vsfFee.MatchedLdgLocations,
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
                    Conditions.Add(new Contains<Guid>(_vsfFee.MatchedLdgLocations,
                        _flight.LdgLocationId.Value));
                }
            }

        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == _vsfFee.ERPArticleNumber))
            {
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == _vsfFee.ERPArticleNumber);
                line.Quantity++;

                Logger.Info($"Invoice line for VSF fee already exists. Add quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = _flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = _vsfFee.ERPArticleNumber;
                line.Quantity = 1.0m;
                line.UnitType = CostCenterUnitType.PerLanding.ToUnitTypeString();
                line.InvoiceLineText = $"{_vsfFee.InvoiceLineText}";

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
