using FLS.Server.Data;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service
{
    public class IdentityService : IIdentityService
    {
        private User _authenticatedUser = null;

        public IdentityService()
        {
            
        }

        public User CurrentAuthenticatedFLSUser
        {
            get { return _authenticatedUser; }
        }

        public void SetUser(User user)
        {
            _authenticatedUser = user;
        }
    }
}
