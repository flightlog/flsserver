using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class BaseRuleFilter
    {
        public BaseRuleFilter()
        {
            IsActive = true;
            AircraftImmatriculations = new List<string>();
            Aircrafts = new List<Guid>();
            MatchedFlightTypeCodes = new List<string>();
            MatchedStartLocations = new List<string>();
            MatchedLdgLocations = new List<string>();
            MatchedStartLocationIds = new List<Guid>();
            MatchedLdgLocationIds = new List<Guid>();
            MatchedClubMemberNumbers = new List<string>();
            MatchedFlightCrewTypes = new List<int>();
            UseRuleForAllStartLocationsExceptListed = true;
            UseRuleForAllLdgLocationsExceptListed = true;
            UseRuleForAllFlightCrewTypesExceptListed = true;
            UseRuleForAllAircraftsExceptListed = true;
            UseRuleForAllClubMemberNumbersExceptListed = true;
            UseRuleForAllFlightTypesExceptListed = true;
        }

        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public ArticleTarget ArticleTarget { get; set; }

        public RecipientDetails RecipientTarget { get; set; }

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

        public bool UseRuleForAllFlightCrewTypesExceptListed { get; set; }
        public List<int> MatchedFlightCrewTypes { get; set; }
    }
}