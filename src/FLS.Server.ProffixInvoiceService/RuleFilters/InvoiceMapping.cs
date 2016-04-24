using System.Collections.Generic;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    internal class InvoiceMapping
    {
        public Dictionary<string, InvoiceRecipientTarget> FlightTypeToInvoiceRecipientMapping { get; set; } 
        public List<AircraftMapping> AircraftERPArticleMapping { get; set; }
        ///// <summary>
        ///// Gets the instructor to erp article mapping.
        ///// </summary>
        ///// <key>The MemberKey of the PersonClub of the instructor.</key>
        ///// <value>
        ///// The target ERPArticleNumber.
        ///// </value>
        public Dictionary<string, string> InstructorToERPArticleMapping { get; set; }

        //public bool IsErrorWhenNoAdditionalFuelFeeRuleMatches { get; set; }

        public List<AdditionalFuelFee> AdditionalFuelFeeRules { get; set; }

        //public bool IsErrorWhenNoLandingTaxRuleMatches { get; set; }
        //public bool IsErrorWhenNoVFSFeeRuleMatches { get; set; }

        public List<LandingTax> LandingTaxRules { get; set; }
        //public VFSMappingRule VFSMappingRule { get; set; }

        public InvoiceMapping()
        {
            FlightTypeToInvoiceRecipientMapping = new Dictionary<string, InvoiceRecipientTarget>();
            AircraftERPArticleMapping = new List<AircraftMapping>();
            InstructorToERPArticleMapping = new Dictionary<string, string>();
            AdditionalFuelFeeRules = new List<AdditionalFuelFee>();
            LandingTaxRules = new List<LandingTax>();
        }

    }
}
