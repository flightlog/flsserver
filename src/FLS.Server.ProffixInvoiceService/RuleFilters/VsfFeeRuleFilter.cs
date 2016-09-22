using System;
using System.Collections.Generic;

namespace FLS.Server.ProffixInvoiceService.RuleFilters
{
    public class VsfFeeRuleFilter : BaseInvoiceLineRuleFilter
    {
        public bool AddVsfFeePerLanding { get; set; }

    }
}
