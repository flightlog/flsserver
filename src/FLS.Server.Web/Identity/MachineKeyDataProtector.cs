using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace FLS.Server.WebApi.Identity
{
    /// <summary>
    /// http://stackoverflow.com/questions/28606676/how-can-i-instantiate-owin-idataprotectionprovider-in-azure-web-jobs
    /// </summary>
    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(params string[] purposes)
        {
            _purposes = purposes;
        }

        public virtual byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public virtual byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }

}