using System;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;

namespace FLS.Server.WebApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityRoleManager : RoleManager<Role, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRoleManager"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public IdentityRoleManager(IRoleStore<Role, Guid> store)
            : base(store)
        {
        }
    }
}
