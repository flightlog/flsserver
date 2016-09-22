using System;
using System.Collections.Generic;
using FLS.Server.Interfaces;
using FLS.Server.ProffixInvoiceService.RuleFilters;
using NLog;

namespace FLS.Server.ProffixInvoiceService
{
    public class InvoiceMappingFactory
    {
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public InvoiceMappingFactory(IAircraftService aircraftService, ILocationService locationService)
        {
            _aircraftService = aircraftService;
            _locationService = locationService;
        }

        internal InvoiceLineRuleFilterContainer CreateInvoiceLineRuleFilterContainer()
        {
            var invoiceLineRuleFilterContainer = new InvoiceLineRuleFilterContainer();
            var lszkId = GetLocationId("LSZK");
            var saanenId = GetLocationId("LSGK");
            var montricherId = GetLocationId("LSTR");
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
                AddVsfFeePerLanding = true,
                ProffixArticleNumber = "1003",
                InvoiceLineText = "VFS-Gebühr",
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
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


            invoiceLineRuleFilterContainer.InstructorToProffixArticleMapping.Add("999999", "50"); //Silvan
            invoiceLineRuleFilterContainer.InstructorToProffixArticleMapping.Add("424976", "29"); //Karl
            invoiceLineRuleFilterContainer.InstructorToProffixArticleMapping.Add("536594", "19"); //H.U.K
            invoiceLineRuleFilterContainer.InstructorToProffixArticleMapping.Add("836001", "116"); //Päde 
            invoiceLineRuleFilterContainer.InstructorToProffixArticleMapping.Add("888888", "90"); //Thomas
            
            var aircraftId = GetAircraftId("HB-3256");
            int sortIndicator = 1;
            var aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1059",
                InvoiceLineText = "Schulung",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1061",
                InvoiceLineText = "Schulung",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1063",
                InvoiceLineText = "Schulung",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1065",
                InvoiceLineText = "Schulung",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1072",
                InvoiceLineText = "Schulung",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1059",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1061",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1063",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1065",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1072",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);




            sortIndicator++;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1187",
                InvoiceLineText = "Weiterbildung mit Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1186",
                InvoiceLineText = "Weiterbildung mit Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1189",
                InvoiceLineText = "Weiterbildung mit Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1188",
                InvoiceLineText = "Weiterbildung mit Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1190",
                InvoiceLineText = "Weiterbildung mit Pauschale",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);


            sortIndicator = 1;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1058",
                InvoiceLineText = "Privat",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1060",
                InvoiceLineText = "Privat",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1062",
                InvoiceLineText = "Privat",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1064",
                InvoiceLineText = "Privat",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1071",
                InvoiceLineText = "Privat",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            //Towing Aircrafts
            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1068",
                InvoiceLineText = "Schulung",
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1069",
                InvoiceLineText = "Schulung",
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1066",
                InvoiceLineText = "Privat",
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1067",
                InvoiceLineText = "Privat",
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PFW");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1153",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KIO");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1154",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PDL");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1155",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-EQC");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1156",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-WAT");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1157",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DGP");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1158",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KDO");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1159",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DCU");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1161",
                InvoiceLineText = "",
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
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftRuleFilter
            {
                SortIndicator = sortIndicator,
                ProffixArticleNumber = "1162",
                InvoiceLineText = "Saanen",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForTowingFlights = true
            };
            aircraftMappingRule.Aircrafts.Add(aircraftId);
            aircraftMappingRule.MatchedStartLocations.Add(saanenId);
            invoiceLineRuleFilterContainer.AircraftRuleFilters.Add(aircraftMappingRule);



            var additionalFuelFeeRule = new AdditionalFuelFeeRuleFilter
            {
                UseRuleForAllAircraftsExceptListed = false,
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                ProffixArticleNumber = "1086",
                InvoiceLineText = "Treibstoffzuschlag",
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                IsActive = true,
                IsRuleForGliderFlights = true,
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForMotorFlights = true,
                IsRuleForTowingFlights = true
            };
            additionalFuelFeeRule.Aircrafts.Add(aircraftId);
            additionalFuelFeeRule.MatchedStartLocations.Add(lszkId);
            invoiceLineRuleFilterContainer.AdditionalFuelFeeRuleFilters.Add(additionalFuelFeeRule);

            var noLandingTax = new NoLandingTaxRuleFilter()
            {
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForSelfstartedGliderFlights = false,
                UseRuleForAllAircraftsExceptListed = true,
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
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
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
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
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                ProffixArticleNumber = "",
                InvoiceLineText = "Keine Landetaxen Speck für Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
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
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                ProffixArticleNumber = "1037",
                InvoiceLineText = "Landetaxen Speck",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
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
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                ProffixArticleNumber = "1106",
                InvoiceLineText = "Landetaxen Montricher",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { montricherId },
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
                Aircrafts = new List<Guid>(),
                SortIndicator = 1,
                ProffixArticleNumber = "1160",
                InvoiceLineText = "Landetaxen Saanen",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { saanenId },
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

        internal Guid GetAircraftId(string immatriculation)
        {
            return _aircraftService.GetAircraftDetails(immatriculation).AircraftId;
        }

        internal Guid GetLocationId(string icaoCode)
        {
            return _locationService.GetLocationDetailsByIcaoCode(icaoCode).LocationId;
        }
    }
}