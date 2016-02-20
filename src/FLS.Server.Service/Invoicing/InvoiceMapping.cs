using System.Collections.Generic;

namespace FLS.Server.Service.Invoicing
{
    internal class InvoiceMapping
    {
        public Dictionary<string, InvoiceRecipientTarget> FlightTypeToInvoiceRecipientMapping { get; set; } 
        public List<AircraftMappingRule> AircraftERPArticleMapping { get; set; }
        /// <summary>
        /// Gets the instructor to erp article mapping.
        /// </summary>
        /// <key>The MemberKey of the PersonClub of the instructor.</key>
        /// <value>
        /// The target ERPArticleNumber.
        /// </value>
        public Dictionary<string, string> InstructorToERPArticleMapping { get; set; }
        public List<string> FlightCodesForInstructorFee { get; set; }
        public bool IsErrorWhenNoAdditionalFuelFeeRuleMatches { get; set; }
        public List<AdditionalFuelFeeRule> AdditionalFuelFeeRules { get; set; }
        public bool IsErrorWhenNoLandingTaxRuleMatches { get; set; }
        public bool IsErrorWhenNoVFSFeeRuleMatches { get; set; }
        public List<LandingTaxRule> LandingTaxRules { get; set; }
        public VFSMappingRule VFSMappingRule { get; set; }

        public InvoiceMapping()
        {
            FlightTypeToInvoiceRecipientMapping = new Dictionary<string, InvoiceRecipientTarget>();
            AircraftERPArticleMapping = new List<AircraftMappingRule>();
            InstructorToERPArticleMapping = new Dictionary<string, string>();
            FlightCodesForInstructorFee = new List<string>();
            AdditionalFuelFeeRules = new List<AdditionalFuelFeeRule>();
            LandingTaxRules = new List<LandingTaxRule>();
        }

    }
}
