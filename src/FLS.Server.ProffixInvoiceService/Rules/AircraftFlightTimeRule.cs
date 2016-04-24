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
    internal class AircraftFlightTimeRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly AircraftMapping _aircraftMapping;

        internal AircraftFlightTimeRule(Flight flight, AircraftMapping aircraftMapping)
        {
            _flight = flight;
            _aircraftMapping = aircraftMapping;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<Guid>(_flight.AircraftId, _aircraftMapping.AircraftId));
            Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, _aircraftMapping.MinFlightTimeMatchingValue, _aircraftMapping.MaxFlightTimeMatchingValue));

            if (_aircraftMapping.UseRuleForAllFlightTypesExceptListed)
            {
                if (_aircraftMapping.MatchedFlightTypeCodes.Any())
                {
                    Conditions.Add(new Inverter(new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
                }
            }
            else
            {
                Conditions.Add(new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes, _flight.FlightType.FlightCode));
            }    

            //TODO: add further conditions for locations, etc.
        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            var lineQuantity = 0.0m;

            if (_aircraftMapping.MinFlightTimeMatchingValue == 0)
            {
                lineQuantity = Convert.ToDecimal(flightInvoiceDetails.ActiveFlightTime);
                flightInvoiceDetails.ActiveFlightTime = 0;
            }
            else
            {
                flightInvoiceDetails.ActiveFlightTime -= _aircraftMapping.MinFlightTimeMatchingValue;
                lineQuantity = Convert.ToDecimal(flightInvoiceDetails.ActiveFlightTime);
            }

            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == _aircraftMapping.ERPArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                Logger.Warn($"Invoice line already exists. Add quantity to the existing line!");
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == _aircraftMapping.ERPArticleNumber);
                line.Quantity += lineQuantity;
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = _flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = _aircraftMapping.ERPArticleNumber;
                line.Quantity = lineQuantity;
                line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

                if (_aircraftMapping.IncludeThresholdText)
                {
                    if (_aircraftMapping.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{_flight.AircraftImmatriculation} {_aircraftMapping.InvoiceLineText} {_flight.FlightType.FlightTypeName} {_aircraftMapping.ThresholdText}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{_flight.AircraftImmatriculation} {_aircraftMapping.InvoiceLineText} {_aircraftMapping.ThresholdText}";
                    }
                }
                else
                {
                    if (_aircraftMapping.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{_flight.AircraftImmatriculation} {_aircraftMapping.InvoiceLineText} {_flight.FlightType.FlightTypeName}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{_flight.AircraftImmatriculation} {_aircraftMapping.InvoiceLineText}";
                    }
                }

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
