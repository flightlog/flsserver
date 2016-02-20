using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Resources;
using FLS.Data.WebApi.User;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Tests.Extensions;
using Foundation.ObjectHydrator;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.Helpers
{
    public class UserHelper : BaseHelper
    {
        public UserHelper(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
        }

        public User GetUser(string username)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
                return user;
            }
        }

        public Role GetRole(string roleName)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var role = context.Roles.FirstOrDefault(u => u.RoleApplicationKeyString.ToLower() == roleName.ToLower());
                return role;
            }
        }

        public Guid GetClubIdOfUser(string username)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());

                if (user != null)
                {
                    return user.ClubId;
                }
            }

            throw new Exception("User not found");
        }

        public UserDetails CreateUserDetails(Guid clubId)
        {
            var hydrator = new Hydrator<UserDetails>();
            var userDetails = hydrator.GetSingle();
            userDetails.UserName = "User" + DateTime.Now.Ticks;
            userDetails.UserId = Guid.Empty;
            userDetails.ClubId = clubId;
            userDetails.PersonId = null;
            userDetails.UserRoleIds = new List<Guid>();
            userDetails.AccountState = (int)FLS.Data.WebApi.User.UserAccountState.Active;
            var role = GetRole(RoleApplicationKeyStrings.FlightOperator);
            Assert.IsNotNull(role);
            userDetails.UserRoleIds.Add(role.Id);
            return userDetails;
        }

        public UserRegistrationDetails CreateUserRegistrationDetails(Guid clubId)
        {
            var hydrator = new Hydrator<UserRegistrationDetails>();
            var userDetails = hydrator.GetSingle();
            userDetails.UserName = "User" + DateTime.Now.Ticks;
            userDetails.PersonId = null;
            userDetails.UserRoleIds = new List<Guid>();
            var role = GetRole(RoleApplicationKeyStrings.FlightOperator);
            Assert.IsNotNull(role);
            userDetails.UserRoleIds.Add(role.Id);
            userDetails.EmailConfirmationLink = "http://localhost/api/account/ConfirmEmail?userId={userid}&code={code}";
            return userDetails;
        }

        public UserDetails CreateTestClubAdminUserDetails(Guid clubId)
        {
            var userDetails = new UserDetails()
            {
                ClubId = clubId,
                UserRoleIds = new List<Guid>(),
                NotificationEmail = "test@glider-fls.ch",
                FriendlyName = "TestClub-Admin-User",
                UserName = "TestClub-Admin-User",
                Remarks = "TestClub-Admin"
            };

            var role = GetRole(RoleApplicationKeyStrings.ClubAdministrator);
            Assert.IsNotNull(role);
            userDetails.UserRoleIds.Add(role.Id);

            return userDetails;
        }

        public User CreateNewUserInDb(Guid clubId, string plainPassword, bool emailConfirmed = false)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                if (DataAccessService.IdentityService != null &&
                    DataAccessService.IdentityService.CurrentAuthenticatedFLSUser == null)
                {
                    DataAccessService.IdentityService.SetUser(IdentityService.CurrentAuthenticatedFLSUser);
                }

                var hydrator = new Hydrator<User>()
                    .Ignoring(x => x.Person)
                    .Ignoring(x => x.UserAccountState)
                    .Ignoring(x => x.Club)
                    .Ignoring(x => x.UserRoles);
                var user = hydrator.GetSingle();
                user.RemoveMetadataInfo();

                user.ClubId = clubId;
                user.AccountState = (int) FLS.Data.WebApi.User.UserAccountState.Active;
                user.EmailConfirmed = emailConfirmed;
                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(plainPassword);
                user.UserName = "User" + DateTime.Now.Ticks;
                user.PasswordHash = hashedPassword;
                user.LastPasswordChangeOn = DateTime.UtcNow;
                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
        }

        public void SetUsersPassword(Guid userId, string plainPassword)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher();
                    var hashedPassword = passwordHasher.HashPassword(plainPassword);
                    user.PasswordHash = hashedPassword;
                    user.LastPasswordChangeOn = DateTime.UtcNow;

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
        }
    }
}
