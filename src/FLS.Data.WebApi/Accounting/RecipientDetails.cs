using System;

namespace FLS.Data.WebApi.Accounting
{
    public class RecipientDetails
    {
        public Guid? PersonId { get; set; }

        public string RecipientName { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string PersonClubMemberNumber { get; set; }

        public override string ToString()
        {
            return $"{Lastname} {Firstname}{Environment.NewLine}{AddressLine1}{Environment.NewLine}{AddressLine2}{Environment.NewLine}{ZipCode} {City}";
        }

    }
}
