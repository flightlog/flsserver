using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class DeliveryCreationResult
    {
        public DeliveryCreationResult()
        {
            MatchedAccountingRuleFilterIds = new List<Guid>();
            MatchedAccountingRuleFilters = new List<AccountingRuleFilterOverview>();
        }

        public Guid FlightId { get; set; }

        public DeliveryDetails CreatedDeliveryDetails { get; set; }

        public List<Guid> MatchedAccountingRuleFilterIds { get; set; }

        public List<AccountingRuleFilterOverview> MatchedAccountingRuleFilters { get; set; }
    }
}
