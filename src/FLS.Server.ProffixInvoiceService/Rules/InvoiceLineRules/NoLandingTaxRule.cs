using System;
using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.Conditions;
using FLS.Server.ProffixInvoiceService.RuleFilters;

namespace FLS.Server.ProffixInvoiceService.Rules.InvoiceLineRules
{
    internal class NoLandingTaxRule : BaseInvoiceLineRule
    {
        private readonly NoLandingTaxRuleFilter _noLandingTax;

        internal NoLandingTaxRule(Flight flight, NoLandingTaxRuleFilter noLandingTax)
            : base(flight, noLandingTax)
        {
            _noLandingTax = noLandingTax;
        }
        
        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {

            flightInvoiceDetails.NoLandingTaxForGliderFlight = _noLandingTax.NoLandingTaxForGlider;
            flightInvoiceDetails.NoLandingTaxForTowFlight = _noLandingTax.NoLandingTaxForTowingAircraft;
            flightInvoiceDetails.NoLandingTaxForFlight = _noLandingTax.NoLandingTaxForAircraft;

            Logger.Debug($"Apply no landing tax! Set NO landing tax for glider to : {flightInvoiceDetails.NoLandingTaxForGliderFlight}, for towing to: {flightInvoiceDetails.NoLandingTaxForTowFlight}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
