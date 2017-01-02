using System;

namespace FLS.Data.WebApi.Accounting.RuleFilters
{
    public class AccountingRuleFilterOverview : FLSBaseData
    {

        public Guid AccountingRuleFilterId { get; set; }

        public string RuleFilterName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int SortIndicator { get; set; }

        public int AccountingRuleFilterTypeId { get; set; }
        

        public override Guid Id
        {
            get { return AccountingRuleFilterId; }
            set { AccountingRuleFilterId = value; }
            
        }
    }
}