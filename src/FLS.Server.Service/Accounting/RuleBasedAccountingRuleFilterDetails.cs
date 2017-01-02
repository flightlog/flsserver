using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Accounting
{
    public class RuleBasedAccountingRuleFilterDetails : AccountingRuleFilterDetails
    {
        public RuleBasedAccountingRuleFilterDetails(AccountingRuleFilterDetails accountingRuleFilterDetails)
        {
            Type t = typeof(AccountingRuleFilterDetails);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                pi.SetValue(this, pi.GetValue(accountingRuleFilterDetails, null), null);
            }
        }

        public List<Guid> MatchedAircraftIds { get; set; }

        public List<Guid> MatchedStartLocationIds { get; set; }

        public List<Guid> MatchedLdgLocationIds { get; set; }
    }
}
