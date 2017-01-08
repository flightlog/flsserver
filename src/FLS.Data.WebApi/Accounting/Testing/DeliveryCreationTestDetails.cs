using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class DeliveryCreationTestDetails : FLSBaseData
    {
        public DeliveryCreationTestDetails()
        {
            IsActive = true;
            ExpectedMatchedAccountingRuleFilterIds = new List<Guid>();
        }

        public Guid DeliveryCreationTestId { get; set; }

        public Guid FlightId { get; set; }

        public bool IsActive { get; set; }

        [StringLength(250)]
        public string DeliveryCreationTestName { get; set; }

        public string Description { get; set; }

        public DeliveryDetails ExpectedDeliveryDetails { get; set; }

        public List<Guid> ExpectedMatchedAccountingRuleFilterIds { get; set; }

        public bool MustNotCreateDeliveryForFlight { get; set; }

        public bool IgnoreRecipientName { get; set; }

        public bool IgnoreRecipientAddress { get; set; }

        public bool IgnoreRecipientPersonId { get; set; }

        public bool IgnoreRecipientClubMemberNumber { get; set; }

        public bool IgnoreDeliveryInformation { get; set; }

        public bool IgnoreAdditionalInformation { get; set; }

        public bool IgnoreItemPositioning { get; set; }

        public bool IgnoreItemText { get; set; }

        public bool IgnoreItemAdditionalInformation { get; set; }

        public LastDeliveryCreationTestResult LastDeliveryCreationTestResult { get; set; }

        public override Guid Id
        {
            get { return DeliveryCreationTestId; }
            set { DeliveryCreationTestId = value; }
        }
    }
}
