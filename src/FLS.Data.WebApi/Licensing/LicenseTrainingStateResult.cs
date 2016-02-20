using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Licensing
{
    public class LicenseTrainingStateResult
    {
        public LicenseTrainingStateRequest Request { get; set; }

        public DateTime DateTimeOfLicenseTrainingStateCalculation { get; set; }

        public LicenseTrainingStateListItem CurrentLicenseTrainingState { get; set; }
    }
}
