using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class LastDeliveryCreationTestResult
    {
        public DateTime? LastTestRunOn { get; set; }

        public bool? LastTestSuccessful { get; set; }

        public string LastTestResultMessage { get; set; }

        public DeliveryDetails LastTestCreatedDeliveryDetails { get; set; }

        public List<Guid> LastTestMatchedAccountingRuleFilterIds { get; set; }

        public List<AccountingRuleFilterOverview> MatchedAccountingRuleFilters { get; set; }
    }
}
