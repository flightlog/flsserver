﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;

namespace FLS.Server.Data.DbEntities
{
    public partial class AccountingRuleFilter : IFLSMetaData
    {
        public AccountingRuleFilter()
        {
            
        }

        public Guid AccountingRuleFilterId { get; set; }

        public Guid ClubId { get; set; }

        public int AccountingRuleFilterTypeId { get; set; }

        [StringLength(250)]
        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public string ArticleTarget { get; set; }

        public string RecipientTarget { get; set; }

        //TODO: Implement logic together with SortIndicator
        public bool StopRuleEngineWhenRuleApplied { get; set; }

        public bool IsRuleForGliderFlights { get; set; }
        public bool IsRuleForTowingFlights { get; set; }

        public bool IsRuleForMotorFlights { get; set; }

        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public string MatchedAircraftImmatriculations { get; set; }

        public bool UseRuleForAllStartTypesExceptListed { get; set; }

        public string MatchedStartTypes { get; set; }

        public bool UseRuleForAllFlightTypesExceptListed { get; set; }

        //[Column("MatchedFlightTypeCodes")]
        public string MatchedFlightTypeCodes { get; set; }

        public bool ExtendMatchingFlightTypeCodesToGliderAndTowFlight { get; set; }

        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the start locations ICAO codes for this rule filter
        /// </summary>
        public string MatchedStartLocations { get; set; }
        
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        /// <summary>
        /// Get or sets the landing locations ICAO codes for this rule filter
        /// </summary>
        public string MatchedLdgLocations { get; set; }
        
        public bool UseRuleForAllClubMemberNumbersExceptListed { get; set; }

        //[Column("MatchedClubMemberNumbers")]
        public string MatchedClubMemberNumbers { get; set; }

        public bool UseRuleForAllFlightCrewTypesExceptListed { get; set; }

        //[Column("MatchedFlightCrewTypes")]
        public string MatchedFlightCrewTypes { get; set; }

        public bool UseRuleForAllAircraftsOnHomebaseExceptListed { get; set; }

        public string MatchedAircraftsHomebase { get; set; }

        public bool UseRuleForAllMemberStatesExceptListed { get; set; }

        public string MatchedMemberStates { get; set; }

        public bool UseRuleForAllPersonCategoriesExceptListed { get; set; }

        public string MatchedPersonCategories { get; set; }

        public bool IsChargedToClubInternal { get; set; }

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

        public int? AccountingUnitTypeId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Club Club { get; set; }

        public virtual AccountingRuleFilterType AccountingRuleFilterType { get; set; }

        public virtual AccountingUnitType AccountingUnitType { get; set; }

        public Guid Id
        {
            get { return AccountingRuleFilterId; }
            set { AccountingRuleFilterId = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
            {
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));
            }

            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo info in type.GetProperties())
            {
                Type t = info.PropertyType;
                if (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                    t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)
                    || (t.Namespace != null && t.Namespace.Contains("FLS.Server.Data.DbEntities")))
                {
                    continue;
                }

                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            }

            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}