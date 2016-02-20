using System;

namespace FLS.Data.WebApi.Club
{
    public class ClubOverview : FLSBaseData
    {

        public Guid ClubId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ClubName { get; set; }

        public string CountryName { get; set; }

        public string EmailAddress { get; set; }

        public string HomebaseName { get; set; }

        public string PhoneNumber { get; set; }

        public string WebPage { get; set; }

        public string ZipCode { get; set; }
        
        public override Guid Id
        {
            get { return ClubId; }
            set { ClubId = value; }
        }
    }
}
