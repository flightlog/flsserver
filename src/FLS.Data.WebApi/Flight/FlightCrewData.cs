using System;

namespace FLS.Data.WebApi.Flight
{
    public class FlightCrewData
    {
        public Guid PersonId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string PersonClubMemberNumber { get; set; }
    }
}
