using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Emails;
using FLS.Data.WebApi.Exceptions;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using NLog;

namespace FLS.Server.Service
{
    public class TemplateService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public TemplateService(DataAccessService dataAccessService, IdentityService identityService) 
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }
        
        #region EmailTemplate
        public List<EmailTemplateOverview> GetEmailTemplateOverviews()
        {
            var emailTemplates = GetEmailTemplates();

            var items = emailTemplates.Select(e => e.ToEmailTemplateOverview()).ToList();
            SetEmailTemplateOverviewSecurity(items);

            return items;
        }
        
        public EmailTemplateDetails GetEmailTemplateDetails(Guid emailTemplateId)
        {
            var emailTemplate = GetEmailTemplate(emailTemplateId);

            var emailTemplateDetails = emailTemplate.ToEmailTemplateDetails();
            SetEmailTemplateDetailsSecurity(emailTemplateDetails, emailTemplate);

            return emailTemplateDetails;
        }

        public EmailTemplateDetails GetEmailTemplateDetails(string templateKeyName, Guid clubId)
        {
            var emailTemplate = GetEmailTemplate(templateKeyName, clubId);

            var emailTemplateDetails = emailTemplate.ToEmailTemplateDetails();
            SetEmailTemplateDetailsSecurity(emailTemplateDetails, emailTemplate);

            return emailTemplateDetails;
        }
        
        internal List<EmailTemplate> GetEmailTemplates()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                if (IsCurrentUserInRoleSystemAdministrator)
                {
                    var items = context.EmailTemplates.Where(q => q.IsSystemTemplate || q.ClubId == null)
                        .OrderBy(a => a.EmailTemplateKeyName).ToList();
                    return items;
                }

                if (IsCurrentUserInRoleClubAdministrator)
                {
                    var clubTemplates =
                        context.EmailTemplates.Where(q => q.IsSystemTemplate == false 
                            && q.ClubId == CurrentAuthenticatedFLSUserClubId).ToList();

                    var systemTemplates = context.EmailTemplates.Where(q => q.IsSystemTemplate).ToList();
                    
                    foreach (var systemTemplate in systemTemplates.ToList())
                    {
                        if (clubTemplates.Any(e => e.EmailTemplateKeyName == systemTemplate.EmailTemplateKeyName) == false)
                        {
                            //no club template found, so we add the system template to the club collection
                            clubTemplates.Add(systemTemplate);
                        }
                    }

                    return clubTemplates.OrderBy(a => a.EmailTemplateKeyName).ToList();
                }
                
                return new List<EmailTemplate>();
            }
        }

        internal EmailTemplate GetEmailTemplate(Guid emailTemplateId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var emailTemplate = context.EmailTemplates
                    .FirstOrDefault(a => a.EmailTemplateId == emailTemplateId);

                return emailTemplate;
            }
        }

        internal EmailTemplate GetEmailTemplate(string templateKeyName, Guid? clubId = null)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var clubTemplate =
                        context.EmailTemplates.FirstOrDefault(q => q.IsSystemTemplate == false
                            && q.ClubId == clubId
                            && q.EmailTemplateKeyName.ToUpper() == templateKeyName.ToUpper());

                if (clubTemplate != null) return clubTemplate;

                var systemTemplate = context.EmailTemplates.FirstOrDefault(q => q.IsSystemTemplate
                    && q.EmailTemplateKeyName.ToUpper() == templateKeyName.ToUpper());

                return systemTemplate;
            }
        }

        public void InsertEmailTemplateDetails(EmailTemplateDetails emailTemplateDetails)
        {
            var emailTemplate = emailTemplateDetails.ToEmailTemplate();
            emailTemplate.NotNull("EmailTemplate");

            InsertEmailTemplate(emailTemplate);

            //Map it back to details
            emailTemplate.ToEmailTemplateDetails(emailTemplateDetails);
        }

        internal void InsertEmailTemplate(EmailTemplate emailTemplate)
        {
            emailTemplate.ArgumentNotNull("emailTemplate");

            if (IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException("User must be in role system-administrator");
            }

            emailTemplate.IsSystemTemplate = true;

            if (ExistsSystemEmailTemplateAlready(emailTemplate.EmailTemplateKeyName))
            {
                throw new DuplicateNameException($"Email-Template with Key-Name: {emailTemplate.EmailTemplateKeyName} already exists as System-Template");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.EmailTemplates.Add(emailTemplate);
                context.SaveChanges();
            }
        }

        private bool ExistsSystemEmailTemplateAlready(string emailTemplateKeyName)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var any = context.EmailTemplates
                    .Any(a => a.EmailTemplateKeyName.ToUpper() == emailTemplateKeyName.ToUpper()
                    && a.IsSystemTemplate);

                return any;
            }
        }

        public void UpdateEmailTemplateDetails(EmailTemplateDetails currentEmailTemplateDetails)
        {
            currentEmailTemplateDetails.ArgumentNotNull("currentEmailTemplateDetails");
            var original = GetEmailTemplate(currentEmailTemplateDetails.EmailTemplateId);
            original.EntityNotNull("EmailTemplate", currentEmailTemplateDetails.EmailTemplateId);

            if (IsCurrentUserInRoleClubAdministrator == false
                && IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException("User must be in role club- or system-administrator");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.EmailTemplates.Attach(original);

                if (original.IsSystemTemplate)
                {
                    if (IsCurrentUserInRoleSystemAdministrator)
                    {
                        //we are allowed to update a systemtemplate
                        currentEmailTemplateDetails.ToEmailTemplate(original);
                    }
                    else if (IsCurrentUserInRoleClubAdministrator)
                    {
                        //as it is a systemtemplate, we make a copy and create a club specific template
                        var newEmailTemplate = currentEmailTemplateDetails.ToEmailTemplate();
                        newEmailTemplate.ClubId = CurrentAuthenticatedFLSUserClubId;
                        newEmailTemplate.IsSystemTemplate = false;
                        newEmailTemplate.EmailTemplateKeyName = original.EmailTemplateKeyName; //make sure we have the same key name
                        context.EmailTemplates.Add(newEmailTemplate);
                    }
                }
                else
                {
                    currentEmailTemplateDetails.ToEmailTemplate(original);
                }


                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToEmailTemplateDetails(currentEmailTemplateDetails);
                }
            }
        }

        public void DeleteEmailTemplate(Guid emailTemplateId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.EmailTemplates.FirstOrDefault(l => l.EmailTemplateId == emailTemplateId);
                original.EntityNotNull("EmailTemplate", emailTemplateId);

                if (original.IsSystemTemplate)
                {
                    if (IsCurrentUserInRoleSystemAdministrator == false)
                    {
                        throw new UnauthorizedAccessException("User must be in role system-administrator");
                    }
                }
                else
                {
                    if (IsCurrentUserInRoleSystemAdministrator == false
                        && IsCurrentUserInRoleClubAdministrator == false)
                    {
                        throw new UnauthorizedAccessException("User must be in role club- or system-administrator");
                    }
                }

                context.EmailTemplates.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion EmailTemplate

        #region Security
        private void SetEmailTemplateOverviewSecurity(IEnumerable<EmailTemplateOverview> list)
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

            foreach (var emailTemplateOverview in list)
            {
                if (emailTemplateOverview.IsCustomizable &&
                    (IsCurrentUserInRoleClubAdministrator || IsCurrentUserInRoleSystemAdministrator))
                {
                    emailTemplateOverview.CanUpdateRecord = true;
                    emailTemplateOverview.CanDeleteRecord = true;
                }
                else
                {
                    emailTemplateOverview.CanUpdateRecord = false;
                    emailTemplateOverview.CanDeleteRecord = false;
                }
            }
        }

        private void SetEmailTemplateDetailsSecurity(EmailTemplateDetails details, EmailTemplate emailTemplate)
        {
            if (details == null)
            {
                Logger.Error(string.Format("EmailTemplateDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (emailTemplate.IsCustomizable
                && (IsCurrentUserInRoleClubAdministrator || IsCurrentUserInRoleSystemAdministrator))
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
    }
}
