namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class NoLandingTaxRuleFilter : BaseRuleFilter
    {
        public bool NoLandingTaxForGlider { get; set; }
        public bool NoLandingTaxForTowingAircraft { get; set; }

        public bool NoLandingTaxForAircraft { get; set; }
    }
}
