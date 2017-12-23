using System;

namespace FLS.Data.WebApi.Settings
{
    public class SettingDetailsSearchFilter
    {
        public Guid? ClubId { get; set; }

        public Guid? UserId { get; set; }

        public string SettingKey { get; set; }
    }
}
