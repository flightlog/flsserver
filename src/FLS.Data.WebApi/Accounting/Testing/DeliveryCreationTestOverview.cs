using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Accounting.RuleFilters;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class DeliveryCreationTestOverview : FLSBaseData
    {
        public DeliveryCreationTestOverview()
        {
            FlightInformationOverview = new FlightInformationOverview();
        }

        public Guid DeliveryCreationTestId { get; set; }

        public FlightInformationOverview FlightInformationOverview { get; set; }

        public string DeliveryCreationTestName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool? LastTestSuccessful { get; set; }

        public string LastTestResultMessage { get; set; }

        public DateTime? LastTestRunOn { get; set; }

        public override Guid Id
        {
            get { return DeliveryCreationTestId; }
            set { DeliveryCreationTestId = value; }
        }
    }
}
