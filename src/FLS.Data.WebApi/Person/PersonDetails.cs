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

        public bool HasGliderInstructorLicence { get; set; }

        public bool HasGliderPilotLicence { get; set; }

        public bool HasGliderTraineeLicence { get; set; }

        public bool HasMotorPilotLicence { get; set; }

        public bool HasTowPilotLicence { get; set; }

        public bool HasGliderPassengerLicence { get; set; }

        public bool HasTMGLicence { get; set; }

        public bool HasWinchOperatorLicence { get; set; }

        public bool HasMotorInstructorLicence { get; set; }

        [StringLength(20)]
        public string LicenceNumber { get; set; }

        public DateTime? MedicalClass1ExpireDate { get; set; }

        public DateTime? MedicalClass2ExpireDate { get; set; }

        public DateTime? MedicalLaplExpireDate { get; set; }

        public DateTime? GliderInstructorLicenceExpireDate { get; set; }

        public DateTime? MotorInstructorLicenceExpireDate { get; set; }

        public DateTime? PartMLicenceExpireDate { get; set; }

        public bool HasGliderTowingStartPermission { get; set; }

        public bool HasGliderSelfStartPermission { get; set; }

        public bool HasGliderWinchStartPermission { get; set; }

        [StringLength(250)]
        public string SpotLink { get; set; }

        public override Guid Id
        {
            get { return PersonId; }
            set { PersonId = value; }
        }
    }
}
