using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal class VsfFeeRule : BaseInvoiceLineRule
    {
        internal VsfFeeRule(Flight flight, VsfFeeRuleFilter vsfFee)
            : base(flight, vsfFee)
        {
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            BaseInvoiceLineRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(flightInvoiceDetails);
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber))
            {
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity++;

                Logger.Info($"Invoice line for VSF fee already exists. Add quantity to the existing line! New line values: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = Flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = BaseInvoiceLineRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = 1.0m;
                line.UnitType = CostCenterUnitType.PerLanding.ToUnitTypeString();
                line.InvoiceLineText = $"{BaseInvoiceLineRuleFilter.ArticleTarget.InvoiceLineText}";

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
