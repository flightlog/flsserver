using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class DeliveryCreationTestResultDetails
    {
        public DeliveryCreationTestResultDetails()
        {
            MatchedAccountingRuleFilters = new List<AccountingRuleFilterOverview>();
        }

        public Guid DeliveryCreationTestId { get; set; }

        public Guid FlightId { get; set; }

        public string DeliveryCreationTestName { get; set; }

        public DeliveryDetails ExpectedDeliveryDetails { get; set; }

        public DeliveryDetails CreatedDeliveryDetails { get; set; }

        public List<AccountingRuleFilterOverview> MatchedAccountingRuleFilters { get; set; }

        public bool LastTestSuccessful { get; set; }

        public string ResultMessage { get; set; }

        public DateTime LastTestRunOn { get; set; }

    }
}
