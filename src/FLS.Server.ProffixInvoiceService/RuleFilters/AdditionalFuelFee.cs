using System;
using System.Collections.Generic;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    public class AdditionalFuelFee
    {
        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public List<Guid> AircraftIds { get; set; }
        public int SortIndicator { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        public List<Guid> MatchedStartLocations { get; set; }

        public AdditionalFuelFee()
        {
            MatchedStartLocations = new List<Guid>();
        }
    }
}
