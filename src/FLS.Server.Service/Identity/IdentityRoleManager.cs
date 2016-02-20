using System;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FLS.Server.Service.Identity
{
    public class IdentityRoleManager : RoleManager<Role, Guid>
    {
        public IdentityRoleManager(IRoleStore<Role, Guid> store)
            : base(store)
        {
        }

        public static IdentityRoleManager Create(IdentityFactoryOptions<IdentityRoleManager> options, IOwinContext context)
        {
            var manager = new IdentityRoleManager(new RoleStoreService());
            
            return manager;
        }
    }
}
