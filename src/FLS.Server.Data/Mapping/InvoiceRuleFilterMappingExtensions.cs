using System;
using System.Collections.Generic;
using FLS.Common.Converters;
using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Data.WebApi.Location;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using Newtonsoft.Json;

namespace FLS.Server.Data.Mapping
{
    public static class InvoiceRuleFilterMappingExtensions
    {
        public static InvoiceRuleFilterOverview ToInvoiceRuleFilterOverview(this InvoiceRuleFilter entity,
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, InvoiceRuleFilterOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new InvoiceRuleFilterOverview();
            }

            overview.InvoiceRuleFilterId = entity.InvoiceRuleFilterId;
            overview.InvoiceRuleFilterTypeId = entity.InvoiceRuleFilterTypeId;
            overview.RuleFilterName = entity.RuleFilterName;
            overview.Description = entity.Description;
            overview.IsActive = entity.IsActive;
            overview.SortIndicator = entity.SortIndicator;
            //TODO: Serialize JSON lists to properties

            switch (entity.InvoiceRuleFilterTypeId)
            {
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.RecipientInvoiceRuleFilter:

                    var recipientDetails = new RecipientDetails();
                    //TODO: Serialize recipient target property
                    break;
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AircraftInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AdditionalFuelFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.InstructorFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.LandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.NoLandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.VsfFeeInvoiceRuleFilter:
                    //TODO: Serialize article target property
                    break;
                default:
                    throw new ArgumentOutOfRangeException("InvoiceRuleFilterTypeId");
                    break;
            }

            return overview;
        }

        public static InvoiceRuleFilterDetails ToInvoiceRuleFilterDetails(this InvoiceRuleFilter entity, 
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, InvoiceRuleFilterDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new InvoiceRuleFilterDetails();
            }

            details.InvoiceRuleFilterId = entity.InvoiceRuleFilterId;
            details.InvoiceRuleFilterTypeId = entity.InvoiceRuleFilterTypeId;
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
            details.AircraftImmatriculations = JsonConvert.DeserializeObject<List<string>>(entity.AircraftImmatriculations);
            details.MatchedStartLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedStartLocations);
            details.MatchedLdgLocations = JsonConvert.DeserializeObject<List<string>>(entity.MatchedLdgLocations);
            details.MatchedFlightTypeCodes = JsonConvert.DeserializeObject<List<string>>(entity.MatchedFlightTypeCodes);
            details.MatchedClubMemberNumbers = JsonConvert.DeserializeObject<List<string>>(entity.MatchedClubMemberNumbers);
            details.MatchedFlightCrewTypes = JsonConvert.DeserializeObject<List<int>>(entity.MatchedFlightCrewTypes);

            switch (entity.InvoiceRuleFilterTypeId)
            {
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.RecipientInvoiceRuleFilter:

                    details.RecipientTarget = JsonConvert.DeserializeObject<RecipientDetails>(entity.RecipientTarget);
                    details.ArticleTarget = null;
                    details.IsInvoicedToClubInternal = entity.IsInvoicedToClubInternal;
                    break;
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AircraftInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AdditionalFuelFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.InstructorFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.LandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.NoLandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.VsfFeeInvoiceRuleFilter:

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
                    throw new ArgumentOutOfRangeException("InvoiceRuleFilterTypeId");
                    break;
            }
            
            return details;
        }

        public static InvoiceRuleFilter ToInvoiceRuleFilter(this InvoiceRuleFilterDetails details, Guid clubId,
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems, InvoiceRuleFilter entity = null, 
            bool overwriteInvoiceRuleFilterId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new InvoiceRuleFilter();
            }

            if (overwriteInvoiceRuleFilterId) entity.InvoiceRuleFilterId = details.InvoiceRuleFilterId;
            entity.ClubId = clubId;
            entity.InvoiceRuleFilterTypeId = details.InvoiceRuleFilterTypeId;
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
            entity.AircraftImmatriculations = JsonConvert.SerializeObject(details.AircraftImmatriculations);
            entity.MatchedStartLocations = JsonConvert.SerializeObject(details.MatchedStartLocations);
            entity.MatchedLdgLocations = JsonConvert.SerializeObject(details.MatchedLdgLocations);
            entity.MatchedFlightTypeCodes = JsonConvert.SerializeObject(details.MatchedFlightTypeCodes);
            entity.MatchedClubMemberNumbers = JsonConvert.SerializeObject(details.MatchedClubMemberNumbers);
            entity.MatchedFlightCrewTypes = JsonConvert.SerializeObject(details.MatchedFlightCrewTypes);

            switch (details.InvoiceRuleFilterTypeId)
            {
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.RecipientInvoiceRuleFilter:
                    entity.ArticleTarget = null;
                    entity.RecipientTarget = JsonConvert.SerializeObject(details.RecipientTarget);
                    entity.IsInvoicedToClubInternal = details.IsInvoicedToClubInternal;
                    break;
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AircraftInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AdditionalFuelFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.InstructorFeeInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.LandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.NoLandingTaxInvoiceRuleFilter:
                case (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.VsfFeeInvoiceRuleFilter:
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
                    throw new ArgumentOutOfRangeException("InvoiceRuleFilterTypeId");
                    break;
            }

            return entity;
        }
    }
}
