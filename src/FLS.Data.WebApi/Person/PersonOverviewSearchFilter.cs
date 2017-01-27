using System;

namespace FLS.Data.WebApi.Person
{
    public class PersonOverviewSearchFilter
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string PrivateEmail { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string MemberStateName { get; set; }

        public bool? HasGliderInstructorLicence { get; set; }

        public bool? HasGliderPilotLicence { get; set; }

        public bool? HasGliderTraineeLicence { get; set; }

        public bool? HasMotorPilotLicence { get; set; }

        public bool? HasTMGLicence { get; set; }

        public bool? HasTowPilotLicence { get; set; }

        public bool? HasGliderPassengerLicence { get; set; }

        public bool? HasWinchOperatorLicence { get; set; }

        public bool? HasMotorInstructorLicence { get; set; }

        public string LicenceNumber { get; set; }

        public bool? OnlyClubRelatedPersons { get; set; }
    }
}
