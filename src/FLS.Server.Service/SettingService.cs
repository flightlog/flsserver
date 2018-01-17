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
using FLS.Server.Data.Exceptions;
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
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BadRequestException("Key is required!");
            }

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

        public bool TryGetSettingValue<T>(string key, Guid? clubId, Guid? userId, out T settingValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BadRequestException("Key is required!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.Settings
                    .WhereIf(clubId.HasValue, x => x.ClubId == clubId)
                    .WhereIf(userId.HasValue, x => x.UserId == userId)
                    .FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower());

                if (record == null)
                {
                    settingValue = default(T);
                    return false;
                }

                if (typeof(T) != typeof(string))
                {
                    settingValue = JsonConvert.DeserializeObject<T>(record.SettingValue);
                    return true;
                }

                settingValue = (T)Convert.ChangeType(record.SettingValue, typeof(T));
                return true;
            }
        }

        public string GetSettingValue(string key, Guid? clubId, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BadRequestException("Key is required!");    
            }

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

        public bool TryGetSettingValue(string key, Guid? clubId, Guid? userId, out string settingValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BadRequestException("Key is required!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.Settings
                    .WhereIf(clubId.HasValue, x => x.ClubId == clubId)
                    .WhereIf(userId.HasValue, x => x.UserId == userId)
                    .FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower());

                if (record == null)
                {
                    settingValue = null;
                    return false;
                }

                settingValue = record.SettingValue;
                return true;
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
                settings = settings.WhereIf(filter.ClubId.HasValue, setting => setting.ClubId == filter.ClubId);
                settings = settings.WhereIf(filter.UserId.HasValue, setting => setting.UserId == filter.UserId);
                settings = settings.WhereIf(filter.SettingKey,
                    setting => setting.SettingKey.ToLower().Contains(filter.SettingKey.ToLower()));
                
                var pagedQuery = new PagedQuery<Setting>(settings, pageStart, pageSize);

                var result = pagedQuery.Items.ToList().Select(x => new SettingDetails()
                {
                    SettingKey = x.SettingKey,
                    SettingValue = x.SettingValue,
                    ClubId = x.ClubId,
                    UserId = x.UserId
                })
                .ToList();

                var pagedList = new PagedList<SettingDetails>(result, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }
        
        public void InsertOrUpdateSettingDetails(SettingDetails settingDetails)
        {
            settingDetails.ArgumentNotNull("settingDetails");

            if (string.IsNullOrWhiteSpace(settingDetails.SettingKey))
            {
                throw new BadRequestException("Key is required!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Settings
                    .WhereIf(settingDetails.ClubId.HasValue, x => x.ClubId == settingDetails.ClubId)
                    .WhereIf(settingDetails.UserId.HasValue, x => x.UserId == settingDetails.UserId)
                    .FirstOrDefault(x => x.SettingKey.ToLower() == settingDetails.SettingKey.ToLower());

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

        public void DeleteSetting(string key, Guid? clubId, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BadRequestException("Key is required!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Settings
                    .WhereIf(clubId.HasValue, x => x.ClubId == clubId)
                    .WhereIf(userId.HasValue, x => x.UserId == userId)
                    .FirstOrDefault(x => x.SettingKey.ToLower() == key.ToLower());
                original.EntityNotNull("Setting");

                CheckSecurity(original);

                context.Settings.Remove(original);
                context.SaveChanges();
            }
        }
        
        private void CheckSecurity(Setting setting)
        {
            if (setting.ClubId.HasValue == false && setting.UserId.HasValue == false
                && IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a system administrator to edit this setting!");
            }

            if (setting.ClubId.HasValue && setting.ClubId.Value != CurrentAuthenticatedFLSUserClubId
                && IsCurrentUserInRoleSystemAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a system administrator to edit this setting!");
            }

            if (setting.ClubId.HasValue && setting.ClubId.Value == CurrentAuthenticatedFLSUserClubId
                && IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to edit this setting!");
            }

            if (setting.UserId.HasValue && setting.UserId.Value != CurrentAuthenticatedFLSUser.UserId)
            {
                throw new UnauthorizedAccessException("You are not allowed to edit this user setting!");
            }
        }
    }
}
