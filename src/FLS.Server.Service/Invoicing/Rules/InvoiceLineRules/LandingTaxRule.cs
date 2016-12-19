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
    internal class LandingTaxRule : BaseInvoiceRule
    {
        internal LandingTaxRule(Flight flight, InvoiceRuleFilterDetails landingTax)
            : base(flight, landingTax)
        {
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            InvoiceRuleFilter.ArticleTarget.NotNull("ArticleTarget");
            base.Initialize(flightInvoiceDetails);

            if (flightInvoiceDetails.NoLandingTaxForGliderFlight &&
                Flight.FlightAircraftType == (int) FlightAircraftTypeValue.GliderFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }

            if (flightInvoiceDetails.NoLandingTaxForTowFlight &&
                Flight.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight)
            {
                //rule must not be applied
                Conditions.Add(new Equals<bool>(false, true));
            }
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (flightInvoiceDetails.FlightInvoiceLineItems.Any(x => x.ERPArticleNumber == InvoiceRuleFilter.ArticleTarget.ArticleNumber))
            {
                var line = flightInvoiceDetails.FlightInvoiceLineItems.First(x => x.ERPArticleNumber == InvoiceRuleFilter.ArticleTarget.ArticleNumber);
                line.Quantity++;

                Logger.Warn($"Invoice line for landing tax already exists. Add quantity to the existing line! New line value: {line}");
            }
            else
            {
                var line = new FlightInvoiceLineItem();
                line.FlightId = Flight.FlightId;
                line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                line.ERPArticleNumber = InvoiceRuleFilter.ArticleTarget.ArticleNumber;
                line.Quantity = 1.0m;
                line.UnitType = CostCenterUnitType.PerLanding.ToUnitTypeString();
                line.InvoiceLineText = $"{InvoiceRuleFilter.ArticleTarget.InvoiceLineText}";

                flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

                Logger.Debug($"Added new invoice item line to invoice. Line: {line}");
            }
            
            return base.Apply(flightInvoiceDetails);
        }
    }
}
