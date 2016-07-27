using System;

namespace FLS.Data.WebApi.Invoicing
{
    public class RecipientDetails
    {
        public string RecipientName { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string PersonClubMemberNumber { get; set; }
    }
}
