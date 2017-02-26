using System;

namespace FLS.Data.WebApi.Accounting.RuleFilters
{
    public class AccountingRuleFilterTypeListItem 
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public int AccountingRuleFilterTypeId { get; set; }

        public string AccountingRuleFilterTypeName { get; set; }
    }
}
