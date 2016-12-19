using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal class NoLandingTaxRule : BaseInvoiceRule
    {
        internal NoLandingTaxRule(Flight flight, InvoiceRuleFilterDetails noLandingTax)
            : base(flight, noLandingTax)
        {
        }
        
        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {

            flightInvoiceDetails.NoLandingTaxForGliderFlight = InvoiceRuleFilter.NoLandingTaxForGlider;
            flightInvoiceDetails.NoLandingTaxForTowFlight = InvoiceRuleFilter.NoLandingTaxForTowingAircraft;
            flightInvoiceDetails.NoLandingTaxForFlight = InvoiceRuleFilter.NoLandingTaxForAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {flightInvoiceDetails.NoLandingTaxForGliderFlight}, for towing to: {flightInvoiceDetails.NoLandingTaxForTowFlight}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
