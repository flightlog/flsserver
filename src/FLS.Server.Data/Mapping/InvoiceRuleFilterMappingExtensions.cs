using System;
using System.Collections.Generic;
using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Data.WebApi.Location;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;

namespace FLS.Server.Data.Mapping
{
    public static class InvoiceRuleFilterMappingExtensions
    {
        public static InvoiceRuleFilterDetails ToInvoiceLineRuleFilterDetails(this InvoiceRuleFilter entity, 
            List<AircraftListItem> aircraftListItems, List<LocationListItem> locationListItems)
        {
            entity.ArgumentNotNull("entity");

            var invoiceRuleFilterDetails = new InvoiceRuleFilterDetails();
            invoiceRuleFilterDetails.RuleFilterName = entity.RuleFilterName;
            invoiceRuleFilterDetails.Description = entity.Description;
            invoiceRuleFilterDetails.ExtendMatchingFlightTypeCodesToGliderAndTowFlight =
                entity.ExtendMatchingFlightTypeCodesToGliderAndTowFlight;
            invoiceRuleFilterDetails.IsActive = entity.IsActive;
            invoiceRuleFilterDetails.IsRuleForGliderFlights = entity.IsRuleForGliderFlights;
            invoiceRuleFilterDetails.IsRuleForMotorFlights = entity.IsRuleForMotorFlights;
            invoiceRuleFilterDetails.IsRuleForSelfstartedGliderFlights = entity.IsRuleForSelfstartedGliderFlights;
            invoiceRuleFilterDetails.IsRuleForTowingFlights = entity.IsRuleForTowingFlights;
            invoiceRuleFilterDetails.SortIndicator = entity.SortIndicator;
            invoiceRuleFilterDetails.UseRuleForAllAircraftsExceptListed = entity.UseRuleForAllAircraftsExceptListed;
            invoiceRuleFilterDetails.UseRuleForAllClubMemberNumbersExceptListed =
                entity.UseRuleForAllClubMemberNumbersExceptListed;
            invoiceRuleFilterDetails.UseRuleForAllFlightCrewTypesExceptListed =
                entity.UseRuleForAllFlightCrewTypesExceptListed;
            invoiceRuleFilterDetails.UseRuleForAllFlightTypesExceptListed = entity.UseRuleForAllFlightTypesExceptListed;
            invoiceRuleFilterDetails.UseRuleForAllLdgLocationsExceptListed = entity.UseRuleForAllLdgLocationsExceptListed;
            invoiceRuleFilterDetails.UseRuleForAllStartLocationsExceptListed =
                entity.UseRuleForAllStartLocationsExceptListed;

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
            
            return invoiceRuleFilterDetails;
        }
    }
}
