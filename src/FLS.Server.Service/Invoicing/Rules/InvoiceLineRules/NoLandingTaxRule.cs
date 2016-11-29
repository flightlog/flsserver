using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Invoicing.Rules.InvoiceLineRules
{
    internal class NoLandingTaxRule : BaseInvoiceRule
    {
        private readonly NoLandingTaxRuleFilter _noLandingTax;

        internal NoLandingTaxRule(Flight flight, NoLandingTaxRuleFilter noLandingTax)
            : base(flight, noLandingTax)
        {
            _noLandingTax = noLandingTax;
        }
        
        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {

            flightInvoiceDetails.NoLandingTaxForGliderFlight = _noLandingTax.NoLandingTaxForGlider;
            flightInvoiceDetails.NoLandingTaxForTowFlight = _noLandingTax.NoLandingTaxForTowingAircraft;
            flightInvoiceDetails.NoLandingTaxForFlight = _noLandingTax.NoLandingTaxForAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {flightInvoiceDetails.NoLandingTaxForGliderFlight}, for towing to: {flightInvoiceDetails.NoLandingTaxForTowFlight}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
