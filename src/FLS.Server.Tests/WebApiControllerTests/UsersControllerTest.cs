using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Resources;
using FLS.Data.WebApi.User;
using FLS.Server.Service.Email;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure.WebApi;
using FLS.Server.Tests.Mocks.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class UsersControllerTest : BaseAuthenticatedTests
    {
        private UserHelper _userHelper;
        
        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize: UsersControllerTest.TestInitialize()");
            _userHelper = UnityContainer.Resolve<UserHelper>();
            _userHelper.SetUser(TestConfigurationSettings.Instance.TestClubAdminUsername);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetUsersOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<UserOverview>>(RoutePrefix).Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetUserDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<UserOverview>>(RoutePrefix).Result;

            Assert.IsTrue(response.Any());

            var id = response.First().UserId;

            var result = GetAsync<UserDetails>(RoutePrefix + "/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertUserDetailsWebApiTest()
        {
            var userRegistrationDetails = _userHelper.CreateUserRegistrationDetails(ClubId);
            userRegistrationDetails.UserName = "HalloUser";
            var response = PostAsync(userRegistrationDetails, "/api/v1/users").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<UserDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            
            
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateUserDetailsWebApiTest()
        {
            //insert new user with all roles
            var userRegistrationDetails = _userHelper.CreateUserRegistrationDetails(ClubId);
            userRegistrationDetails.UserRoleIds.Clear();
            var role = _userHelper.GetRole(RoleApplicationKeyStrings.ClubAdministrator);
            userRegistrationDetails.UserRoleIds.Add(role.Id);
            role = _userHelper.GetRole(RoleApplicationKeyStrings.SystemAdministrator);
            userRegistrationDetails.UserRoleIds.Add(role.Id);
            role = _userHelper.GetRole(RoleApplicationKeyStrings.FlightOperator);
            userRegistrationDetails.UserRoleIds.Add(role.Id);
            var response = PostAsync(userRegistrationDetails, "/api/v1/users").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<UserDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.IsTrue(responseDetails.UserRoleIds.Count == 3);

            //update user details
            responseDetails.UserRoleIds.Clear();
            responseDetails.UserRoleIds.Add(role.Id); // flight operator
            response = PutAsync(responseDetails, "/api/v1/users/" + responseDetails.Id).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            responseDetails = ConvertToModel<UserDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            Assert.IsTrue(responseDetails.UserRoleIds.Count == 1);
            Assert.IsTrue(responseDetails.UserRoleIds.First() == role.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void ChangeUserPasswordWebApiTest()
        {
            var firstPassword = "Test1234";
            var user = _userHelper.CreateNewUserInDb(ClubId, firstPassword);

            Assert.IsNotNull(user, "User is null");
            var myUserDetails = GetAsync<UserDetails>("/api/v1/users/" + user.Id).Result;

            var passwordChange = new PasswordChangeRequest
                {
                    UserId = myUserDetails.UserId,
                    OldPassword = firstPassword,
                    NewPassword = "Test1234567890"
                };

            var putResult = PutAsync(passwordChange, "/api/v1/users/changepassword").Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            var updatedUser = _userHelper.GetUser(user.UserName);
            Assert.AreNotEqual(user.LastPasswordChangeOn, updatedUser.LastPasswordChangeOn, "LastPasswordChange values invalid");
            Assert.AreNotEqual(user.SecurityStamp, updatedUser.SecurityStamp, "SecurityStamp has not been updated");
            Assert.AreNotEqual(user.PasswordHash, updatedUser.PasswordHash, "Password was not updated correctly");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void ResetLostUserPasswordWebApiTest()
        {
            var firstPassword = "Test1234";
            var user = _userHelper.CreateNewUserInDb(ClubId, firstPassword, emailConfirmed:true);

            Assert.IsNotNull(user, "User is null");
            var newUserDetails = GetAsync<UserDetails>("/api/v1/users/" + user.Id).Result;

            var passwordResetRequest = new LostPasswordRequest()
            {
                UsernameOrNotificationEmailAddress = newUserDetails.UserName,
                PasswordResetLink = TestServer.BaseAddress + "api/v1/users/resetpassword/?userid={userid}&code={code}"
            };

            Logout();
            var passwordResetResult = PostAsync(passwordResetRequest, "/api/v1/users/lostpassword").Result;

            Assert.IsTrue(passwordResetResult.IsSuccessStatusCode);
            
        }

        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void UserLockoutWebApiTest()
        {
            var userDetails = _userHelper.CreateUserDetails(ClubId);
            
            var response = PostAsync(userDetails, "/api/v1/users").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<UserDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            Logout();

            Login(userDetails.UserName, "WrongPassword", true);

            LoginAsSystemAdmin();

            var updatedUserDetails = GetAsync<UserDetails>("/api/v1/users/" + responseDetails.Id).Result;
            Assert.IsNotNull(updatedUserDetails);
        }
        
        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/users"; }
        }
    }
}
