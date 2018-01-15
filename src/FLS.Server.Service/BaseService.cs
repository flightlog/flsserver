using System;
using System.Linq;
using System.Security.Authentication;
using FLS.Common.Validators;
using FLS.Data.WebApi.Resources;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using NLog;
using FLS.Server.Data.Resources;

namespace FLS.Server.Service
{
    public abstract class BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IIdentityService _identityService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public IIdentityService IdentityService 
        {
            get { return _identityService; }
        }

        protected BaseService(DataAccessService dataAccessService, IIdentityService identityService)
        {
            _dataAccessService = dataAccessService;
            _identityService = identityService;
        }

        protected User CurrentAuthenticatedFLSUser
        {
            get { return _identityService.CurrentAuthenticatedFLSUser; }
        }

        protected Guid CurrentAuthenticatedFLSUserClubId
        {
            get
            {
                if (CurrentAuthenticatedFLSUser == null)
                {
                    throw new AuthenticationException(ErrorMessage.UserNotAuthenticated);
                }

                return CurrentAuthenticatedFLSUser.ClubId;
            }
        }

        protected bool IsCurrentUserInClub(Guid clubId)
        {
            return CurrentAuthenticatedFLSUser != null && CurrentAuthenticatedFLSUser.ClubId == clubId;
        }

        protected bool IsOwnerInClub(Guid ownerId, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == ownerId);

                if (user == null) return false;

                return user.ClubId == clubId;
            }
        }

        protected bool IsOwner(IOwnershipMetaData ownershipMetaData)
        {
            ownershipMetaData.ArgumentNotNull("ownershipMetaData");

            if ((ownershipMetaData.OwnershipType == (int)OwnershipType.Club 
                && ownershipMetaData.OwnerId == CurrentAuthenticatedFLSUserClubId)
                || (ownershipMetaData.OwnershipType == (int)OwnershipType.User 
                && ownershipMetaData.OwnerId == CurrentAuthenticatedFLSUser.UserId))
            {
                return true;
            }

            return false;
        }

        protected bool IsCurrentUserInRoleClubAdministrator
        {
            get
            {
                return CurrentAuthenticatedFLSUser != null &&
                       CurrentAuthenticatedFLSUser.UserRoles.Any(
                           r => r.Role.RoleApplicationKeyString == RoleApplicationKeyStrings.ClubAdministrator);
            }
        }

        protected bool IsCurrentUserInRoleSystemAdministrator
        {
            get
            {
                return CurrentAuthenticatedFLSUser != null &&
                       CurrentAuthenticatedFLSUser.UserRoles.Any(
                           r => r.Role.RoleApplicationKeyString == RoleApplicationKeyStrings.SystemAdministrator);
            }
        }
    }
}
