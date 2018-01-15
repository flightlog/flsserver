using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Exceptions;
using FLS.Data.WebApi.System;
using FLS.Data.WebApi.User;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Service.Email;
using Microsoft.AspNet.Identity;
using NLog;
using Constants = FLS.Server.Data.Resources.Constants;

namespace FLS.Server.Service
{
    public class UserService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IIdentityService _identityService;
        private readonly UserAccountEmailBuildService _passwordEmailService;

        public UserService(DataAccessService dataAccessService, IIdentityService identityService, UserAccountEmailBuildService passwordEmailService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _identityService = identityService;
            _passwordEmailService = passwordEmailService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region UserAccountState
        public List<UserAccountStateListItem> GetUserAccountStateListItems()
        {
            var entities = GetUserAccountStates();

            var items = entities.Select(u => u.ToUserAccountStateListItem()).ToList();

            return items;
        }
        
        internal List<Data.DbEntities.UserAccountState> GetUserAccountStates()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var userAccountStates = context.UserAccountStates
                    .OrderBy(a => a.UserAccountStateName)
                    .ToList();

                return userAccountStates;
            }
        }
        #endregion UserAccountState

        #region Roles
        /// <summary>
        /// Get the roles ordered by roleApplicationKeyString.
        /// </summary>
        /// <returns></returns>
        public List<RoleOverview> GetRoleOverviews()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var roles = context.Roles.OrderBy(r => r.RoleApplicationKeyString).ToList();

                var items = roles.Select(r => r.ToRoleOverview()).ToList();

                return items;
            }
        }
        
        internal Role GetRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return null;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                Logger.Debug(
                    string.Format(
                        "Requested Role with name: {0} and got role {1} from database.",
                        roleName, role));

                return role;
            }
        }

        internal void InsertRole(Role role)
        {
            role.ArgumentNotNull("role");
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                if (context.Roles.Any(r => r.RoleName.ToLower() == role.RoleName.ToLower()))
                {
                    //role with same name exists already
                    throw new DuplicateNameException("Role with same name exists already");
                }

                context.Roles.Add(role);
                context.SaveChanges();
                Logger.Debug(string.Format("Insert Role: {0} into database", role));
            }
        }

        internal void UpdateRole(Role currentRole)
        {
            currentRole.ArgumentNotNull("currentRole");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Roles.FirstOrDefault(l => l.RoleId == currentRole.RoleId);
                original.EntityNotNull("Role", currentRole.RoleId);

                currentRole.ToRole(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }
        }

        public void DeleteRole(Guid roleId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Roles.FirstOrDefault(l => l.RoleId == roleId);
                original.EntityNotNull("Role", roleId);

                context.Roles.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Roles

        #region User
        public List<UserOverview> GetUserOverviews()
        {
            List<User> users = null;

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                users = GetUsers();
            }
            else if (IsCurrentUserInRoleClubAdministrator)
            {
                users = GetUsersOfClub(CurrentAuthenticatedFLSUserClubId);
            }

            return PrepareUserOverviews(users);
        }

        public PagedList<UserOverview> GetPagedUserOverview(int? pageStart, int? pageSize, PageableSearchFilter<UserOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<UserOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new UserOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("UserName", "asc");
            }

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "PersonName")
                {
                    pageableSearchFilter.Sorting.Add("Person.Lastname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Add("Person.Firstname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "ClubName")
                {
                    pageableSearchFilter.Sorting.Add("Club.Clubname", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "AccountState")
                {
                    pageableSearchFilter.Sorting.Add("UserAccountState.UserAccountStateName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "UserRoles")
                {
                    //TODO: Add ability to sort for user roles
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("UserName", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var users = context.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole)
                    .Include(Constants.Club).Include(Constants.Person)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                if (IsCurrentUserInRoleSystemAdministrator)
                {
                    //we don't have to filter for club related users, return all users
                }
                else if (IsCurrentUserInRoleClubAdministrator)
                {
                    users = users.Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);
                }
                else
                {
                    throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
                }
                
                var filter = pageableSearchFilter.SearchFilter;
                users = users.WhereIf(filter.UserName,
                        user => user.UserName.Contains(filter.UserName));
                users = users.WhereIf(filter.AccountState,
                        user => user.UserAccountState.UserAccountStateName.Contains(filter.AccountState));
                users = users.WhereIf(filter.ClubName,
                        user => user.Club.Clubname.Contains(filter.ClubName));
                users = users.WhereIf(filter.FriendlyName,
                        user => user.FriendlyName.Contains(filter.FriendlyName));
                users = users.WhereIf(filter.NotificationEmail,
                        user => user.NotificationEmail.Contains(filter.NotificationEmail));
                users = users.WhereIf(filter.PersonName,
                        user => user.Person.Lastname.Contains(filter.PersonName)
                            || user.Person.Firstname.Contains(filter.PersonName));
                users = users.WhereIf(filter.UserRoles,
                        user => user.UserRoles.Any(x => x.Role.RoleName.Contains(filter.UserRoles)));

                var pagedQuery = new PagedQuery<User>(users, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(x => x.ToUserOverview())
                .Where(obj => obj != null)
                .ToList();

                var pagedList = new PagedList<UserOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public List<UserOverview> GetMyClubUserOverviews()
        {
            List<User> users = GetUsersOfClub(CurrentAuthenticatedFLSUserClubId);
            
            return PrepareUserOverviews(users);
        }

        public List<UserOverview> GetUserOverviews(Guid clubId)
        {
            List<User> users = GetUsersOfClub(clubId);

            return PrepareUserOverviews(users);
        }

        private List<UserOverview> PrepareUserOverviews(List<User> users)
        {
            var userOverviewList = users.Select(e => e.ToUserOverview()).ToList();
            SetUserOverviewSecurity(userOverviewList);

            return userOverviewList.ToList();
        }

        public UserDetails GetUserDetails(Guid userId)
        {
            var user = GetUser(userId);

            var userDetails = user.ToUserDetails();
            SetUserDetailsSecurity(userDetails, user);

            return userDetails;
        }

        public UserDetails GetUserDetails(string username)
        {
            var user = GetUser(username);

            var userDetails = user.ToUserDetails();
            SetUserDetailsSecurity(userDetails, user);

            return userDetails;
        }

        public UserDetails GetMyUserDetails()
        {
            var user = GetUser(_identityService.CurrentAuthenticatedFLSUser.UserId);

            var userDetails = user.ToUserDetails();
            SetUserDetailsSecurity(userDetails, user);

            return userDetails;
        }

        internal List<User> GetUsersOfClub(Guid clubId)
        {
            if (IsCurrentUserInRoleSystemAdministrator ||
                (IsCurrentUserInRoleClubAdministrator && IsCurrentUserInClub(clubId)))
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var users = context.Users
                        .Include(Constants.Club)
                        .Include(Constants.UserRoles)
                        .Include(Constants.UserRolesRole)
                        .Include(Constants.Person)
                                       .Where(u => u.ClubId == CurrentAuthenticatedFLSUser.ClubId)
                                       .OrderBy(u => u.UserName)
                                       .ToList();

                    return users;
                }
            }
            else
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }
        }


        /// <summary>
        /// Gets all users.
        /// <remarks>Is also needed to verify user authentication, so we have to get all users regardless of club.</remarks>
        /// </summary>
        /// <returns></returns>
        internal List<User> GetUsers()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var users = context.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole)
                    .Include(Constants.Club).Include(Constants.Person)
                    .OrderBy(u => u.UserName)
                    .ToList();

                return users;
            }
        }

        internal User GetUser(Guid userId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var user = context.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole)
                    .Include(Constants.Club).Include(Constants.Person)
                    .FirstOrDefault(a => a.UserId == userId);

                return user;
            }
        }

        internal User GetUser(string username, bool includeNotificationEmailToSearchTerm = false)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var user = context.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole)
                    .Include(Constants.Club).Include(Constants.Person)
                    .FirstOrDefault(u => u.UserName.ToLower() == username.ToLower()
                        || (includeNotificationEmailToSearchTerm && u.NotificationEmail.ToLower() == username.ToLower()));
                
                return user;
            }
        }
        
        public void UpdateUserPassword(User user, string passwordHash)
        {
            var original = GetUser(user.UserId);
            original.EntityNotNull("User");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Users.Attach(original);
                original.PasswordHash = passwordHash;
                original.LastPasswordChangeOn = DateTime.UtcNow;
                original.DoNotUpdateMetaData = true;
                
                context.SaveChanges();
            }
        }
        #endregion User

        #region Security
        private void SetUserOverviewSecurity(IEnumerable<UserOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var userOverview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator ||
                        context.Users.First(a => a.UserId == userOverview.UserId).OwnerId ==
                        CurrentAuthenticatedFLSUser.UserId)
                    {
                        userOverview.CanUpdateRecord = true;
                        userOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        userOverview.CanUpdateRecord = false;
                        userOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetUserDetailsSecurity(UserDetails details, User User)
        {
            if (details == null)
            {
                Logger.Error(string.Format("UserDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || User.OwnerId == CurrentAuthenticatedFLSUser.UserId)
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }
        #endregion Security

        public void AddUserToRole(User user, string roleName)
        {
            user.ArgumentNotNull("user");

            if (user.UserRoles.Any(userRole => userRole.Role.RoleApplicationKeyString == roleName))
            {
                Logger.Info(string.Format("User {0} is already in role {1}", user.UserName, roleName));
                return;
            }

            var role = GetRole(roleName);

            if (role == null)
            {
                Logger.Error(string.Format("Role {0} not found. Can not add user {1} to role {0}", roleName,
                                           user.UserName));
                return;
            }

            var newUserRole = new UserRole();
            newUserRole.User = user;
            newUserRole.Role = role;
            user.UserRoles.Add(newUserRole);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.UserRoles.Add(newUserRole);
                context.SaveChanges();
            }
        }

        public void RemoveUserFromRole(User user, string roleName)
        {
            user.ArgumentNotNull("user");

            var userRole = user.UserRoles.FirstOrDefault(r => r.Role.RoleApplicationKeyString == roleName);

            if (userRole == null)
            {
                Logger.Error(string.Format("User {1} is not in Role {0}. Can not remove role from user.", roleName,
                                           user.UserName));
                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                if (context.UserRoles.Contains(userRole))
                {
                    context.UserRoles.Remove(userRole);
                    context.SaveChanges();
                }
            }
        }

        public void ChangeUsersPassword(PasswordChangeRequest passwordChange)
        {
            passwordChange.ArgumentNotNull("passwordChange");
            var original = GetUser(passwordChange.UserId);
            original.EntityNotNull("User", passwordChange.UserId);

            if (IsCurrentUserInRoleSystemAdministrator
                || (IsCurrentUserInRoleClubAdministrator && IsCurrentUserInClub(original.ClubId))
                || CurrentAuthenticatedFLSUser.UserId == original.UserId)
            {
                var passwordHasher = new PasswordHasher();

                if (passwordHasher.VerifyHashedPassword(original.PasswordHash, passwordChange.OldPassword) ==
                    PasswordVerificationResult.Failed)
                {
                    throw new FLSServerException("Old password is incorrect!");
                }

                //detect password changes
                if (string.IsNullOrEmpty(passwordChange.NewPassword) == false)
                {
                    var hashedPassword = passwordHasher.HashPassword(passwordChange.NewPassword);
                    UpdateUserPassword(original, hashedPassword);
                }
                else
                {
                    throw new FLSServerException("New password is empty or does not match!");
                }
            }
            else
            {
                Logger.Warn("User is not authorized for updating user details");
                throw new UnauthorizedAccessException();
            }
        }
    }
}
