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
                    if (entity.RecipientTarget != null)
                    {
                        var recipient = JsonConvert.DeserializeObject<RecipientDetails>(entity.RecipientTarget);
                        if (recipient != null)
                        {
                            overview.Target =
                                $"{recipient.RecipientName} ({recipient.PersonClubMemberNumber}){Environment.NewLine}{recipient.Lastname} {recipient.Firstname}{Environment.NewLine}{recipient.AddressLine1}{Environment.NewLine}{recipient.AddressLine2}{Environment.NewLine}{recipient.ZipCode} {recipient.City}";
                        }
                        else
                        {
                            overview.Target = string.Empty;
                        }
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                case (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.StartTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.EngineTimeAccountingRuleFilter:
                    if (entity.ArticleTarget != null)
                    {
                        var articleTarget = JsonConvert.DeserializeObject<ArticleTargetDetails>(entity.ArticleTarget);
                        if (articleTarget != null)
                        {
                            overview.Target = $"{articleTarget.ArticleNumber} ({articleTarget.DeliveryLineText})";
                        }
                        else
                        {
                            overview.Target = string.Empty;
                        }
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                    if (entity.ArticleTarget != null)
                    {
                        var target = JsonConvert.DeserializeObject<ArticleTargetDetails>(entity.ArticleTarget);
                        if (target != null)
                        {
                            overview.Target = $"{target.ArticleNumber} ({target.DeliveryLineText})";
                        }
                        else
                        {
                            overview.Target = string.Empty;
                        }
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                default:
                    break;
            }

            return overview;
        }

        public static AccountingRuleFilterOverview ToAccountingRuleFilterOverview(this AccountingRuleFilterDetails details,
            List<FLS.Server.Data.DbEntities.AccountingRuleFilterType> accountingFilterTypes, AccountingRuleFilterOverview overview = null)
        {
            details.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new AccountingRuleFilterOverview();
            }

            overview.AccountingRuleFilterId = details.AccountingRuleFilterId;

            var accountingFilterType =
                accountingFilterTypes.FirstOrDefault(
                    x => x.AccountingRuleFilterTypeId == details.AccountingRuleFilterTypeId);

            if (accountingFilterType != null)
            {
                overview.AccountingRuleFilterTypeName = accountingFilterType.AccountingRuleFilterTypeName;
            }

            overview.RuleFilterName = details.RuleFilterName;
            overview.Description = details.Description;
            overview.IsActive = details.IsActive;
            overview.SortIndicator = details.SortIndicator;

            switch (details.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:
                    if (details.RecipientTarget != null)
                    {
                        var recipient = details.RecipientTarget;
                        if (recipient != null)
                        {
                            overview.Target =
                                $"{recipient.RecipientName} ({recipient.PersonClubMemberNumber}){Environment.NewLine}{recipient.Lastname} {recipient.Firstname}{Environment.NewLine}{recipient.AddressLine1}{Environment.NewLine}{recipient.AddressLine2}{Environment.NewLine}{recipient.ZipCode} {recipient.City}";
                        }
                        else
                        {
                            overview.Target = string.Empty;
                        }
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                case (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.StartTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.EngineTimeAccountingRuleFilter:
                    if (details.ArticleTarget != null)
                    {
                        overview.Target =
                            $"{details.ArticleTarget.ArticleNumber} ({details.ArticleTarget.DeliveryLineText})";
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                    if (details.ArticleTarget != null)
                    {
                        overview.Target = $"{details.ArticleTarget.ArticleNumber} ({details.ArticleTarget.DeliveryLineText})";
                    }
                    else
                    {
                        overview.Target = string.Empty;
                    }
                    break;
                default:
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
            details.StopRuleEngineWhenRuleApplied = entity.StopRuleEngineWhenRuleApplied;
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
            details.UseRuleForAllStartTypesExceptListed =
                entity.UseRuleForAllStartTypesExceptListed;
            details.AccountingUnitTypeId = entity.AccountingUnitTypeId;

            details.UseRuleForAllAircraftsOnHomebaseExceptListed =
                entity.UseRuleForAllAircraftsOnHomebaseExceptListed;
            details.UseRuleForAllMemberStatesExceptListed =
                entity.UseRuleForAllMemberStatesExceptListed;
            details.UseRuleForAllPersonCategoriesExceptListed =
                entity.UseRuleForAllPersonCategoriesExceptListed;

            //Deserialize JSON lists to properties
            if (entity.MatchedAircraftImmatriculations != null) details.MatchedAircraftImmatriculations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedAircraftImmatriculations);
            if (entity.MatchedStartLocations != null) details.MatchedStartLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedStartLocations);
            if (entity.MatchedLdgLocations != null) details.MatchedLdgLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedLdgLocations);
            if (entity.MatchedFlightTypeCodes != null) details.MatchedFlightTypeCodes = JsonConvert.DeserializeObject<List<string>>(entity.MatchedFlightTypeCodes);
            if (entity.MatchedClubMemberNumbers != null) details.MatchedClubMemberNumbers = JsonConvert.DeserializeObject<List<string>>(entity.MatchedClubMemberNumbers);
            if (entity.MatchedFlightCrewTypes != null) details.MatchedFlightCrewTypes = JsonConvert.DeserializeObject<List<int>>(entity.MatchedFlightCrewTypes);
            if (entity.MatchedStartTypes != null) details.MatchedStartTypes = JsonConvert.DeserializeObject<List<int>>(entity.MatchedStartTypes);

            if (entity.MatchedAircraftsHomebase != null) details.MatchedAircraftsHomebase = JsonConvert.DeserializeObject<List<string>>(entity.MatchedAircraftsHomebase);
            if (entity.MatchedMemberStates != null) details.MatchedMemberStates = JsonConvert.DeserializeObject<List<Guid>>(entity.MatchedMemberStates);
            if (entity.MatchedPersonCategories != null) details.MatchedPersonCategories = JsonConvert.DeserializeObject<List<Guid>>(entity.MatchedPersonCategories);

            switch (entity.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:

                    if (entity.RecipientTarget != null) details.RecipientTarget = JsonConvert.DeserializeObject<RecipientDetails>(entity.RecipientTarget);
                    details.ArticleTarget = null;
                    details.IsChargedToClubInternal = entity.IsChargedToClubInternal;
                    break;
                case (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.StartTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.EngineTimeAccountingRuleFilter:

                    if (entity.ArticleTarget != null) details.ArticleTarget = JsonConvert.DeserializeObject<ArticleTargetDetails>(entity.ArticleTarget);
                    details.RecipientTarget = null;
                    details.MinFlightTimeInSecondsMatchingValue = entity.MinFlightTimeInSecondsMatchingValue;
                    details.MaxFlightTimeInSecondsMatchingValue = entity.MaxFlightTimeInSecondsMatchingValue;
                    details.MinEngineTimeInSecondsMatchingValue = entity.MinEngineTimeInSecondsMatchingValue;
                    details.MaxEngineTimeInSecondsMatchingValue = entity.MaxEngineTimeInSecondsMatchingValue;
                    details.IncludeThresholdText = entity.IncludeThresholdText;
                    details.ThresholdText = entity.ThresholdText;
                    details.IncludeFlightTypeName = entity.IncludeFlightTypeName;
                    details.NoLandingTaxForGlider = entity.NoLandingTaxForGlider;
                    details.NoLandingTaxForTowingAircraft = entity.NoLandingTaxForTowingAircraft;
                    details.NoLandingTaxForAircraft = entity.NoLandingTaxForAircraft;
                    break;
                default:
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
            entity.StopRuleEngineWhenRuleApplied = details.StopRuleEngineWhenRuleApplied;
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
            entity.UseRuleForAllStartTypesExceptListed =
               details.UseRuleForAllStartTypesExceptListed;
            entity.AccountingUnitTypeId = details.AccountingUnitTypeId;
            entity.UseRuleForAllAircraftsOnHomebaseExceptListed = details.UseRuleForAllAircraftsOnHomebaseExceptListed;
            entity.UseRuleForAllMemberStatesExceptListed = details.UseRuleForAllMemberStatesExceptListed;
            entity.UseRuleForAllPersonCategoriesExceptListed = details.UseRuleForAllPersonCategoriesExceptListed;

            //Serialize JSON lists to properties
            entity.MatchedAircraftImmatriculations = JsonConvert.SerializeObject(details.MatchedAircraftImmatriculations);
            entity.MatchedStartLocations = JsonConvert.SerializeObject(details.MatchedStartLocations);
            entity.MatchedLdgLocations = JsonConvert.SerializeObject(details.MatchedLdgLocations);
            entity.MatchedFlightTypeCodes = JsonConvert.SerializeObject(details.MatchedFlightTypeCodes);
            entity.MatchedClubMemberNumbers = JsonConvert.SerializeObject(details.MatchedClubMemberNumbers);
            entity.MatchedFlightCrewTypes = JsonConvert.SerializeObject(details.MatchedFlightCrewTypes);
            entity.MatchedStartTypes = JsonConvert.SerializeObject(details.MatchedStartTypes);
            entity.MatchedAircraftsHomebase = JsonConvert.SerializeObject(details.MatchedAircraftsHomebase);
            entity.MatchedMemberStates = JsonConvert.SerializeObject(details.MatchedMemberStates);
            entity.MatchedPersonCategories = JsonConvert.SerializeObject(details.MatchedPersonCategories);


            switch (details.AccountingRuleFilterTypeId)
            {
                case (int)AccountingRuleFilterType.RecipientAccountingRuleFilter:
                    entity.ArticleTarget = null;
                    entity.RecipientTarget = JsonConvert.SerializeObject(details.RecipientTarget);
                    entity.IsChargedToClubInternal = details.IsChargedToClubInternal;
                    break;
                case (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.StartTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter:
                case (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter:
                case (int)AccountingRuleFilterType.EngineTimeAccountingRuleFilter:
                    entity.ArticleTarget = JsonConvert.SerializeObject(details.ArticleTarget);
                    entity.RecipientTarget = null;
                    entity.MinFlightTimeInSecondsMatchingValue = details.MinFlightTimeInSecondsMatchingValue;
                    entity.MaxFlightTimeInSecondsMatchingValue = details.MaxFlightTimeInSecondsMatchingValue;
                    entity.MinEngineTimeInSecondsMatchingValue = details.MinEngineTimeInSecondsMatchingValue;
                    entity.MaxEngineTimeInSecondsMatchingValue = details.MaxEngineTimeInSecondsMatchingValue;
                    entity.IncludeThresholdText = details.IncludeThresholdText;
                    entity.ThresholdText = details.ThresholdText;
                    entity.IncludeFlightTypeName = details.IncludeFlightTypeName;
                    entity.NoLandingTaxForGlider = details.NoLandingTaxForGlider;
                    entity.NoLandingTaxForTowingAircraft = details.NoLandingTaxForTowingAircraft;
                    entity.NoLandingTaxForAircraft = details.NoLandingTaxForAircraft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("AccountingRuleFilterTypeId");
            }

            return entity;
        }
    }
}
