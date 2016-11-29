using System;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal class InstructorFeeRule : BaseInvoiceRule
    {
        internal InstructorFeeRule(Flight flight, InstructorFeeRuleFilter instructorFeeRuleFilter)
            : base(flight, instructorFeeRuleFilter)
        {
        }
        
        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            var line = new FlightInvoiceLineItem();
            line.FlightId = Flight.FlightId;
            line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
            line.ERPArticleNumber = BaseInvoiceRuleFilter.ArticleTarget.ArticleNumber;
            line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

            line.InvoiceLineText = $"Fluglehrer-Honorar {Flight.InstructorDisplayName}";

            if (Flight.FlightCostBalanceTypeId.HasValue &&
                                Flight.FlightCostBalanceTypeId.Value ==
                                (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee)
            {
                //no instructor fee for this flight, so set quantity to 0
                line.Quantity = 0;
            }
            else
            {
                line.Quantity = Convert.ToDecimal(Flight.FlightDurationZeroBased.TotalMinutes);
            }

            flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

            Logger.Debug($"Added new invoice item line to invoice. Line: {line}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
