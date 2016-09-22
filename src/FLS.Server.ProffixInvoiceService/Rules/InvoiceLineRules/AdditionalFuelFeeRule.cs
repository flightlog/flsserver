using System;
using System.Linq;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.ProffixInvoiceService.RuleFilters;

namespace FLS.Server.ProffixInvoiceService.Rules.InvoiceLineRules
{
    internal class AdditionalFuelFeeRule : BaseInvoiceLineRule
    {

        internal AdditionalFuelFeeRule(Flight flight, AdditionalFuelFeeRuleFilter additionalFuelFee)
            : base(flight, additionalFuelFee)
        {
        }
        
        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ProffixArticleNumber))
            {
                //this case should never happened. It happens when multiple rules matches
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ProffixArticleNumber);
                line.Quantity += Convert.ToDecimal(Flight.Duration.TotalMinutes);

                Logger.Warn($"Invoice line already exists. Added quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = Flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = BaseInvoiceLineRuleFilter.ProffixArticleNumber;
                line.Quantity = Convert.ToDecimal(Flight.Duration.TotalMinutes);
                line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();
                line.InvoiceLineText = $"{BaseInvoiceLineRuleFilter.InvoiceLineText} {Flight.AircraftImmatriculation}";
                
                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
