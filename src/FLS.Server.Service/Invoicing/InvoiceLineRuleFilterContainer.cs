using System.Collections.Generic;
using FLS.Data.WebApi.Invoicing.RuleFilters;

namespace FLS.Server.Service.Invoicing
{
    internal class InvoiceLineRuleFilterContainer
    {
        public List<AircraftRuleFilter> AircraftRuleFilters { get; set; }
        ///// <summary>
        ///// Gets the instructor to erp article mapping.
        ///// </summary>
        ///// <key>The MemberNumber of the PersonClub of the instructor.</key>
        ///// <value>
        ///// The target ERPArticleNumber.
        ///// </value>
        public Dictionary<string, string> InstructorToArticleMapping { get; set; }

        public List<AdditionalFuelFeeRuleFilter> AdditionalFuelFeeRuleFilters { get; set; }

        public List<NoLandingTaxRuleFilter> NoLandingTaxRuleFilters { get; set; }
        public List<LandingTaxRuleFilter> LandingTaxRuleFilters { get; set; }
        public List<VsfFeeRuleFilter> VsfFeeRuleFilters { get; set; }

        public InvoiceLineRuleFilterContainer()
        {
            AircraftRuleFilters = new List<AircraftRuleFilter>();
            InstructorToArticleMapping = new Dictionary<string, string>();
            AdditionalFuelFeeRuleFilters = new List<AdditionalFuelFeeRuleFilter>();
            LandingTaxRuleFilters = new List<LandingTaxRuleFilter>();
            NoLandingTaxRuleFilters = new List<NoLandingTaxRuleFilter>();
            VsfFeeRuleFilters = new List<VsfFeeRuleFilter>();
        }

    }
}
