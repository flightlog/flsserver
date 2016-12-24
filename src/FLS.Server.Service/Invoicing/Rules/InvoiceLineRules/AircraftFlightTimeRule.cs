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
    internal class AircraftFlightTimeRule : BaseInvoiceRule
    {
        internal AircraftFlightTimeRule(Flight flight, InvoiceRuleFilterDetails aircraftMapping)
            : base(flight, aircraftMapping)
        {
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            InvoiceRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(flightInvoiceDetails);

            Conditions.Add(new Between<double>(flightInvoiceDetails.ActiveFlightTime, InvoiceRuleFilter.MinFlightTimeMatchingValue, InvoiceRuleFilter.MaxFlightTimeMatchingValue, includeMinValue:false, includeMaxValue:true));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            var lineQuantity = 0.0m;

            if (InvoiceRuleFilter.MinFlightTimeMatchingValue == 0)
            {
                lineQuantity = Convert.ToDecimal(flightInvoiceDetails.ActiveFlightTime);
                flightInvoiceDetails.ActiveFlightTime = 0;
            }
            else
            {
                lineQuantity = Convert.ToDecimal(flightInvoiceDetails.ActiveFlightTime - InvoiceRuleFilter.MinFlightTimeMatchingValue);
                flightInvoiceDetails.ActiveFlightTime = InvoiceRuleFilter.MinFlightTimeMatchingValue;
            }

            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ArticleNumber == InvoiceRuleFilter.ArticleTarget.ArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ArticleNumber == InvoiceRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity += lineQuantity;

                Logger.Warn($"Invoice line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ArticleNumber = InvoiceRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = lineQuantity;
                line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

                if (InvoiceRuleFilter.IncludeThresholdText)
                {
                    if (InvoiceRuleFilter.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{Flight.AircraftImmatriculation} {InvoiceRuleFilter.ArticleTarget.InvoiceLineText} {Flight.FlightType.FlightTypeName} {InvoiceRuleFilter.ThresholdText}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{Flight.AircraftImmatriculation} {InvoiceRuleFilter.ArticleTarget.InvoiceLineText} {InvoiceRuleFilter.ThresholdText}";
                    }
                }
                else
                {
                    if (InvoiceRuleFilter.IncludeFlightTypeName)
                    {
                        line.InvoiceLineText =
                            $"{Flight.AircraftImmatriculation} {InvoiceRuleFilter.ArticleTarget.InvoiceLineText} {Flight.FlightType.FlightTypeName}";
                    }
                    else
                    {
                        line.InvoiceLineText = $"{Flight.AircraftImmatriculation} {InvoiceRuleFilter.ArticleTarget.InvoiceLineText}";
                    }
                }

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
