using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Data.WebApi.Invoicing.RuleFilters
{
    public class RecipientTarget
    {
        public string DisplayName { get; set; }

        public string MemberNumber { get; set; }

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }
    }
}
