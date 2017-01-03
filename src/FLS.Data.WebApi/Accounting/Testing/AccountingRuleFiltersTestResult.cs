using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class AccountingRuleFiltersTestResult
    {
        public AccountingRuleFiltersTestResult()
        {
            MatchedAccountingRuleFilters = new List<AccountingRuleFilterOverview>();
        }

        public DeliveryDetails DeliveryDetails { get; set; }

        public List<AccountingRuleFilterOverview> MatchedAccountingRuleFilters { get; set; }

        public bool IsTestSuccessful { get; set; }

        public string Errors { get; set; }

    }
}
