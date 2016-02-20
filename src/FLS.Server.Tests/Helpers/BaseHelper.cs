using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;

namespace FLS.Server.Tests.Helpers
{
    public class BaseHelper
    {
        public BaseHelper(DataAccessService dataAccessService, IdentityService identityService)
        {
            DataAccessService = dataAccessService;
            IdentityService = identityService;
        }

        protected DataAccessService DataAccessService { get; set; }
        protected IdentityService IdentityService { get; set; }

        public void SetUser(string username)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
                IdentityService.SetUser(user);
            }
        }
    }
}
