using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Converters;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Location;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using Newtonsoft.Json;
using NLog;
using AccountingRuleFilterType = FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType;

namespace FLS.Server.Data.Mapping
{
    public static class AccountingRuleFilterMappingExtensions
    {
        public static Logger Logger = LogManager.GetLogger("FLS.Server.Data.Mapping.AccountingRuleFilterMappingExtensions");

        public static AccountingRuleFilterOverview ToAccountingRuleFilterOverview(this AccountingRuleFilter entity,
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, AccountingRuleFilterOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new AccountingRuleFilterOverview();
            }

            overview.AccountingRuleFilterId = entity.AccountingRuleFilterId;

            if (entity.AccountingRuleFilterType != null)
            {
                overview.AccountingRuleFilterTypeName = entity.AccountingRuleFilterType.AccountingRuleFilterTypeName;
            }

            overview.RuleFilterName = entity.RuleFilterName;
            overview.Description = entity.Description;
            overview.IsActive = entity.IsActive;
            overview.SortIndicator = entity.SortIndicator;

            switch (entity.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:
                    var recipient = JsonConvert.DeserializeObject<RecipientDetails>(entity.RecipientTarget);
                    overview.Target = recipient.ToString();
                    break;
                case (int)AccountingRuleFilterType.AircraftAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                     var articleTarget = JsonConvert.DeserializeObject<ArticleTargetDetails>(entity.ArticleTarget);
                    overview.Target = $"{articleTarget.ArticleNumber} ({articleTarget.DeliveryLineText})";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("AccountingRuleFilterTypeId");
                    break;
            }

            return overview;
        }

        public static AccountingRuleFilterDetails ToAccountingRuleFilterDetails(this AccountingRuleFilter entity, 
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, AccountingRuleFilterDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new AccountingRuleFilterDetails();
            }

            details.AccountingRuleFilterId = entity.AccountingRuleFilterId;
            details.AccountingRuleFilterTypeId = entity.AccountingRuleFilterTypeId;
            details.RuleFilterName = entity.RuleFilterName;
            details.Description = entity.Description;
            details.ExtendMatchingFlightTypeCodesToGliderAndTowFlight =
                entity.ExtendMatchingFlightTypeCodesToGliderAndTowFlight;
            details.IsActive = entity.IsActive;
            details.IsRuleForGliderFlights = entity.IsRuleForGliderFlights;
            details.IsRuleForMotorFlights = entity.IsRuleForMotorFlights;
            details.IsRuleForSelfstartedGliderFlights = entity.IsRuleForSelfstartedGliderFlights;
            details.IsRuleForTowingFlights = entity.IsRuleForTowingFlights;
            details.SortIndicator = entity.SortIndicator;
            details.UseRuleForAllAircraftsExceptListed = entity.UseRuleForAllAircraftsExceptListed;
            details.UseRuleForAllClubMemberNumbersExceptListed =
                entity.UseRuleForAllClubMemberNumbersExceptListed;
            details.UseRuleForAllFlightCrewTypesExceptListed =
                entity.UseRuleForAllFlightCrewTypesExceptListed;
            details.UseRuleForAllFlightTypesExceptListed = entity.UseRuleForAllFlightTypesExceptListed;
            details.UseRuleForAllLdgLocationsExceptListed = entity.UseRuleForAllLdgLocationsExceptListed;
            details.UseRuleForAllStartLocationsExceptListed =
                entity.UseRuleForAllStartLocationsExceptListed;

            //Deserialize JSON lists to properties
            details.MatchedAircraftImmatriculations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedAircraftImmatriculations);
            details.MatchedStartLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedStartLocations);
            details.MatchedLdgLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedLdgLocations);
            details.MatchedFlightTypeCodes = JsonConvert.DeserializeObject<List<string>>(entity.MatchedFlightTypeCodesAsJson);
            details.MatchedClubMemberNumbers = JsonConvert.DeserializeObject<List<string>>(entity.MatchedClubMemberNumbersAsJson);
            details.MatchedFlightCrewTypes = JsonConvert.DeserializeObject<List<int>>(entity.MatchedFlightCrewTypesAsJson);
            
            switch (entity.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:

                    details.RecipientTarget = JsonConvert.DeserializeObject<RecipientDetails>(entity.RecipientTarget);
                    details.ArticleTarget = null;
                    details.IsChargedToClubInternal = entity.IsChargedToClubInternal;
                    break;
                case (int)AccountingRuleFilterType.AircraftAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:

                    details.ArticleTarget = JsonConvert.DeserializeObject<ArticleTargetDetails>(entity.ArticleTarget);
                    details.RecipientTarget = null;
                    details.MinFlightTimeMatchingValue = entity.MinFlightTimeMatchingValue;
                    details.MaxFlightTimeMatchingValue = entity.MaxFlightTimeMatchingValue;
                    details.IncludeThresholdText = entity.IncludeThresholdText;
                    details.ThresholdText = entity.ThresholdText;
                    details.IncludeFlightTypeName = entity.IncludeFlightTypeName;
                    details.NoLandingTaxForGlider = entity.NoLandingTaxForGlider;
                    details.NoLandingTaxForTowingAircraft = entity.NoLandingTaxForTowingAircraft;
                    details.NoLandingTaxForAircraft = entity.NoLandingTaxForAircraft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("AccountingRuleFilterTypeId");
                    break;
            }
            
            return details;
        }

        public static AccountingRuleFilter ToAccountingRuleFilter(this AccountingRuleFilterDetails details, Guid clubId,
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, AccountingRuleFilter entity = null, 
            bool overwriteAccountingRuleFilterId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new AccountingRuleFilter();
            }

            if (overwriteAccountingRuleFilterId) entity.AccountingRuleFilterId = details.AccountingRuleFilterId;
            entity.ClubId = clubId;
            entity.AccountingRuleFilterTypeId = details.AccountingRuleFilterTypeId;
            entity.RuleFilterName = details.RuleFilterName;
            entity.Description = details.Description;
            entity.ExtendMatchingFlightTypeCodesToGliderAndTowFlight =
                details.ExtendMatchingFlightTypeCodesToGliderAndTowFlight;
            entity.IsActive = details.IsActive;
            entity.IsRuleForGliderFlights = details.IsRuleForGliderFlights;
            entity.IsRuleForMotorFlights = details.IsRuleForMotorFlights;
            entity.IsRuleForSelfstartedGliderFlights = details.IsRuleForSelfstartedGliderFlights;
            entity.IsRuleForTowingFlights = details.IsRuleForTowingFlights;
            entity.SortIndicator = details.SortIndicator;
            entity.UseRuleForAllAircraftsExceptListed = details.UseRuleForAllAircraftsExceptListed;
            entity.UseRuleForAllClubMemberNumbersExceptListed =
                details.UseRuleForAllClubMemberNumbersExceptListed;
            entity.UseRuleForAllFlightCrewTypesExceptListed =
                details.UseRuleForAllFlightCrewTypesExceptListed;
            entity.UseRuleForAllFlightTypesExceptListed = details.UseRuleForAllFlightTypesExceptListed;
            entity.UseRuleForAllLdgLocationsExceptListed = details.UseRuleForAllLdgLocationsExceptListed;
            entity.UseRuleForAllStartLocationsExceptListed =
                details.UseRuleForAllStartLocationsExceptListed;

            //Serialize JSON lists to properties
            entity.MatchedAircraftImmatriculations = JsonConvert.SerializeObject(details.MatchedAircraftImmatriculations);
            entity.MatchedStartLocations = JsonConvert.SerializeObject(details.MatchedStartLocations);
            entity.MatchedLdgLocations = JsonConvert.SerializeObject(details.MatchedLdgLocations);
            entity.MatchedFlightTypeCodesAsJson = JsonConvert.SerializeObject(details.MatchedFlightTypeCodes);
            entity.MatchedClubMemberNumbersAsJson = JsonConvert.SerializeObject(details.MatchedClubMemberNumbers);
            entity.MatchedFlightCrewTypesAsJson = JsonConvert.SerializeObject(details.MatchedFlightCrewTypes);

            switch (details.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:
                    entity.ArticleTarget = null;
                    entity.RecipientTarget = JsonConvert.SerializeObject(details.RecipientTarget);
                    entity.IsChargedToClubInternal = details.IsChargedToClubInternal;
                    break;
                case (int)AccountingRuleFilterType.AircraftAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                    entity.ArticleTarget = JsonConvert.SerializeObject(details.ArticleTarget);
                    entity.RecipientTarget = null;
                    entity.MinFlightTimeMatchingValue = details.MinFlightTimeMatchingValue;
                    entity.MaxFlightTimeMatchingValue = details.MaxFlightTimeMatchingValue;
                    entity.IncludeThresholdText = details.IncludeThresholdText;
                    entity.ThresholdText = details.ThresholdText;
                    entity.IncludeFlightTypeName = details.IncludeFlightTypeName;
                    entity.NoLandingTaxForGlider = details.NoLandingTaxForGlider;
                    entity.NoLandingTaxForTowingAircraft = details.NoLandingTaxForTowingAircraft;
                    entity.NoLandingTaxForAircraft = details.NoLandingTaxForAircraft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("AccountingRuleFilterTypeId");
                    break;
            }

            return entity;
        }
    }
}
