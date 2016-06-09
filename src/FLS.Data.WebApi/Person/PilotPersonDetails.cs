using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Person
{
    public class PilotPersonDetails : PersonDetails
    {
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
