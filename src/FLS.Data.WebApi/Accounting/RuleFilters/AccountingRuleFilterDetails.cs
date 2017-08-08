using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.Accounting.RuleFilters
{
    public class AccountingRuleFilterDetails : FLSBaseData
    {
        public AccountingRuleFilterDetails()
        {
            IsActive = true;
            MatchedAircraftImmatriculations = new List<string>();
            MatchedFlightTypeCodes = new List<string>();
            MatchedStartLocations = new List<string>();
            MatchedLdgLocations = new List<string>();
            MatchedClubMemberNumbers = new List<string>();
            MatchedFlightCrewTypes = new List<int>();
            MatchedStartTypes = new List<int>();
            MatchedAircraftsHomebase = new List<string>();
            MatchedMemberStates = new List<Guid>();
            MatchedPersonCategories = new List<Guid>();
            UseRuleForAllStartLocationsExceptListed = true;
            UseRuleForAllLdgLocationsExceptListed = true;
            UseRuleForAllFlightCrewTypesExceptListed = true;
            UseRuleForAllAircraftsExceptListed = true;
            UseRuleForAllClubMemberNumbersExceptListed = true;
            UseRuleForAllFlightTypesExceptListed = true;
            UseRuleForAllStartTypesExceptListed = true;
            UseRuleForAllAircraftsOnHomebaseExceptListed = true;
            UseRuleForAllMemberStatesExceptListed = true;
            UseRuleForAllPersonCategoriesExceptListed = true;
            MinFlightTimeInSecondsMatchingValue = 0;
            MaxFlightTimeInSecondsMatchingValue = int.MaxValue;
            MinEngineTimeInSecondsMatchingValue = 0;
            MaxEngineTimeInSecondsMatchingValue = int.MaxValue;
        }

        public Guid AccountingRuleFilterId { get; set; }

        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public int AccountingRuleFilterTypeId { get; set; }

        public bool StopRuleEngineWhenRuleApplied { get; set; }
        public bool IsRuleForGliderFlights { get; set; }
        public bool IsRuleForTowingFlights { get; set; }

        public bool IsRuleForMotorFlights { get; set; }

        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public List<string> MatchedAircraftImmatriculations { get; set; }

        public bool UseRuleForAllStartTypesExceptListed { get; set; }

        public List<int> MatchedStartTypes { get; set; }

        public bool UseRuleForAllFlightTypesExceptListed { get; set; }
        public List<string> MatchedFlightTypeCodes { get; set; }

        public bool ExtendMatchingFlightTypeCodesToGliderAndTowFlight { get; set; }

        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the start locations ICAO codes for this rule filter
        /// </summary>
        public List<string> MatchedStartLocations { get; set; }
        
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the landing locations ICAO codes for this rule filter
        /// </summary>
        public List<string> MatchedLdgLocations { get; set; }
        
        public bool UseRuleForAllClubMemberNumbersExceptListed { get; set; }
        public List<string> MatchedClubMemberNumbers { get; set; }

        public bool UseRuleForAllFlightCrewTypesExceptListed { get; set; }
        public List<int> MatchedFlightCrewTypes { get; set; }

        public int? AccountingUnitTypeId { get; set; }

        public bool UseRuleForAllAircraftsOnHomebaseExceptListed { get; set; }

        public List<string> MatchedAircraftsHomebase { get; set; }

        public bool UseRuleForAllMemberStatesExceptListed { get; set; }

        public List<Guid> MatchedMemberStates { get; set; }

        public bool UseRuleForAllPersonCategoriesExceptListed { get; set; }

        public List<Guid> MatchedPersonCategories { get; set; }

        #region Recipient rule part
        public RecipientDetails RecipientTarget { get; set; }

        public bool IsChargedToClubInternal { get; set; }
        #endregion Recipient rule part

        #region invoice line rule part
        public ArticleTargetDetails ArticleTarget { get; set; }

        public int? MinFlightTimeInSecondsMatchingValue { get; set; }
        public int? MaxFlightTimeInSecondsMatchingValue { get; set; }

        public int? MinEngineTimeInSecondsMatchingValue { get; set; }
        public int? MaxEngineTimeInSecondsMatchingValue { get; set; }

        public bool IncludeThresholdText { get; set; }

        [StringLength(250)]
        public string ThresholdText { get; set; }

        public bool IncludeFlightTypeName { get; set; }

        public bool NoLandingTaxForGlider { get; set; }
        public bool NoLandingTaxForTowingAircraft { get; set; }

        public bool NoLandingTaxForAircraft { get; set; }
        #endregion invoice line rule part

        public override Guid Id
        {
            get { return AccountingRuleFilterId; }
            set { AccountingRuleFilterId = value; }
            
        }
    }
}