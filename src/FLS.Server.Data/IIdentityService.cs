using FLS.Server.Data.DbEntities;

namespace FLS.Server.Data
{
    public interface IIdentityService
    {
        User CurrentAuthenticatedFLSUser { get; }
        void SetUser(User user);
    }
}