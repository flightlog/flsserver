using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Settings
{
    public class SettingDetails : FLSBaseData
    {
        public Guid SettingId { get; set; }

        public Guid? ClubId { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        [StringLength(250)]
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        public override Guid Id
        {
            get { return SettingId; }
            set { SettingId = value; }
        }
    }
}
