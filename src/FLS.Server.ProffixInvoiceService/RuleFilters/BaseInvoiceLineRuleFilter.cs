using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    public class BaseInvoiceLineRuleFilter
    {
        public BaseInvoiceLineRuleFilter()
        {
            AircraftImmatriculations = new List<string>();
            Aircrafts = new List<Guid>();
            MatchedFlightTypeCodes = new List<string>();
            MatchedStartLocations = new List<string>();
            MatchedLdgLocations = new List<string>();
            MatchedStartLocationIds = new List<Guid>();
            MatchedLdgLocationIds = new List<Guid>();
            MatchedClubMemberNumbers = new List<string>();
            UseRuleForAllStartLocationsExceptListed = true;
            UseRuleForAllLdgLocationsExceptListed = true;
        }

        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public string ProffixArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }

        public bool IsRuleForSelfstartedGliderFlights { get; set; }
        public bool IsRuleForGliderFlights { get; set; }
        public bool IsRuleForTowingFlights { get; set; }

        public bool IsRuleForMotorFlights { get; set; }

        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public List<string> AircraftImmatriculations { get; set; }

        [JsonIgnore]
        public List<Guid> Aircrafts { get; set; }
        public bool UseRuleForAllFlightTypesExceptListed { get; set; }
        public List<string> MatchedFlightTypeCodes { get; set; }

        public bool ExtendMatchingFlightTypeCodesToGliderAndTowFlight { get; set; }

        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the start locations ICAO codes for this rule filter
        /// </summary>
        public List<string> MatchedStartLocations { get; set; }

        [JsonIgnore]
        public List<Guid> MatchedStartLocationIds { get; set; }
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the landing locations ICAO codes for this rule filter
        /// </summary>
        public List<string> MatchedLdgLocations { get; set; }

        [JsonIgnore]
        public List<Guid> MatchedLdgLocationIds { get; set; }

        public bool UseRuleForAllClubMemberNumbersExceptListed { get; set; }
        public List<string> MatchedClubMemberNumbers { get; set; }
    }
}