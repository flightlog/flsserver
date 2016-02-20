using System;
using System.Linq;
using FLS.Data.WebApi.User;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using Foundation.ObjectHydrator;
using Microsoft.AspNet.Identity;

namespace FLS.Server.TestInfrastructure
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
            userDetails.UserId = Guid.Empty;
            userDetails.ClubId = clubId;
            userDetails.PersonId = null;

            return userDetails;
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
                    user.Password = hashedPassword;
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
