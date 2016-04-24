using System;
using System.Collections.Generic;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    public class VSFMappingRule
    {
        public bool AddVFSFeePerLanding { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        public List<Guid> MatchedLdgLocations { get; set; }

        public VSFMappingRule()
        {
        }
    }
}
