using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using NLog;
using System;
using System.CodeDom;
using FLS.Server.Data.Mapping;
using FLS.Data.WebApi.Settings;
using FLS.Data.WebApi;
using System.Collections.Generic;
using FLS.Common.Exceptions;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Server.Data.Resources;
using Newtonsoft.Json;

namespace FLS.Server.Service
{
    public class SettingService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public SettingService(DataAccessService dataAccessService, IdentityService identityService) 
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public T GetSettingValue<T>(string key, string clubKey)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.Settings.FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower()
                    && x.Club.ClubKey.ToUpper() == clubKey.ToUpper());

                if (record == null)
                {
                    throw new EntityNotFoundException("Setting", key);
                }

                if (typeof(T) != typeof(string))
                {
                    T value = JsonConvert.DeserializeObject<T>(record.SettingValue);
                    return value;
                }

                return (T) Convert.ChangeType(record.SettingValue, typeof(T));
            }
        }

        public string GetSettingValue(string key, string clubKey)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.Settings.FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower()
                    && x.Club.ClubKey.ToUpper() == clubKey.ToUpper());

                if (record == null)
                {
                    throw new EntityNotFoundException("Setting", key);
                }

                return record.SettingValue;
            }
        }

        public string GetSettingValue(string key, Guid? clubId, Guid? userId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.Settings
                    .WhereIf(clubId.HasValue, x => x.ClubId == clubId)
                    .WhereIf(userId.HasValue, x => x.UserId == userId)
                    .FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower());

                if (record == null)
                {
                    throw new EntityNotFoundException("Setting", key);
                }

                return record.SettingValue;
            }
        }

        public PagedList<SettingDetails> GetPagedSettingDetails(int? pageStart, int? pageSize, PageableSearchFilter<SettingDetailsSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<SettingDetailsSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new SettingDetailsSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("SettingKey", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var settings = context.Settings.OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                settings = settings.Where(setting => setting.ClubId == filter.ClubId);
                settings = settings.Where(setting => setting.UserId == filter.UserId);
                settings = settings.WhereIf(filter.SettingKey,
                    setting => setting.SettingKey.ToLower() == filter.SettingKey.ToLower());
                
                var pagedQuery = new PagedQuery<Setting>(settings, pageStart, pageSize);

                var result = pagedQuery.Items.ToList().Select(x => new SettingDetails()
                {
                    SettingId = x.SettingId,
                    SettingKey = x.SettingKey,
                    SettingValue = x.SettingValue,
                    ClubId = x.ClubId,
                    UserId = x.UserId
                })
                .ToList();

                SetSettingSecurity(result);

                var pagedList = new PagedList<SettingDetails>(result, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }
        
        public SettingDetails GetSettingDetails(Guid settingId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var setting = context.Settings.FirstOrDefault(c => c.SettingId == settingId);

                var settingDetails = setting.ToSettingDetails();
                SetSettingDetailsSecurity(settingDetails);
                return settingDetails;
            }
        }

        public void InsertSettingDetails(SettingDetails settingDetails)
        {
            var setting = settingDetails.ToSetting();
            setting.EntityNotNull("Setting", Guid.Empty);

            CheckSecurity(setting);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Settings.Add(setting);
                context.SaveChanges();
            }

            //Map it back to details
            setting.ToSettingDetails(settingDetails);
        }

        public void InsertOrUpdateSettingDetails(SettingDetails settingDetails)
        {
            settingDetails.ArgumentNotNull("settingDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Settings.FirstOrDefault(x => x.SettingId == settingDetails.SettingId);

                if (original == null)
                {
                    //insert new setting
                    original = settingDetails.ToSetting();
                    CheckSecurity(original);
                    context.Settings.Add(original);
                }
                else
                {
                    //update setting
                    CheckSecurity(original);
                    settingDetails.ToSetting(original);
                }

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToSettingDetails(settingDetails);
                }
            }
        }

        public void DeleteSetting(Guid settingId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Settings.FirstOrDefault(x => x.SettingId == settingId);
                original.EntityNotNull("Setting", settingId);

                CheckSecurity(original);

                context.Settings.Remove(original);
                context.SaveChanges();
            }
        }

        private void SetSettingSecurity(List<SettingDetails> result)
        {

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                result.Clear();
                return;
            }

            foreach (var record in result.Where(x => x.ClubId.HasValue))
            {
                if (IsCurrentUserInRoleClubAdministrator)
                {
                    record.CanUpdateRecord = true;
                    record.CanDeleteRecord = true;
                }
            }

            foreach (var record in result.Where(x => x.UserId.HasValue))
            {
                record.CanUpdateRecord = true;
                record.CanDeleteRecord = true;
            }

            if (IsCurrentUserInRoleSystemAdministrator == false)
            {
                //remove all system settings from the list if the user is not an system admin
                result.RemoveAll(x => x.ClubId.HasValue == false && x.UserId.HasValue == false);
            }
            else
            {
                foreach (var record in result.Where(x => x.ClubId.HasValue && x.UserId.HasValue))
                {
                    record.CanUpdateRecord = true;
                    record.CanDeleteRecord = true;
                }
            }
        }

        private void SetSettingDetailsSecurity(SettingDetails record)
        {

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                return;
            }

            if (record.ClubId.HasValue && IsCurrentUserInRoleClubAdministrator 
                && CurrentAuthenticatedFLSUserClubId == record.ClubId.Value)
            {
                record.CanUpdateRecord = true;
                record.CanDeleteRecord = true;
            }

            if (record.UserId.HasValue && CurrentAuthenticatedFLSUser.UserId == record.UserId.Value)
            {
                record.CanUpdateRecord = true;
                record.CanDeleteRecord = true;
            }

            if (IsCurrentUserInRoleSystemAdministrator
                && record.ClubId.HasValue == false
                && record.UserId.HasValue == false)
            {
                record.CanUpdateRecord = true;
                record.CanDeleteRecord = true;
            }
        }

        private void CheckSecurity(Setting setting)
        {
            if (setting.ClubId.HasValue == false && setting.UserId.HasValue == false
                && IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleSystemAdmin);
            }

            if (setting.ClubId.HasValue && setting.ClubId.Value != CurrentAuthenticatedFLSUserClubId
                && IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleSystemAdmin);
            }

            if (setting.ClubId.HasValue && setting.ClubId.Value == CurrentAuthenticatedFLSUserClubId
                && IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            if (setting.UserId.HasValue && setting.UserId.Value != CurrentAuthenticatedFLSUser.UserId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
