using System;

namespace FLS.Data.WebApi.Accounting.RuleFilters
{
    public class AccountingRuleFilterOverviewSearchFilter
    {
        public string RuleFilterName { get; set; }

        public string Target { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public string SortIndicator { get; set; }

        public string AccountingRuleFilterTypeName { get; set; }
    }
}