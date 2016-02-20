using System;
using System.Collections.Generic;

namespace FLS.Server.Service.Invoicing
{
    public class LandingTaxRule
    {
        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public List<Guid> AircraftIds { get; set; }
        public int SortIndicator { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllFlightTypesExceptListed { get; set; }
        public List<string> MatchedFlightTypeCodes { get; set; }
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        public List<Guid> MatchedLdgLocations { get; set; }
        public bool IsRuleForSelfstartedGliderFlights { get; set; }
        public bool IncludesTowingLandingTaxes { get; set; }

        public LandingTaxRule()
        {
            AircraftIds = new List<Guid>();
            MatchedFlightTypeCodes = new List<string>();
            MatchedLdgLocations = new List<Guid>();
        }
    }
}
