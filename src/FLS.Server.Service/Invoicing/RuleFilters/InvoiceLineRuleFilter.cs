using System.ComponentModel.DataAnnotations;
using FLS.Data.WebApi.Invoicing;

namespace FLS.Server.Service.Invoicing.RuleFilters
{
    public class InvoiceLineRuleFilter : InvoiceRuleFilter
    {
        public InvoiceLineRuleFilter()
        {
            MinFlightTimeMatchingValue = 0;
            MaxFlightTimeMatchingValue = int.MaxValue;
        }
        
        public ArticleTargetDetails ArticleTarget { get; set; }

        public int MinFlightTimeMatchingValue { get; set; }
        public int MaxFlightTimeMatchingValue { get; set; }

        public bool IncludeThresholdText { get; set; }

        [StringLength(250)]
        public string ThresholdText { get; set; }

        public bool IncludeFlightTypeName { get; set; }

        public bool NoLandingTaxForGlider { get; set; }
        public bool NoLandingTaxForTowingAircraft { get; set; }

        public bool NoLandingTaxForAircraft { get; set; }
    }
}