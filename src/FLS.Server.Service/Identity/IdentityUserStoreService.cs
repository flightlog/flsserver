using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using Constants = FLS.Server.Data.Resources.Constants;

namespace FLS.Server.Service.Identity
{
    /// <summary>
    /// Implementation based on http://www.symbolsource.org/MyGet/Metadata/aspnetwebstacknightly/Project/Microsoft.AspNet.Identity.EntityFramework/2.3.0-rtm-150806/Release/Default/Microsoft.AspNet.Identity.EntityFramework
    /// </summary>
    public class IdentityUserStoreService : IUserStore<User, Guid>, IUserPasswordStore<User, Guid>, 
        IUserRoleStore<User, Guid>, IUserLockoutStore<User, Guid>,
        IUserEmailStore<User, Guid>, IUserSecurityStampStore<User, Guid>,
        IQueryableUserStore<User, Guid>
    {
        private readonly DataAccessService _dataAccessService;
        //private readonly IDbSet<TUserLogin> _logins;
        private readonly EntityStore<Role> _roleStore;
        //private readonly IDbSet<TUserClaim> _userClaims;
        private readonly IDbSet<UserRole> _userRoles;
        private bool _disposed;
        private EntityStore<User> _userStore;

        public IdentityUserStoreService(DataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
            Context = _dataAccessService.CreateDbContext();
            AutoSaveChanges = true;
            _userStore = new EntityStore<User>(Context);
            _roleStore = new EntityStore<Role>(Context);
            //_logins = Context.Set<UserLogin>();
            //_userClaims = Context.Set<UserClaim>();
            _userRoles = Context.Set<UserRole>();
        }

        /// <summary>
        ///     Context for the store
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        ///     If true will call dispose on the DbContext during Dispose
        /// </summary>
        public bool DisposeContext { get; set; }

        /// <summary>
        ///     If true will call SaveChanges after Create/Update/Delete
        /// </summary>
        public bool AutoSaveChanges { get; set; }

        /// <summary>
        ///     Returns an IQueryable of users
        /// </summary>
        public IQueryable<User> Users
        {
            get { return _userStore.EntitySet; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            _userStore = null;
        }

        public async Task CreateAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            _userStore.Create(user);
            await SaveChanges().WithCurrentCulture();
        }

        public async Task UpdateAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            _userStore.Update(user);
            await SaveChanges().WithCurrentCulture();
        }

        public async Task DeleteAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            _userStore.Delete(user);
            await SaveChanges().WithCurrentCulture();
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.Id.Equals(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper());
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            ThrowIfDisposed();
            user.NotNull("user");
            passwordHash.NotNullOrEmpty("passwordHash");
            user.PasswordHash = passwordHash;
            user.LastPasswordChangeOn = DateTime.UtcNow;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            user.NotNull("user");
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            roleName.NotNullOrEmpty("roleName");

            if (IsInRoleAsync(user, roleName).Result == false)
            {
                var roleEntity =
                    await
                        _roleStore.DbEntitySet.SingleOrDefaultAsync(
                            r => r.RoleApplicationKeyString.ToUpper() == roleName.ToUpper()).WithCurrentCulture();
                roleEntity.EntityNotNull("Role");

                var ur = new UserRole {UserId = user.Id, RoleId = roleEntity.Id};
                _userRoles.Add(ur);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            roleName.NotNullOrEmpty("roleName");
            var roleEntity = await _roleStore.DbEntitySet.SingleOrDefaultAsync(r => r.RoleApplicationKeyString.ToUpper() == roleName.ToUpper()).WithCurrentCulture();
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userId = user.Id;
                var userRole = await _userRoles.FirstOrDefaultAsync(r => roleId.Equals(r.RoleId) && r.UserId.Equals(userId)).WithCurrentCulture();
                if (userRole != null)
                {
                    _userRoles.Remove(userRole);
                }
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            var userId = user.UserId;
            var query = from userRole in _userRoles
                        where userRole.UserId.Equals(userId)
                        join role in _roleStore.DbEntitySet on userRole.RoleId equals role.RoleId
                        select role.RoleApplicationKeyString;
            return await query.ToListAsync().WithCurrentCulture();
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            roleName.NotNullOrEmpty("roleName");
            var role = await _roleStore.DbEntitySet.SingleOrDefaultAsync(r => r.RoleApplicationKeyString.ToUpper() == roleName.ToUpper()).WithCurrentCulture();
            if (role != null)
            {
                var userId = user.UserId;
                var roleId = role.RoleId;
                return await _userRoles.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId)).WithCurrentCulture();
            }
            return false;
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.AccessFailedCount++;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.AccessFailedCount = 0;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.LockoutEnabled = enabled;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(User user, string email)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.NotificationEmail = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return Task.FromResult(user.NotificationEmail);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.EmailConfirmed = confirmed;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.NotificationEmail.ToUpper() == email.ToUpper());
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");
            user.SecurityStamp = stamp;
            user.DoNotUpdateMetaData = true;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            ThrowIfDisposed();
            user.ArgumentNotNull("user");

            return Task.FromResult(user.SecurityStamp);
        }
        
        // Only call save changes if AutoSaveChanges is true
        private async Task SaveChanges()
        {
            if (AutoSaveChanges)
            {
                Context.SaveChanges();
            }
        }

        //private bool AreClaimsLoaded(User user)
        //{
        //    return Context.Entry(user).Collection(u => u.Claims).IsLoaded;
        //}

        //private async Task EnsureClaimsLoaded(User user)
        //{
        //    if (!AreClaimsLoaded(user))
        //    {
        //        var userId = user.Id;
        //        await _userClaims.Where(uc => uc.UserId.Equals(userId)).LoadAsync().WithCurrentCulture();
        //        Context.Entry(user).Collection(u => u.Claims).IsLoaded = true;
        //    }
        //}

        private async Task EnsureRolesLoaded(User user)
        {
            if (!Context.Entry(user).Collection(u => u.UserRoles).IsLoaded)
            {
                var userId = user.UserId;
                await _userRoles.Where(uc => uc.UserId.Equals(userId)).LoadAsync().WithCurrentCulture();
                Context.Entry(user).Collection(u => u.UserRoles).IsLoaded = true;
            }
        }

        //private bool AreLoginsLoaded(User user)
        //{
        //    return Context.Entry(user).Collection(u => u.Logins).IsLoaded;
        //}

        //private async Task EnsureLoginsLoaded(User user)
        //{
        //    if (!AreLoginsLoaded(user))
        //    {
        //        var userId = user.Id;
        //        await _logins.Where(uc => uc.UserId.Equals(userId)).LoadAsync().WithCurrentCulture();
        //        Context.Entry(user).Collection(u => u.Logins).IsLoaded = true;
        //    }
        //}

        /// <summary>
        /// Used to attach child entities to the User aggregate, i.e. Roles, Logins, and Claims
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual async Task<User> GetUserAggregateAsync(Expression<Func<User, bool>> filter)
        {
            Guid id;
            User user;
            if (FindByIdFilterParser.TryMatchAndGetId(filter, out id))
            {
                user = await _userStore.GetByIdAsync(id).WithCurrentCulture();
            }
            else
            {
                user = await Users
                    .Include(Constants.UserRoles)
                    .Include(Constants.UserRolesRole)
                    .FirstOrDefaultAsync(filter)
                    .WithCurrentCulture();
            }
            if (user != null)
            {
                //await EnsureClaimsLoaded(user).WithCurrentCulture();
                //await EnsureLoginsLoaded(user).WithCurrentCulture();
                await EnsureRolesLoaded(user).WithCurrentCulture();
            }
            return user;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        
        // We want to use FindAsync() when looking for an User.Id instead of LINQ to avoid extra 
        // database roundtrips. This class cracks open the filter expression passed by 
        // UserStore.FindByIdAsync() to obtain the value of the id we are looking for 
        private static class FindByIdFilterParser
        {
            // expression pattern we need to match
            private static readonly Expression<Func<User, bool>> Predicate = u => u.Id.Equals(default(Guid));
            // method we need to match: Object.Equals() 
            private static readonly MethodInfo EqualsMethodInfo = ((MethodCallExpression)Predicate.Body).Method;
            // property access we need to match: User.Id 
            private static readonly MemberInfo UserIdMemberInfo = ((MemberExpression)((MethodCallExpression)Predicate.Body).Object).Member;

            internal static bool TryMatchAndGetId(Expression<Func<User, bool>> filter, out Guid id)
            {
                // default value in case we can’t obtain it 
                id = default(Guid);

                // lambda body should be a call 
                if (filter.Body.NodeType != ExpressionType.Call)
                {
                    return false;
                }

                // actually a call to object.Equals(object)
                var callExpression = (MethodCallExpression)filter.Body;
                if (callExpression.Method != EqualsMethodInfo)
                {
                    return false;
                }
                // left side of Equals() should be an access to User.Id
                if (callExpression.Object == null
                    || callExpression.Object.NodeType != ExpressionType.MemberAccess
                    || ((MemberExpression)callExpression.Object).Member != UserIdMemberInfo)
                {
                    return false;
                }

                // There should be only one argument for Equals()
                if (callExpression.Arguments.Count != 1)
                {
                    return false;
                }

                MemberExpression fieldAccess;
                if (callExpression.Arguments[0].NodeType == ExpressionType.Convert)
                {
                    // convert node should have an member access access node
                    // This is for cases when primary key is a value type
                    var convert = (UnaryExpression)callExpression.Arguments[0];
                    if (convert.Operand.NodeType != ExpressionType.MemberAccess)
                    {
                        return false;
                    }
                    fieldAccess = (MemberExpression)convert.Operand;
                }
                else if (callExpression.Arguments[0].NodeType == ExpressionType.MemberAccess)
                {
                    // Get field member for when key is reference type
                    fieldAccess = (MemberExpression)callExpression.Arguments[0];
                }
                else
                {
                    return false;
                }

                // and member access should be a field access to a variable captured in a closure
                if (fieldAccess.Member.MemberType != MemberTypes.Field
                    || fieldAccess.Expression.NodeType != ExpressionType.Constant)
                {
                    return false;
                }

                // expression tree matched so we can now just get the value of the id 
                var fieldInfo = (FieldInfo)fieldAccess.Member;
                var closure = ((ConstantExpression)fieldAccess.Expression).Value;

                id = (Guid)fieldInfo.GetValue(closure);
                return true;
            }
        }
    }
}
