using System.Collections.Generic;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Flight;

namespace FLS.Server.Service.Accounting
{
    public class AccountingRuleFilterFactory
    {
        internal List<AccountingRuleFilterDetails> CreateAccountingRuleFilterDetails()
        {
            var accountingRuleFilters = new List<AccountingRuleFilterDetails>();

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

            var vsfFeeRuleFilter = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter,
                ArticleTarget = new ArticleTargetDetails { ArticleNumber = "1003", DeliveryLineText = "VSF-Gebühr"},
                RuleFilterName = "VSF-Gebühr",
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 30,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true
            };
            accountingRuleFilters.Add(vsfFeeRuleFilter);

            var instructorRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>() {"999999"},
                Description = "Silvan",
                ArticleTarget = new ArticleTargetDetails() { ArticleNumber = "50" },
                UseRuleForAllFlightCrewTypesExceptListed = false,
                MatchedFlightCrewTypes = new List<int>() {(int) FlightCrewType.FlightInstructor},
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                RuleFilterName = "Instruktor-Honorar Silvan",
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = false,
                IsRuleForMotorFlights = false
            };
            accountingRuleFilters.Add(instructorRule);

            instructorRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>() { "424976" },
                Description = "Karl",
                ArticleTarget = new ArticleTargetDetails() { ArticleNumber = "29" },
                UseRuleForAllFlightCrewTypesExceptListed = false,
                MatchedFlightCrewTypes = new List<int>() { (int)FlightCrewType.FlightInstructor },
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                RuleFilterName = "Instruktor-Honorar Karl",
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = false,
                IsRuleForMotorFlights = false
            };
            accountingRuleFilters.Add(instructorRule);

            instructorRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>() { "536594" },
                Description = "HUK",
                ArticleTarget = new ArticleTargetDetails() { ArticleNumber = "19" },
                UseRuleForAllFlightCrewTypesExceptListed = false,
                MatchedFlightCrewTypes = new List<int>() { (int)FlightCrewType.FlightInstructor },
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                RuleFilterName = "Instruktor-Honorar HUK",
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = false,
                IsRuleForMotorFlights = false
            };
            accountingRuleFilters.Add(instructorRule);

            instructorRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>() { "836001" },
                Description = "Päde",
                ArticleTarget = new ArticleTargetDetails() { ArticleNumber = "116" },
                UseRuleForAllFlightCrewTypesExceptListed = false,
                MatchedFlightCrewTypes = new List<int>() { (int)FlightCrewType.FlightInstructor },
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                RuleFilterName = "Instruktor-Honorar Päde",
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = false,
                IsRuleForMotorFlights = false
            };
            accountingRuleFilters.Add(instructorRule);

            instructorRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>() { "888888" },
                Description = "Thomas",
                ArticleTarget = new ArticleTargetDetails() { ArticleNumber = "90" },
                UseRuleForAllFlightCrewTypesExceptListed = false,
                MatchedFlightCrewTypes = new List<int>() { (int)FlightCrewType.FlightInstructor },
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllAircraftsExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllStartTypesExceptListed = true,
                RuleFilterName = "Instruktor-Honorar Thomas",
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = false,
                IsRuleForMotorFlights = false
            };
            accountingRuleFilters.Add(instructorRule);

            int sortIndicator = 1;
            var aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1059",
                    DeliveryLineText = "Schulung"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3256 Schulung"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3256");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1061",
                    DeliveryLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3407 Schulung"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3407");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1063",
                    DeliveryLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1841 Schulung"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1841");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1065",
                    DeliveryLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1824 Schulung"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1824");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1072",
                    DeliveryLineText = "Schulung",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-2464 Schulung"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-2464");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1059",
                    DeliveryLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3256 Weiterbildung ohne Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3256");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1061",
                    DeliveryLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3407 Weiterbildung ohne Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3407");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1063",
                    DeliveryLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1841 Weiterbildung ohne Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1841");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1065",
                    DeliveryLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1824 Weiterbildung ohne Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1824");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1072",
                    DeliveryLineText = "Weiterbildung ohne Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = false,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-2464 Weiterbildung ohne Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-2464");
            accountingRuleFilters.Add(aircraftMappingRule);




            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1187",
                    DeliveryLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3256 Weiterbildung mit Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3256");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1186",
                    DeliveryLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3407 Weiterbildung mit Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3407");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1189",
                    DeliveryLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1841 Weiterbildung mit Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1841");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1188",
                    DeliveryLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1824 Weiterbildung mit Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1824");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1190",
                    DeliveryLineText = "Weiterbildung mit Pauschale",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(furtherTrainingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                MatchedClubMemberNumbers = new List<string>(noFlatRateClubMemberNumbers),
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-2464 Weiterbildung mit Pauschale"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-2464");
            accountingRuleFilters.Add(aircraftMappingRule);


            sortIndicator = 1;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1058",
                    DeliveryLineText = "Privat",
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3256 Privat"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3256");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1060",
                    DeliveryLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-3407 Privat"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-3407");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1062",
                    DeliveryLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1841 Privat"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1841");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1064",
                    DeliveryLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-1824 Privat"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-1824");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1071",
                    DeliveryLineText = "Privat"
                },
                IncludeFlightTypeName = true,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllStartLocationsExceptListed = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-2464 Privat"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-2464");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            //Towing Aircrafts
            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1068",
                    DeliveryLineText = "Schulung"
                },
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = 600,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Schulung bis 10min"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1069",
                    DeliveryLineText = "Schulung"
                },
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeInSecondsMatchingValue = 600,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Schulung ab 11.min"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1066",
                    DeliveryLineText = "Privat"
                },
                IncludeThresholdText = true,
                ThresholdText = "1. bis 10. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = 600,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Privat bis 10min"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1067",
                    DeliveryLineText = "Privat"
                },
                IncludeThresholdText = true,
                ThresholdText = "ab 11. Min.",
                IncludeFlightTypeName = false,
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                MinFlightTimeInSecondsMatchingValue = 600,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Privat ab 11.min"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KCB");
            aircraftMappingRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1153",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-PFW"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-PFW");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1154",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KIO"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KIO");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1155",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-PDL"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-PDL");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1156",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-EQC"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-EQC");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1157",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-WAT"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-WAT");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1158",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-DGP"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-DGP");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1159",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KDO"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KDO");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1161",
                    DeliveryLineText = ""
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-DCU"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-DCU");
            accountingRuleFilters.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.FlightTimeAccountingRuleFilter,
                SortIndicator = sortIndicator,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1162",
                    DeliveryLineText = "Saanen"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeInSecondsMatchingValue = 0,
                MaxFlightTimeInSecondsMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> { "LSGK" },
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllAircraftsExceptListed = false,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Saanen"
            };
            aircraftMappingRule.MatchedAircraftImmatriculations.Add("HB-KCB");
            accountingRuleFilters.Add(aircraftMappingRule);



            var additionalFuelFeeRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter,
                UseRuleForAllAircraftsExceptListed = false,
                MatchedAircraftImmatriculations = new List<string> { "HB-KCB" },
                SortIndicator = 1,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1086",
                    DeliveryLineText = "Treibstoffzuschlag"
                },
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<string> {"LSZK"},
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightTypesExceptListed = true,
                UseRuleForAllLdgLocationsExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 10,
                IsRuleForGliderFlights = true,
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForMotorFlights = true,
                IsRuleForTowingFlights = true,
                RuleFilterName = "HB-KCB Treibstoffzuschlag"
            };
            accountingRuleFilters.Add(additionalFuelFeeRule);

            var noLandingTax = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter,
                IsRuleForGliderFlights = true,
                IsRuleForTowingFlights = true,
                UseRuleForAllStartTypesExceptListed = false,
                UseRuleForAllAircraftsExceptListed = true,
                MatchedAircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true,
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK" },
                NoLandingTaxForGlider = true,
                NoLandingTaxForTowingAircraft = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllStartLocationsExceptListed = true,
                RuleFilterName = "Keine Landetaxen für Schulung ab Speck"
            };
            noLandingTax.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(noLandingTax);
            
            var landingTaxRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter,
                UseRuleForAllAircraftsExceptListed = true,
                MatchedAircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1037",
                    DeliveryLineText = "Landetaxen Speck"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(baseTeachingFlightTypeCodes),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSZK"},
                UseRuleForAllStartTypesExceptListed = true,   //TODO: create start tax for self starting gliders
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 30,
                UseRuleForAllStartLocationsExceptListed = true,
                RuleFilterName = "Landetaxen Speck",
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true
            };
            landingTaxRule.MatchedFlightTypeCodes.AddRange(furtherTrainingFlightTypeCodes);
            accountingRuleFilters.Add(landingTaxRule);

            landingTaxRule = new AccountingRuleFilterDetails
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter,
                UseRuleForAllAircraftsExceptListed = true,
                MatchedAircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1106",
                    DeliveryLineText = "Landetaxen Montricher"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSTR" },
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 30,
                UseRuleForAllStartLocationsExceptListed = true,
                RuleFilterName = "Landetaxen Montricher",
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true
            };
            accountingRuleFilters.Add(landingTaxRule);

            landingTaxRule = new AccountingRuleFilterDetails()
            {
                AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter,
                UseRuleForAllAircraftsExceptListed = true,
                MatchedAircraftImmatriculations = new List<string>(),
                SortIndicator = 1,
                ArticleTarget = new ArticleTargetDetails
                {
                    ArticleNumber = "1160",
                    DeliveryLineText = "Landetaxen Saanen"
                },
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<string> { "LSGK" },
                UseRuleForAllStartTypesExceptListed = true,
                IsRuleForGliderFlights = false,
                IsRuleForTowingFlights = true,
                IsRuleForMotorFlights = true,
                UseRuleForAllClubMemberNumbersExceptListed = true,
                UseRuleForAllFlightCrewTypesExceptListed = true,
                IsActive = true,
                AccountingUnitTypeId = 30,
                UseRuleForAllStartLocationsExceptListed = true,
                RuleFilterName = "Landetaxen Saanen",
                ExtendMatchingFlightTypeCodesToGliderAndTowFlight = true
            };
            accountingRuleFilters.Add(landingTaxRule);


            var accountingRecipientRuleFilter = new AccountingRuleFilterDetails();
            accountingRecipientRuleFilter.AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.RecipientAccountingRuleFilter;
            accountingRecipientRuleFilter.RecipientTarget = new RecipientDetails()
            {
                RecipientName = "FGZO Schnupperflug Gutschein",
                PersonClubMemberNumber = "999006"
            };
            accountingRecipientRuleFilter.MatchedFlightTypeCodes = new List<string>() { "68" };
            accountingRecipientRuleFilter.UseRuleForAllFlightTypesExceptListed = false;
            accountingRecipientRuleFilter.RuleFilterName = "Schnupperflug Gutschein auf FGZO Konto buchen";
            accountingRecipientRuleFilter.Description = "Schnupperflug Gutschein auf FGZO Konto buchen";
            accountingRecipientRuleFilter.IsChargedToClubInternal = true;
            accountingRecipientRuleFilter.IsRuleForGliderFlights = true;
            accountingRuleFilters.Add(accountingRecipientRuleFilter);

            accountingRecipientRuleFilter = new AccountingRuleFilterDetails();
            accountingRecipientRuleFilter.AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.RecipientAccountingRuleFilter;
            accountingRecipientRuleFilter.RecipientTarget = new RecipientDetails()
            {
                RecipientName = "FGZO Schnupperflug und Lufttaufe bar",
                PersonClubMemberNumber = "999000"
            };
            accountingRecipientRuleFilter.MatchedFlightTypeCodes = new List<string>();
            accountingRecipientRuleFilter.MatchedFlightTypeCodes.Add("66"); //Lufttaufe bar
            accountingRecipientRuleFilter.MatchedFlightTypeCodes.Add("69"); //Schnupperflug bar
            accountingRecipientRuleFilter.UseRuleForAllFlightTypesExceptListed = false;
            accountingRecipientRuleFilter.RuleFilterName = "FGZO Schnupperflug und Lufttaufe bar auf FGZO Konto buchen";
            accountingRecipientRuleFilter.Description = "FGZO Schnupperflug und Lufttaufe bar auf FGZO Konto buchen";
            accountingRecipientRuleFilter.IsChargedToClubInternal = true;
            accountingRecipientRuleFilter.IsRuleForGliderFlights = true;
            accountingRuleFilters.Add(accountingRecipientRuleFilter);


            accountingRecipientRuleFilter = new AccountingRuleFilterDetails();
            accountingRecipientRuleFilter.AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.RecipientAccountingRuleFilter;
            accountingRecipientRuleFilter.RecipientTarget = new RecipientDetails()
            {
                RecipientName = "FGZO Passagierflug bar",
                PersonClubMemberNumber = "999001"
            };
            accountingRecipientRuleFilter.MatchedFlightTypeCodes = new List<string>() { "63" };
            accountingRecipientRuleFilter.UseRuleForAllFlightTypesExceptListed = false;
            accountingRecipientRuleFilter.RuleFilterName = "FGZO Passagierflug bar auf FGZO Konto buchen";
            accountingRecipientRuleFilter.Description = "FGZO Passagierflug bar auf FGZO Konto buchen";
            accountingRecipientRuleFilter.IsChargedToClubInternal = true;
            accountingRecipientRuleFilter.IsRuleForGliderFlights = true;
            accountingRuleFilters.Add(accountingRecipientRuleFilter);


            accountingRecipientRuleFilter = new AccountingRuleFilterDetails();
            accountingRecipientRuleFilter.AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.RecipientAccountingRuleFilter;
            accountingRecipientRuleFilter.RecipientTarget = new RecipientDetails()
            {
                RecipientName = "FGZO Passagierflug Gutschein",
                PersonClubMemberNumber = "999007"
            };
            accountingRecipientRuleFilter.MatchedFlightTypeCodes = new List<string>() { "62" };
            accountingRecipientRuleFilter.UseRuleForAllFlightTypesExceptListed = false;
            accountingRecipientRuleFilter.RuleFilterName = "FGZO Passagierflug Gutschein auf FGZO Konto buchen";
            accountingRecipientRuleFilter.Description = "FGZO Passagierflug Gutschein auf FGZO Konto buchen";
            accountingRecipientRuleFilter.IsChargedToClubInternal = true;
            accountingRecipientRuleFilter.IsRuleForGliderFlights = true;
            accountingRuleFilters.Add(accountingRecipientRuleFilter);

            accountingRecipientRuleFilter = new AccountingRuleFilterDetails();
            accountingRecipientRuleFilter.AccountingRuleFilterTypeId = (int)AccountingRuleFilterType.RecipientAccountingRuleFilter;
            accountingRecipientRuleFilter.RecipientTarget = new RecipientDetails()
            {
                RecipientName = "FGZO Marketingflug",
                PersonClubMemberNumber = "999004"
            };
            accountingRecipientRuleFilter.MatchedFlightTypeCodes = new List<string>() { "100" };
            accountingRecipientRuleFilter.UseRuleForAllFlightTypesExceptListed = false;
            accountingRecipientRuleFilter.RuleFilterName = "FGZO Marketingflug auf FGZO Konto buchen";
            accountingRecipientRuleFilter.Description = "FGZO Marketingflug auf FGZO Konto buchen";
            accountingRecipientRuleFilter.IsChargedToClubInternal = true;
            accountingRecipientRuleFilter.IsRuleForGliderFlights = true;
            accountingRuleFilters.Add(accountingRecipientRuleFilter);

            return accountingRuleFilters;
        }
    }
}