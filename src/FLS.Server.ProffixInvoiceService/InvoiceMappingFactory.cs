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


        internal InvoiceMapping CreateInvoiceMapping()
        {
            var invoiceMapping = new InvoiceMapping();
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

            //invoiceMapping.IsErrorWhenNoAdditionalFuelFeeRuleMatches = false;
            //invoiceMapping.IsErrorWhenNoLandingTaxRuleMatches = false;
            //invoiceMapping.IsErrorWhenNoVFSFeeRuleMatches = false;
            invoiceMapping.VsfFee = new VsfFee()
            {
                AddVsfFeePerLanding = true,
                ERPArticleNumber = "1003",
                InvoiceLineText = "VFS-Gebühr",
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId }
            };


            invoiceMapping.InstructorToERPArticleMapping.Add("999999", "50"); //Silvan
            invoiceMapping.InstructorToERPArticleMapping.Add("424976", "29"); //Karl
            invoiceMapping.InstructorToERPArticleMapping.Add("536594", "19"); //H.U.K
            invoiceMapping.InstructorToERPArticleMapping.Add("836001", "116"); //Päde 
            invoiceMapping.InstructorToERPArticleMapping.Add("888888", "90"); //Thomas

            var invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Schnupperflug Gutschein";
            invoiceRecipientTarget.MemberKey = "999006";
            invoiceRecipientTarget.MemberNumber = "999006";
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("68", invoiceRecipientTarget); //Schnupperflug Gutschein

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Schnupperflug und Lufttaufe bar";
            invoiceRecipientTarget.MemberKey = "999000";
            invoiceRecipientTarget.MemberNumber = "999000";
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("69", invoiceRecipientTarget); //Schnupperflug bar
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("66", invoiceRecipientTarget); //Lufttaufe bar

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Passagierflug bar";
            invoiceRecipientTarget.MemberKey = "999001";
            invoiceRecipientTarget.MemberNumber = "999001";
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("63", invoiceRecipientTarget); //PAX bar

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Passagierflug Gutschein";
            invoiceRecipientTarget.MemberKey = "999007";
            invoiceRecipientTarget.MemberNumber = "999007";
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("62", invoiceRecipientTarget); //PAX Gutschein

            invoiceRecipientTarget = new InvoiceRecipientTarget();
            invoiceRecipientTarget.DisplayName = "FGZO Marketingflug";
            invoiceRecipientTarget.MemberKey = "999004";
            invoiceRecipientTarget.MemberNumber = "999004";
            invoiceMapping.FlightTypeToInvoiceRecipientMapping.Add("100", invoiceRecipientTarget);
            
            var aircraftId = GetAircraftId("HB-3256");
            int sortIndicator = 1;
            var aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1059",
                InvoiceLineText = "Schulung",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1061",
                InvoiceLineText = "Schulung",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1063",
                InvoiceLineText = "Schulung",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1065",
                InvoiceLineText = "Schulung",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1072",
                InvoiceLineText = "Schulung",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1059",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1061",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1063",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1065",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1072",
                InvoiceLineText = "Weiterbildung ohne Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);




            sortIndicator++;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1187",
                InvoiceLineText = "Weiterbildung mit Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1186",
                InvoiceLineText = "Weiterbildung mit Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1189",
                InvoiceLineText = "Weiterbildung mit Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1188",
                InvoiceLineText = "Weiterbildung mit Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1190",
                InvoiceLineText = "Weiterbildung mit Pauschale",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers)
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);


            sortIndicator = 1;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1058",
                InvoiceLineText = "Privat",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1060",
                InvoiceLineText = "Privat",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1062",
                InvoiceLineText = "Privat",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1064",
                InvoiceLineText = "Privat",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1071",
                InvoiceLineText = "Privat",
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            //Towing Aircrafts
            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1068",
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
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1069",
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
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1066",
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
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1067",
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
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PFW");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1153",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KIO");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1154",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PDL");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1155",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-EQC");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1156",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-WAT");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1157",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DGP");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1158",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KDO");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1159",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DCU");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1161",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftMapping
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1162",
                InvoiceLineText = "Saanen",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>(),
                UseRuleForAllClubMemberNumbersExceptListed = true
            };
            aircraftMappingRule.MatchedStartLocations.Add(saanenId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            

            var additionalFuelFeeRule = new AdditionalFuelFee
            {
                UseRuleForAllAircraftsExceptListed = false,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1086",
                InvoiceLineText = "Treibstoffzuschlag",
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            additionalFuelFeeRule.AircraftIds.Add(aircraftId);
            additionalFuelFeeRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AdditionalFuelFeeRules.Add(additionalFuelFeeRule);

            var noLandingTax = new NoLandingTax()
            {
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForSelfstartedGliderFlights = false,
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                NoLandingTaxForGlider = true,
                NoLandingTaxForTowingAircraft = true,
            };
            noLandingTax.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.NoLandingTaxRules.Add(noLandingTax);

            noLandingTax = new NoLandingTax()
            {
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForSelfstartedGliderFlights = false,
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                NoLandingTaxForGlider = true,
                NoLandingTaxForTowingAircraft = true,
            };
            noLandingTax.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.NoLandingTaxRules.Add(noLandingTax);

            var landingTaxRule = new LandingTax
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "",
                InvoiceLineText = "Keine Landetaxen Speck für Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                IsRuleForSelfstartedGliderFlights = true,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true
            };
            landingTaxRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTax
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1037",
                InvoiceLineText = "Landetaxen Speck",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                IsRuleForSelfstartedGliderFlights = true,   //TODO: create start tax for self starting gliders
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true
            };
            landingTaxRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTax
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1106",
                InvoiceLineText = "Landetaxen Montricher",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { montricherId },
                IsRuleForSelfstartedGliderFlights = false,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTax
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1160",
                InvoiceLineText = "Landetaxen Saanen",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { saanenId },
                IsRuleForSelfstartedGliderFlights = false,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);
            
            return invoiceMapping;
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