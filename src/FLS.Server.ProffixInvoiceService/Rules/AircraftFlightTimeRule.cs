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
            Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, _aircraftMapping.MinFlightTimeMatchingValue, _aircraftMapping.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));

            if (_aircraftMapping.UseRuleForAllFlightTypesExceptListed)
            {
                if (_aircraftMapping.MatchedFlightTypeCodes.Any())
                {
                    ICondition condition = null;

                    if (_aircraftMapping.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                    {
                        foreach (var towedFlight in _flight.TowedFlights)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes,
                                    towedFlight.FlightType.FlightCode));
                        }

                        if (_flight.TowFlight != null)
                        {
                            condition = new Or(condition,
                                new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes,
                                    _flight.TowFlight.FlightType.FlightCode));
                        }
                    }

                    Conditions.Add(new Inverter(new Or(condition, new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes, _flight.FlightType.FlightCode))));
                }
            }
            else
            {
                ICondition condition = null;

                if (_aircraftMapping.ExtendMatchingFlightTypeCodesToGliderAndTowFlight)
                {
                    foreach (var towedFlight in _flight.TowedFlights)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes,
                                towedFlight.FlightType.FlightCode));
                    }

                    if (_flight.TowFlight != null)
                    {
                        condition = new Or(condition,
                            new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes,
                                _flight.TowFlight.FlightType.FlightCode));
                    }
                }

                Conditions.Add(new Or(condition, new Contains<string>(_aircraftMapping.MatchedFlightTypeCodes, _flight.FlightType.FlightCode)));
            }

            if (_aircraftMapping.UseRuleForAllStartLocationsExceptListed)
            {
                if (_aircraftMapping.MatchedStartLocations.Any())
                {
                    if (_flight.StartLocationId.HasValue == false)
                    {
                        Logger.Warn($"Flight has no start location set. May we invoice something wrong!");
                    }
                    else
                    {
                        Conditions.Add(
                            new Inverter(new Contains<Guid>(_aircraftMapping.MatchedStartLocations,
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
                    Conditions.Add(new Contains<Guid>(_aircraftMapping.MatchedStartLocations,
                        _flight.StartLocationId.Value));
                }
            }

            if (_aircraftMapping.UseRuleForAllClubMemberNumbersExceptListed)
            {
                if (_aircraftMapping.MatchedClubMemberNumbers.Any())
                {
                    if (string.IsNullOrWhiteSpace(flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber))
                    {
                        Logger.Info($"Invoice has no recipient with club member number! Condition for club member number will not be added.");
                    }
                    else
                    {

                        Conditions.Add(new Inverter(new Contains<string>(_aircraftMapping.MatchedClubMemberNumbers,
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

                    Conditions.Add(new Contains<string>(_aircraftMapping.MatchedClubMemberNumbers,
                        flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber));
                }
            }

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
                lineQuantity = Convert.ToDecimal(flightInvoiceDetails.ActiveFlightTime - _aircraftMapping.MinFlightTimeMatchingValue);
                flightInvoiceDetails.ActiveFlightTime = _aircraftMapping.MinFlightTimeMatchingValue;
            }

            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == _aircraftMapping.ERPArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == _aircraftMapping.ERPArticleNumber);
                line.Quantity += lineQuantity;

                Logger.Warn($"Invoice line already exists. Added quantity to the existing line! New line values: {line}");
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

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
