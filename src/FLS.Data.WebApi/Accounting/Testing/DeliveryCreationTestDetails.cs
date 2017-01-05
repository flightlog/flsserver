using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class DeliveryCreationTestDetails : FLSBaseData
    {
        public Guid DeliveryCreationTestId { get; set; }

        public Guid FlightId { get; set; }

        public bool IsActive { get; set; }

        public DeliveryDetails ExpectedDeliveryDetails { get; set; }

        public bool IgnoreRecipientName { get; set; }

        public bool IgnoreRecipientAddress { get; set; }

        public bool IgnoreDeliveryInformation { get; set; }

        public bool IgnoreAdditionalInformation { get; set; }

        public bool IgnoreItemPositioning { get; set; }

        public bool IgnoreItemText { get; set; }

        public bool IgnoreItemAdditionalInformation { get; set; }

        public DateTime? LastTestRunOn { get; set; }

        public override Guid Id
        {
            get { return DeliveryCreationTestId; }
            set { DeliveryCreationTestId = value; }
        }
    }
}
