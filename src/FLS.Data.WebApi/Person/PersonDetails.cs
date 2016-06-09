using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Person
{
    public class PersonDetails : FLSBaseData
    {
        public Guid PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [StringLength(100)]
        public string Midname { get; set; }

        [StringLength(200)]
        public string AddressLine1 { get; set; }

        [StringLength(200)]
        public string AddressLine2 { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Region { get; set; }

        public Guid? CountryId { get; set; }
        
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(256)]
        [EmailAddress]
        public string BusinessEmail { get; set; }

        [StringLength(256)]
        [EmailAddress]
        public string PrivateEmail { get; set; }

        [StringLength(30)]
        public string MobilePhoneNumber { get; set; }

        [StringLength(30)]
        public string PrivatePhoneNumber { get; set; }

        [StringLength(30)]
        public string BusinessPhoneNumber { get; set; }

        public Nullable<DateTime> Birthday { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        public bool ReceiveOwnedAircraftStatisticReports { get; set; }

        public bool EnableAddress { get; set; }

        public ClubRelatedPersonDetails ClubRelatedPersonDetails { get; set; }
        
        public override Guid Id
        {
            get { return PersonId; }
            set { PersonId = value; }
        }
    }
}
