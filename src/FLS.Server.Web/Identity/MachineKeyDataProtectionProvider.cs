using Microsoft.Owin.Security.DataProtection;

namespace FLS.Server.WebApi.Identity
{
    /// <summary>
    /// Used to provide the data protection services that are derived from the MachineKey API. It is the best choice of
    /// data protection when you application is hosted by ASP.NET and all servers in the farm are running with the same Machine Key values.
    /// http://stackoverflow.com/questions/28606676/how-can-i-instantiate-owin-idataprotectionprovider-in-azure-web-jobs
    /// </summary>
    public class MachineKeyDataProtectionProvider : IDataProtectionProvider
    {
        /// <summary>
        /// Returns a new instance of IDataProtection for the provider.
        /// </summary>
        /// <param name="purposes">Additional entropy used to ensure protected data may only be unprotected for the correct purposes.</param>
        /// <returns>An instance of a data protection service</returns>
        public virtual IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

}