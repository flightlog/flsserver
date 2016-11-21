namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class AircraftRuleFilter : BaseRuleFilter
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
