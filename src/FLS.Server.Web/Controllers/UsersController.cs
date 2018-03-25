using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi.Resources;
using FLS.Data.WebApi.User;
using FLS.Server.Data.Mapping;
using FLS.Server.Service;
using FLS.Server.WebApi.Identity;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Resources;
using FLS.Server.Service.Email;
using NLog;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for user entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private readonly UserService _userService;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IdentityRoleManager _identityRoleManager;
        private readonly IdentityService _identityService;
        private readonly UserAccountEmailBuildService _userAccountEmailBuildService;

        private Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        public UsersController(UserService userService, IdentityUserManager identityUserManager, 
            IdentityRoleManager identityRoleManager, IdentityService identityService,
            UserAccountEmailBuildService userAccountEmailBuildService)
        {
            _userService = userService;
            _identityUserManager = identityUserManager;
            _identityRoleManager = identityRoleManager;
            _identityService = identityService;
            _userAccountEmailBuildService = userAccountEmailBuildService;
        }

        /// <summary>
        /// Gets the user overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("overview")]
        [ResponseType(typeof(List<UserOverview>))]
        public IHttpActionResult GetUserOverviews()
        {
            var users = _userService.GetUserOverviews();
            return Ok(users);
        }

        /// <summary>
        /// Gets my club user overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("club")]
        [Route("club/overview")]
        [Route("overview/club")]
        [ResponseType(typeof(List<UserOverview>))]
        public IHttpActionResult GetMyClubUserOverviews()
        {
            var users = _userService.GetMyClubUserOverviews();
            return Ok(users);
        }

        /// <summary>
        /// Gets the user overviews.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("club/{clubId}")]
        [Route("club/overview/{clubId}")]
        [ResponseType(typeof(List<UserOverview>))]
        public IHttpActionResult GetUserOverviews(Guid clubId)
        {
            var users = _userService.GetUserOverviews(clubId);
            return Ok(users);
        }

        /// <summary>
        /// Gets the user overviews.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<UserOverview>))]
        public IHttpActionResult GetPagedUserOverview([FromBody]PageableSearchFilter<UserOverviewSearchFilter> pageableSearchFilter, int? pageStart = 0, int? pageSize = 100)
        {
            var users = _userService.GetPagedUserOverview(pageStart, pageSize, pageableSearchFilter);
            return Ok(users);
        }

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult GetUserDetails(Guid userId)
        {
            var userDetails = _userService.GetUserDetails(userId);
            return Ok(userDetails);
        }

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("name/{username}")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult GetUserDetails(string username)
        {
            var userDetails = _userService.GetUserDetails(username);
            return Ok(userDetails);
        }

        /// <summary>
        /// Gets my user details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult GetMyUserDetails()
        {
            var userDetails = _userService.GetMyUserDetails();
            return Ok(userDetails);
        }

        /// <summary>
        /// Inserts the user details.
        /// </summary>
        /// <param name="userRegistrationDetails">The user details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult InsertUserDetails([FromBody] UserRegistrationDetails userRegistrationDetails)
        {
            //TODO: Handle security which was in the service layer
            //    //check if the current user has system admin rights or is a club admin. If Club admin, 
            //    //the new user must be in the same club otherwise an exception is thrown
            //    if (IsCurrentUserInRoleClubAdministrator &&
            //        IsCurrentUserInClub(userDetails.ClubId) == false)
            //    {
            //        throw new UnauthorizedAccessException();
            //    }

            var clubId = _identityService.CurrentAuthenticatedFLSUser.ClubId;
            var user = userRegistrationDetails.ToUser(clubId, UserAccountState.Active);
            user.UserRoles.Clear();
            var result = _identityUserManager.CreateAsync(user).Result;

            if (result.Succeeded == false)
            {
                var errors = string.Join(Environment.NewLine, result.Errors);
                throw new ApplicationException($"Error while trying to create new user. Error: {errors}");
            }

            if (userRegistrationDetails.UserRoleIds.Any())
            {
                var rolesToAdd = new List<string>();
                var roles = _identityRoleManager.Roles.ToList();

                foreach (var userRoleId in userRegistrationDetails.UserRoleIds)
                {
                    var role = roles.First(x => x.RoleId == userRoleId);
                    rolesToAdd.Add(role.RoleApplicationKeyString);
                }

                var addResult = _identityUserManager.AddToRolesAsync(user.UserId, rolesToAdd.ToArray()).Result;
            }

            if (_identityUserManager.UserTokenProvider != null)
            {
                string code = _identityUserManager.GenerateEmailConfirmationTokenAsync(user.Id).Result;

                //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new {userId = user.Id, code = code}));
                var encodedCode = code.ToBase64();

                _logger.Debug($"EmailConfirmation-Token: Code: {code}, Base64: {encodedCode}");

                var callbackUrl =
                    new Uri(
                        userRegistrationDetails.EmailConfirmationLink.ToLower()
                        .Replace("{userid}", user.UserId.ToString())
                            .Replace("{code}", encodedCode));

                var message =_userAccountEmailBuildService.CreateEmailConfirmationEmail(user, callbackUrl.ToString());

                var sendResult = _identityUserManager.SendEmailAsync(user.Id, message.Subject, message.Body);
            }

            var userDetails = _userService.GetUserDetails(user.UserId);
            return Ok(userDetails);
        }

        /// <summary>
        /// Confirms the users email address and sends password reset token by email.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        [ResponseType(typeof(EmailConfirmationResult))]
        public IHttpActionResult ConfirmEmail(Guid userId, string code = "")
        {
            if (userId.IsValid() == false || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var decodedCode = code.Base64ToString();

            _logger.Debug($"EmailConfirmation-Validation: Base64-Code: {code}, Decoded: {decodedCode}");

            var result = _identityUserManager.ConfirmEmailAsync(userId, decodedCode).Result;

            if (result.Succeeded)
            {
                if (_identityUserManager.HasPasswordAsync(userId).Result == false)
                {
                    var passwordResetToken = _identityUserManager.GeneratePasswordResetTokenAsync(userId).Result;
                    var base64PasswordResetToken = passwordResetToken.ToBase64();
                    _logger.Debug($"Password-Reset-Token: Code: {passwordResetToken}, Base64: {base64PasswordResetToken}");

                    //generate and send password confirmation
                    var emailConfirmationResult = new EmailConfirmationResult
                    {
                        UserId = userId,
                        PasswordResetCode = base64PasswordResetToken
                    };

                    return Ok(emailConfirmationResult);
                }
                
            }

            var errors = string.Join(Environment.NewLine, result.Errors);

            throw new ApplicationException(errors);
        }

        /// <summary>
        /// Updates the user details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userDetails">The user details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult UpdateUserDetails(Guid userId, [FromBody]UserDetails userDetails)
        {
            var original = _identityUserManager.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole).First(u => u.UserId == userId);
            
            var roles = _identityRoleManager.Roles.ToList();
            var rolesToRemove = new List<string>();
            //remove no longer set UserRoles
            foreach (var userRole in original.UserRoles.ToList())
            {
                if (userRole.RoleId == Guid.Empty || userRole.UserId == Guid.Empty) continue;

                var found = userDetails.UserRoleIds.Any(userRoleId => userRole.RoleId == userRoleId);

                if (found == false)
                {
                    rolesToRemove.Add(roles.First(q => q.RoleId == userRole.RoleId).RoleApplicationKeyString);

                }
            }

            if (rolesToRemove.Any())
            {
                var removeResult = _identityUserManager.RemoveFromRolesAsync(userId, rolesToRemove.ToArray()).Result;
            }

            var rolesToAdd = new List<string>();
            //add UserRoles
            foreach (var userRoleId in userDetails.UserRoleIds)
            {
                var found = original.UserRoles.Any(userRole => userRole.RoleId == userRoleId);

                if (found == false)
                {
                    rolesToAdd.Add(roles.First(q => q.RoleId == userRoleId).RoleApplicationKeyString);
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = _identityUserManager.AddToRolesAsync(userId, rolesToAdd.ToArray()).Result;
            }

            //TODO: Check if email address has changed, if so set email confirmation flag to false and resend email confirmation
            userDetails.ToUser(original);
            var result = _identityUserManager.UpdateAsync(original).Result;

            if (result.Succeeded)
            {
                var updatedUser = _identityUserManager.Users.Include(Constants.UserRoles).Include(Constants.UserRolesRole).First(u => u.UserId == userId);
                return Ok(updatedUser.ToUserDetails());
            }

            var errors = string.Join(Environment.NewLine, result.Errors);

            throw new ApplicationException(errors);
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
        [HttpDelete]
        [Route("{userId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid userId)
        {
            var user = _identityUserManager.FindByIdAsync(userId).Result;

            user.NotNull();

            var result = _identityUserManager.DeleteAsync(user).Result;

            if (result.Succeeded)
            {
                return Ok();
            }

            var errors = string.Join(Environment.NewLine, result.Errors);

            throw new ApplicationException(errors);
        }

        /// <summary>
        /// Changes the users password.
        /// </summary>
        /// <param name="passwordChangeRequest">The password change request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("changepassword", Name="ChangeUsersPassword")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ChangeUsersPassword([FromBody]PasswordChangeRequest passwordChangeRequest)
        {
            var pwdChange = _identityUserManager.ChangePasswordAsync(passwordChangeRequest.UserId, passwordChangeRequest.OldPassword,
                passwordChangeRequest.NewPassword).Result;

            if (pwdChange.Succeeded) return Ok();

            var errors = string.Join(Environment.NewLine, pwdChange.Errors);

            _logger.Info($"Password could not be changed due to following errors: {errors}");

            throw new BadRequestException(errors);
        }

        /// <summary>
        /// Changes the users password.
        /// </summary>
        /// <param name="passwordResetRequest">The PasswordResetRequest which includes username or notification email address as search term.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("lostpassword")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ResetLostUserPassword([FromBody]LostPasswordRequest passwordResetRequest)
        {
            //_identityUserManager.ResetPasswordAsync()
            var user = _identityUserManager.FindByNameAsync(passwordResetRequest.UsernameOrNotificationEmailAddress).Result;

            if (user == null && passwordResetRequest.SearchForUsernameOnly == false)
            {
                user =
                    _identityUserManager.FindByEmailAsync(passwordResetRequest.UsernameOrNotificationEmailAddress)
                        .Result;
            }

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new BadRequestException("User not found");
            }

            if (_identityUserManager.IsEmailConfirmedAsync(user.Id).Result == false)
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new BadRequestException("Email address is not confirmed");
            }

            string code = _identityUserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
            //var callbackUrl = new Uri(Url.Link("ChangeUsersPassword", new { userId = user.Id, code = code }));
            var encodedCode = code.ToBase64();

            _logger.Debug($"Password reset token: Code: {code}, Base64: {encodedCode}");

            var callbackUrl =
                    new Uri(
                        passwordResetRequest.PasswordResetLink.ToLower()
                        .Replace("{userid}", user.UserId.ToString())
                            .Replace("{code}", encodedCode));

            var message = _userAccountEmailBuildService.CreateLostPasswordResetEmail(user, callbackUrl.ToString());

            await _identityUserManager.SendEmailAsync(user.Id, message.Subject, message.Body);

            return Ok();
        }

        /// <summary>
        /// Resets the users password.
        /// </summary>
        /// <param name="passwordResetRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("resetpassword")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ResetPassword([FromBody]PasswordResetRequest passwordResetRequest)
        {
            var decodedCode = passwordResetRequest.PasswordResetCode.Base64ToString();
            _logger.Debug($"Password-Reset-Validation: Base64-Code: {passwordResetRequest.PasswordResetCode}, Decoded: {decodedCode}");
            var result = _identityUserManager.ResetPasswordAsync(passwordResetRequest.UserId, decodedCode,
                passwordResetRequest.NewPassword).Result;

            if (result.Succeeded == false)
            {
                var errors = string.Join(Environment.NewLine, result.Errors);

                throw new ApplicationException(errors);
            }

            var accessFailedCountResult = _identityUserManager.ResetAccessFailedCountAsync(passwordResetRequest.UserId).Result;

            if (accessFailedCountResult.Succeeded)
            {
                return Ok();
            }

            var accessFailedCountResultErrors = string.Join(Environment.NewLine, accessFailedCountResult.Errors);

            throw new ApplicationException(accessFailedCountResultErrors);
        }

        /// <summary>
        /// Resend email confirmation link
        /// </summary>
        /// <param name="emailTokenResendRequest">The user details.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleApplicationKeyStrings.ClubAdministrator + "," + RoleApplicationKeyStrings.SystemAdministrator)]
        [HttpPost]
        [Route("resendemailtoken")]
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult ResendEmailToken([FromBody] EmailTokenResendRequest emailTokenResendRequest)
        {
            emailTokenResendRequest.ArgumentNotNull("emailTokenResendRequest");

            var user = _identityUserManager.FindByNameAsync(emailTokenResendRequest.UserName).Result;
            
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new BadRequestException("User not found");
            }

            if (user.UserId != emailTokenResendRequest.UserId)
            {
                throw new BadRequestException("UserId does not match");
            }

            if (user.EmailConfirmed)
            {
                throw new BadRequestException("User has already confirmed email address!");
            }

            if (_identityUserManager.UserTokenProvider != null)
            {
                string code = _identityUserManager.GenerateEmailConfirmationTokenAsync(user.Id).Result;

                //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new {userId = user.Id, code = code}));
                var encodedCode = code.ToBase64();

                _logger.Debug($"EmailConfirmation-Token: Code: {code}, Base64: {encodedCode}");

                var callbackUrl =
                    new Uri(
                        emailTokenResendRequest.EmailConfirmationLink.ToLower()
                        .Replace("{userid}", user.UserId.ToString())
                            .Replace("{code}", encodedCode));

                var message = _userAccountEmailBuildService.CreateEmailConfirmationEmail(user, callbackUrl.ToString());

                var sendResult = _identityUserManager.SendEmailAsync(user.Id, message.Subject, message.Body);
            }

            var userDetails = _userService.GetUserDetails(user.UserId);
            return Ok(userDetails);
        }
    }
}
