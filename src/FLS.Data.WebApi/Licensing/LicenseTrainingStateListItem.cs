using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Licensing
{
    public class LicenseTrainingStateListItem
    {
        public int LicenseTrainingStateId { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseTrainingStateName { get; set; }

        public bool? CanFly { get; set; }

        public bool? RequiresInstructor { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }
        
    }
}
