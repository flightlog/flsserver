using System;
using System.Collections.Generic;

namespace FLS.Server.Service.Invoicing
{
    public class VFSMappingRule
    {
        public bool AddVFSFeePerLanding { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllLdgLocationsExceptListed { get; set; }
        public List<Guid> MatchedLdgLocations { get; set; }

        public VFSMappingRule()
        {
        }
    }
}
