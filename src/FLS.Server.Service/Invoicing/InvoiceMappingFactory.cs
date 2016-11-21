using System;
using System.Collections.Generic;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Interfaces;
using NLog;

namespace FLS.Server.Service.Invoicing
{
    public class InvoiceMappingFactory
    {
        internal InvoiceLineRuleFilterContainer CreateInvoiceLineRuleFilterContainer()
        {
            var invoiceLineRuleFilterContainer = new InvoiceLineRuleFilterContainer();
            var baseTeachingFlightTypeCodes = new List<string>();
            baseTeachingFlightTypeCodes.Add("70"); //Grundschulung Doppelsteuer
            baseTeachingFlightTypeCodes.Add("80"); //Grundschulung Solo
            baseTeachingFlightTypeCodes.Add("66"); //Lufttaufe bar
            baseTeachingFlightTypeCodes.Add("68"); //Schnupperflug Gutschein
            baseTeachingFlightTypeCodes.Add("69"); //Schnupperflug bar

            var furtherTrainingFlightTypeCodes = new List<string>();
            furtherTrainingFlightTypeCodes.Add("77"); //Weiterbildung Doppelsteuer
            furtherTrainingFlightTypeCodes.Add("88"); //Weiterbildung Solo
            furtherTrainingFlightTypeCodes.Add("78"); //Jahres-Checkflug

            var noFlatRateClubMemberNumbers = new List<string>();
            noFlatRateClubMemberNumbers.Add("363289"); //Marc
            noFlatRateClubMemberNumbers.Add("897764"); //Hermann
            noFlatRateClubMemberNumbers.Add("155264"); //Heiri
            noFlatRateClubMemberNumbers.Add("463161"); //Fredi
            noFlatRateClubMemberNumbers.Add("622976"); //Rolf
            noFlatRateClubMemberNumbers.Add("686001"); //Gian

            var vsfFeeRuleFilter = new VsfFeeRuleFilter()
            {
                ArticleTarget = new ArticleTarget { ArticleNumber = "1003", InvoiceLineText = "VFS-Gebühr"},
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true
            };
            invoiceLineRuleFilterContainer.VsfFeeRuleFilters.Add(vsfFeeRuleFilter);


            invoiceLineRuleFilterContainer.InstructorToArticleMapping.Add("999999", "50"); //Silvan
            invoiceLineRuleFilterContainer.InstructorToArticleMapping.Add("424976", "29"); //Karl
            invoiceLineRuleFilterContainer.InstructorToArticleMapping.Add("536594", "19"); //H.U.K
            invoiceLineRuleFilterContainer.InstructorToArticleMapping.Add("836001", "116"); //Päde 
            invoiceLineRuleFilterContainer.InstructorToArticleMapping.Add("888888", "90"); //Thomas
            
            int sortIndicator = 1;
            var aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1059",
                    InvoiceLineText = "Schulung"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3256");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1061",
                    InvoiceLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3407");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1063",
                    InvoiceLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1841");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1065",
                    InvoiceLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1824");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1072",
                    InvoiceLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-2464");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1059",
                    InvoiceLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3256");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1061",
                    InvoiceLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3407");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1063",
                    InvoiceLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1841");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1065",
                    InvoiceLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1824");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1072",
                    InvoiceLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-2464");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);




            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1187",
                    InvoiceLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3256");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1186",
                    InvoiceLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3407");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1189",
                    InvoiceLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1841");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1188",
                    InvoiceLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1824");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1190",
                    InvoiceLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-2464");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);


            sortIndicator = 1;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1058",
                    InvoiceLineText = "Privat",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3256");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1060",
                    InvoiceLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-3407");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1062",
                    InvoiceLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1841");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1064",
                    InvoiceLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-1824");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1071",
                    InvoiceLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-2464");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            //Towing Aircrafts
            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1068",
                    InvoiceLineText = "Schulung"
                },
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1069",
                    InvoiceLineText = "Schulung"
                },
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1066",
                    InvoiceLineText = "Privat"
                },
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1067",
                    InvoiceLineText = "Privat"
                },
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1153",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-PFW");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1154",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KIO");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1155",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-PDL");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1156",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-EQC");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1157",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-WAT");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1158",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-DGP");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1159",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KDO");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1161",
                    InvoiceLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-DCU");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1162",
                    InvoiceLineText = "Saanen"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> { "LSGK" },
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.AircraftImmatriculations.Add("HB-KCB");
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);



            var additionalFuelFeeRule = new AdditionalFuelFeeRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = false,
                AircraftImmatriculations = new List<string> { "HB-KCB" },
                SortIndicator = 1,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1086",
                    InvoiceLineText = "Treibstoffzuschlag"
                },
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForMotorFlights = true,
                IsRuleForTowingFlights = true
            };
            invoiceLineRuleFilterContainer.AdditionalFuelFeeRuleFilters.Add(additionalFuelFeeRule);

            var noLandingTax = new NoLandingTaxRuleFilter()
            {
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForSelfstartedGliderFlights = false,
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                NoLandingTaxForGlider = true,
                NoLandingTaxForTowingAircraft = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            noLandingTax.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.NoLandingTaxRuleFilters.Add(noLandingTax);

            noLandingTax = new NoLandingTaxRuleFilter()
            {
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForSelfstartedGliderFlights = false,
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                NoLandingTaxForGlider = true,
                NoLandingTaxForTowingAircraft = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            noLandingTax.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.NoLandingTaxRuleFilters.Add(noLandingTax);

            var landingTaxRule = new LandingTaxRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "",
                    InvoiceLineText = "Keine Landetaxen Speck für Schulung"
                },
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            landingTaxRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.LandingTaxRuleFilters.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1037",
                    InvoiceLineText = "Landetaxen Speck"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK"},
                IsRuleForSelfstartedGliderFlights = true,   //TODO: create start tax for self starting gliders
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            landingTaxRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.LandingTaxRuleFilters.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1106",
                    InvoiceLineText = "Landetaxen Montricher"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSTR" },
                IsRuleForSelfstartedGliderFlights = false,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceLineRuleFilterContainer.LandingTaxRuleFilters.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTarget
                {
                    ArticleNumber = "1160",
                    InvoiceLineText = "Landetaxen Saanen"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSGK" },
                IsRuleForSelfstartedGliderFlights = false,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                IsActive = true,
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceLineRuleFilterContainer.LandingTaxRuleFilters.Add(landingTaxRule);

            return invoiceLineRuleFilterContainer;
        }

        internal Dictionary<string, InvoiceRecipientTarget> CreateFlightTypeToInvoiceRecipientMapping()
        {
            Dictionary<string, InvoiceRecipientTarget> flightTypeToInvoiceRecipientMapping =
                new Dictionary<string, InvoiceRecipientTarget>();


            var invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Schnupperflug Gutschein";
            invoiceRecipientTarget.MemberNumber = "999006";
            flightTypeToInvoiceRecipientMapping.Add("68", invoiceRecipientTarget); //Schnupperflug Gutschein

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Schnupperflug und Lufttaufe bar";
            invoiceRecipientTarget.MemberNumber = "999000";
            flightTypeToInvoiceRecipientMapping.Add("69", invoiceRecipientTarget); //Schnupperflug bar
            flightTypeToInvoiceRecipientMapping.Add("66", invoiceRecipientTarget); //Lufttaufe bar

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Passagierflug bar";
            invoiceRecipientTarget.MemberNumber = "999001";
            flightTypeToInvoiceRecipientMapping.Add("63", invoiceRecipientTarget); //PAX bar

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Passagierflug Gutschein";
            invoiceRecipientTarget.MemberNumber = "999007";
            flightTypeToInvoiceRecipientMapping.Add("62", invoiceRecipientTarget); //PAX Gutschein

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Marketingflug";
            invoiceRecipientTarget.MemberNumber = "999004";
            flightTypeToInvoiceRecipientMapping.Add("100", invoiceRecipientTarget);
            
            return flightTypeToInvoiceRecipientMapping;
        }
    }
}