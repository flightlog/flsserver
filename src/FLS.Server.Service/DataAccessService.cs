using FLS.Server.Data;

namespace FLS.Server.Service
{
    public class DataAccessService
    {
        private readonly IdentityService _identityService;

        public DataAccessService(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public FLSDataEntities CreateDbContext()
        {
            return new FLSDataEntities(_identityService);
        }

        public IIdentityService IdentityService
        {
            get { return _identityService; }
        }
    }
}
