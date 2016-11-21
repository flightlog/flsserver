using System;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal class AircraftFlightTimeRule : BaseInvoiceLineRule
    {
        private readonly AircraftRuleFilter _aircraftMapping;

        internal AircraftFlightTimeRule(Flight flight, AircraftRuleFilter aircraftMapping)
            : base(flight, aircraftMapping)
        {
            _aircraftMapping = aircraftMapping;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            BaseInvoiceLineRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(flightInvoiceDetails);

            Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, _aircraftMapping.MinFlightTimeMatchingValue, _aircraftMapping.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
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

            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity += lineQuantity;

                Logger.Warn($"Invoice line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = Flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = lineQuantity;
                line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

                if (_aircraftMapping.IncludeThresholdText)
                {
                    if (_aircraftMapping.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{Flight.AircraftImmatriculation} {BaseInvoiceLineRuleFilter.ArticleTarget.InvoiceLineText} {Flight.FlightType.FlightTypeName} {_aircraftMapping.ThresholdText}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{Flight.AircraftImmatriculation} {BaseInvoiceLineRuleFilter.ArticleTarget.InvoiceLineText} {_aircraftMapping.ThresholdText}";
                    }
                }
                else
                {
                    if (_aircraftMapping.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{Flight.AircraftImmatriculation} {BaseInvoiceLineRuleFilter.ArticleTarget.InvoiceLineText} {Flight.FlightType.FlightTypeName}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{Flight.AircraftImmatriculation} {BaseInvoiceLineRuleFilter.ArticleTarget.InvoiceLineText}";
                    }
                }

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
