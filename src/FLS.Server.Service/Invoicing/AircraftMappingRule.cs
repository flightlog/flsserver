using System;
using System.Collections.Generic;

namespace FLS.Server.Service.Invoicing
{
    public class AircraftMappingRule
    {
        public Guid AircraftId { get; set; }
        //public string AircraftImmatriculation { get; set; }
        public int SortIndicator { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllFlightTypesExceptListed { get; set; }
        public List<string> MatchedFlightTypeCodes { get; set; }
        public int MinFlightTimeMatchingValue { get; set; }
        public int MaxFlightTimeMatchingValue { get; set; }
        public bool UseRuleBelowFlightTimeMatchingValue { get; set; }
        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        public List<Guid> MatchedStartLocations { get; set; }

        public AircraftMappingRule()
        {
            MatchedFlightTypeCodes = new List<string>();
            MatchedStartLocations = new List<Guid>();
            MinFlightTimeMatchingValue = 1;
            MaxFlightTimeMatchingValue = int.MaxValue;
            UseRuleForAllStartLocationsExceptListed = true;
        }
    }
}
