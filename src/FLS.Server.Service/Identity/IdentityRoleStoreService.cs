using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Utilities;
using FLS.Common.Validators;

namespace FLS.Server.Service.Identity
{
    /// <summary>
    ///     EntityFramework based implementation
    /// </summary>
    public class IdentityRoleStoreService : IQueryableRoleStore<Role, Guid>
    {
        private readonly DataAccessService _dataAccessService;
        private bool _disposed;
        private EntityStore<Role> _roleStore;

        /// <summary>
        ///     Constructor which takes a db context and wires up the stores with default instances using the context
        /// </summary>
        public IdentityRoleStoreService(DataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
            Context = _dataAccessService.CreateDbContext();
            _roleStore = new EntityStore<Role>(Context);
        }

        /// <summary>
        ///     Context for the store
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        ///     If true will call dispose on the DbContext during Dipose
        /// </summary>
        public bool DisposeContext { get; set; }

        /// <summary>
        ///     Find a role by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<Role> FindByIdAsync(Guid roleId)
        {
            ThrowIfDisposed();
            return _roleStore.GetByIdAsync(roleId);
        }

        /// <summary>
        ///     Find a role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<Role> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();
            return _roleStore.EntitySet.FirstOrDefaultAsync(u => u.RoleApplicationKeyString.ToUpper() == roleName.ToUpper());
        }

        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="role"></param>
        public virtual async Task CreateAsync(Role role)
        {
            ThrowIfDisposed();
            role.ArgumentNotNull("role");
            _roleStore.Create(role);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        ///     Mark an entity for deletion
        /// </summary>
        /// <param name="role"></param>
        public virtual async Task DeleteAsync(Role role)
        {
            ThrowIfDisposed();
            role.ArgumentNotNull("role");
            _roleStore.Delete(role);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="role"></param>
        public virtual async Task UpdateAsync(Role role)
        {
            ThrowIfDisposed();
            role.ArgumentNotNull("role");
            _roleStore.Update(role);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        ///     Returns an IQueryable of users
        /// </summary>
        public IQueryable<Role> Roles
        {
            get { return _roleStore.EntitySet; }
        }

        /// <summary>
        ///     Dispose the store
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        ///     If disposing, calls dispose on the Context.  Always nulls out the Context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposeContext && disposing && Context != null)
            {
                Context.Dispose();
            }
            _disposed = true;
            Context = null;
            _roleStore = null;
        }
    }
}
