using System;
using System.Collections.Generic;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    public class AircraftRuleFilter : BaseInvoiceLineRuleFilter
    {
        public int MinFlightTimeMatchingValue { get; set; }
        public int MaxFlightTimeMatchingValue { get; set; }
        
        public bool IncludeThresholdText { get; set; }

        public string ThresholdText { get; set; }

        public bool IncludeFlightTypeName { get; set; }

        public AircraftRuleFilter()
        {
            MinFlightTimeMatchingValue = 0;
            MaxFlightTimeMatchingValue = int.MaxValue;
        }
    }
}
