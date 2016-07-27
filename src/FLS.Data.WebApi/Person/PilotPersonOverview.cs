using System;

namespace FLS.Data.WebApi.Person
{
    public class PilotPersonOverview : PersonOverview
    {
        
        public bool HasGliderInstructorLicence { get; set; }

        public bool HasGliderPilotLicence { get; set; }

        public bool HasGliderTraineeLicence { get; set; }

        public bool HasMotorPilotLicence { get; set; }

        public bool HasTMGLicence { get; set; }

        public bool HasTowPilotLicence { get; set; }

        public bool HasGliderPassengerLicence { get; set; }

        public bool HasWinchOperatorLicence { get; set; }

        public bool HasMotorInstructorLicence { get; set; }

        public string LicenceNumber { get; set; }
        
    }
}
