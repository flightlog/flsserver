using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class InvoiceLineRuleFilterDetails : InvoiceRuleFilterDetails
    {
        public InvoiceLineRuleFilterDetails()
        {
            MinFlightTimeMatchingValue = 0;
            MaxFlightTimeMatchingValue = int.MaxValue;
        }

        public InvoiceLineRuleFilterDetails(InvoiceRuleFilterDetails baseInvoiceRuleFilterDetails, ArticleTargetDetails articleTarget,
            int minFlightTimeMatchingValue, int maxFlightTimeMatchingValue, bool includeThresholdText, string thresholdText,
            bool includeFlightTypeName, bool noLandingTaxForGlider, bool noLandingTaxForTowingAircraft, bool noLandingTaxForAircraft)
            : base(baseInvoiceRuleFilterDetails)
        {
            ArticleTarget = articleTarget;
            MinFlightTimeMatchingValue = minFlightTimeMatchingValue;
            MaxFlightTimeMatchingValue = maxFlightTimeMatchingValue;
            IncludeThresholdText = includeThresholdText;
            ThresholdText = thresholdText;
            IncludeFlightTypeName = includeFlightTypeName;
            NoLandingTaxForGlider = noLandingTaxForGlider;
            NoLandingTaxForTowingAircraft = noLandingTaxForTowingAircraft;
            NoLandingTaxForAircraft = noLandingTaxForAircraft;
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