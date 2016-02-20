using FLS.Server.Data.DbEntities;
using FLS.Server.Service;

namespace FLS.Server.TestInfrastructure
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

        public void SetUser(User user)
        {
            IdentityService.SetUser(user);
        }
    }
}
