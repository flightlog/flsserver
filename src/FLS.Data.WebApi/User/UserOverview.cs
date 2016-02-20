using System;

namespace FLS.Data.WebApi.User
{
    public class UserOverview : FLSBaseData
    {
        
        public Guid UserId { get; set; }
        
        public string FriendlyName { get; set; }

        public string NotificationEmail { get; set; }

        public string PersonName { get; set; }

        public string UserName { get; set; }

        public string UserRoles { get; set; }

        public string ClubName { get; set; }

        public int AccountState { get; set; }

        public override Guid Id
        {
            get { return UserId; }
            set { UserId = value; }
        }
    }
}
