using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace FLS.Server.Service.Invoicing
{
    public class InvoiceMappingFactory
    {
        private readonly DataAccessService _dataAccessService;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public InvoiceMappingFactory(DataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }


        internal InvoiceMapping CreateInvoiceMapping()
        {
            var invoiceMapping = new InvoiceMapping();
            var lszkId = GetLocationId("LSZK");
            var saanenId = GetLocationId("LSGK");
            var montricherId = GetLocationId("LSTR");

            invoiceMapping.IsErrorWhenNoAdditionalFuelFeeRuleMatches = false;
            invoiceMapping.IsErrorWhenNoLandingTaxRuleMatches = false;
            invoiceMapping.IsErrorWhenNoVFSFeeRuleMatches = false;
            invoiceMapping.VFSMappingRule = new VFSMappingRule
            {
                AddVFSFeePerLanding = true,
                ERPArticleNumber = "1003",
                InvoiceLineText = "VFS-Gebühr",
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId }
            };

            invoiceMapping.FlightCodesForInstructorFee.Add("70"); //Grundschulung Doppelsteuer
            invoiceMapping.FlightCodesForInstructorFee.Add("77"); //Weiterbildung Doppelsteuer
            invoiceMapping.FlightCodesForInstructorFee.Add("80"); //Grundschulung Solo
            invoiceMapping.FlightCodesForInstructorFee.Add("88"); //Weiterbildung Solo
            invoiceMapping.FlightCodesForInstructorFee.Add("78"); //Jahres-Checkflug
            invoiceMapping.FlightCodesForInstructorFee.Add("66"); //Lufttaufe bar
            invoiceMapping.FlightCodesForInstructorFee.Add("68"); //Schnupperflug Gutschein
            invoiceMapping.FlightCodesForInstructorFee.Add("69"); //Schnupperflug bar

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
            var aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1059",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1061",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1063",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1065",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1072",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator = 1;
            aircraftId = GetAircraftId("HB-3256");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1058",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-3407");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1060",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1841");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1062",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-1824");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1064",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-2464");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1071",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllStartLocationsExceptListed = true
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            //Towing Aircrafts
            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1068",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1069",
                InvoiceLineText = "Schulung",
                UseRuleForAllFlightTypesExceptListed = false,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1066",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = 10,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1067",
                InvoiceLineText = "Privat",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                MinFlightTimeMatchingValue = 10,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = true,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PFW");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1153",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KIO");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1154",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-PDL");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1155",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-EQC");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1156",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-WAT");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1157",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DGP");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1158",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KDO");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1159",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-DCU");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1161",
                InvoiceLineText = "",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false
            };
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);

            sortIndicator++;
            aircraftId = GetAircraftId("HB-KCB");
            aircraftMappingRule = new AircraftMappingRule
            {
                AircraftId = aircraftId,
                SortIndicator = sortIndicator,
                ERPArticleNumber = "1162",
                InvoiceLineText = "Saanen",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(),
                MinFlightTimeMatchingValue = 0,
                MaxFlightTimeMatchingValue = int.MaxValue,
                UseRuleBelowFlightTimeMatchingValue = false,
                UseRuleForAllStartLocationsExceptListed = false,
                MatchedStartLocations = new List<Guid>()
            };
            aircraftMappingRule.MatchedStartLocations.Add(lszkId);
            invoiceMapping.AircraftERPArticleMapping.Add(aircraftMappingRule);



            var additionalFuelFeeRule = new AdditionalFuelFeeRule
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

            var aircraftIds = new List<Guid>();
            aircraftId = GetAircraftId("HB-3256");
            aircraftIds.Add(aircraftId);
            aircraftId = GetAircraftId("HB-3407");
            aircraftIds.Add(aircraftId);
            aircraftId = GetAircraftId("HB-1841");
            aircraftIds.Add(aircraftId);
            aircraftId = GetAircraftId("HB-1824");
            aircraftIds.Add(aircraftId);
            aircraftId = GetAircraftId("HB-2464");
            aircraftIds.Add(aircraftId);

            //TODO: handle aircraft selection for landing taxes
            var landingTaxRule = new LandingTaxRule
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1037",
                InvoiceLineText = "Landetaxen Speck",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = false
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRule
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
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = false
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRule
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
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = false
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            //selfstarting gliders
            landingTaxRule = new LandingTaxRule
            {
                UseRuleForAllAircraftsExceptListed = true,
                AircraftIds = new List<Guid>(),
                SortIndicator = 1,
                ERPArticleNumber = "1037",
                InvoiceLineText = "Landetaxen Speck",
                UseRuleForAllFlightTypesExceptListed = true,
                MatchedFlightTypeCodes = new List<string>(invoiceMapping.FlightCodesForInstructorFee),
                UseRuleForAllLdgLocationsExceptListed = false,
                MatchedLdgLocations = new List<Guid> { lszkId },
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = true
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRule
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
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = true
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            landingTaxRule = new LandingTaxRule
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
                IncludesTowingLandingTaxes = true,
                IsRuleForSelfstartedGliderFlights = true
            };
            invoiceMapping.LandingTaxRules.Add(landingTaxRule);

            return invoiceMapping;
        }

        internal Guid GetAircraftId(string immatriculation)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraft =
                    context.Aircrafts.FirstOrDefault(a => a.Immatriculation.ToUpper() == immatriculation.ToUpper());

                if (aircraft != null)
                {
                    return aircraft.AircraftId;
                }

                Logger.Warn(string.Format("Aircraft with immatriculation: {0} not found!",
                                                                immatriculation));

                return Guid.Empty;
            }
        }

        internal Guid GetLocationId(string icaoCode)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var location =
                    context.Locations.FirstOrDefault(
                        a => a.IcaoCode.ToUpper() == icaoCode.ToUpper());

                if (location != null)
                {
                    return location.LocationId;
                }

                Logger.Warn(string.Format("Location with IcaoCode: {0} not found!", icaoCode));

                return Guid.Empty;
            }
        }
    }
}