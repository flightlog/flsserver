using System;

namespace FLS.Data.WebApi.User
{
    public class UserOverviewSearchFilter
    {
        public string FriendlyName { get; set; }

        public string NotificationEmail { get; set; }

        public string PersonName { get; set; }

        public string UserName { get; set; }

        public string UserRoles { get; set; }

        public string ClubName { get; set; }

        public string AccountState { get; set; }
    }
}
