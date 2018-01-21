using System;

namespace FLS.Data.WebApi.Dashboard
{
    public class PersonDashboardDetails
    {
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

        public bool HasGliderInstructorLicence { get; set; }

        public bool HasGliderPilotLicence { get; set; }

        public bool HasGliderTraineeLicence { get; set; }

        public bool HasMotorPilotLicence { get; set; }

        public bool HasTowPilotLicence { get; set; }

        public bool HasGliderPassengerLicence { get; set; }

        public bool HasTMGLicence { get; set; }

        public bool HasPartMLicence { get; set; }

    }
}
