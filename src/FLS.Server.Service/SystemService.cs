using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Utils;
using FLS.Common.Validators;
using FLS.Data.WebApi.Audit;
using FLS.Data.WebApi.System;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using NLog;

namespace FLS.Server.Service
{
    public class SystemService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public SystemService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region AuditLog
        public List<string> GetTrackedEntityNames()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.AuditLog
                    .Select(x => x.TypeFullName.Replace("FLS.Server.Data.DbEntities.", ""))
                    .Distinct()
                    .ToList();

                return list;
            }
        }

        public List<AuditLogOverview> GetAuditLogOverviews(string entityName)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.GetLogs(entityName.ToTypeFullName())
                    .Include("LogDetails")
                    .OrderByDescending(o => o.EventDateUTC)
                    .ToList() //ToList is required before ToAuditLogOverview()
                    .Select(x => x.ToAuditLogOverview())
                    .ToList();

                return list;
            }
        }

        public List<AuditLogOverview> GetAuditLogOverviews(string entityName, Guid recordId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.GetLogs(entityName.ToTypeFullName(), recordId)
                    .Include("LogDetails")
                    .OrderByDescending(o => o.EventDateUTC)
                    .ToList() //ToList is required before ToAuditLogOverview()
                    .Select(x => x.ToAuditLogOverview())
                    .ToList();

                return list;
            }
        }
        
        internal List<SystemLog> GetAuditLogs()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.SystemLogs.OrderByDescending(c => c.LogId).Take(2000).ToList();

                return list;
            }
        }

        internal SystemLog GetAuditLog(long logId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                

                var systemLog = context.SystemLogs.FirstOrDefault(q => q.LogId == logId);

                return systemLog;
            }
        }

        #endregion AuditLog

        #region SystemData
        public SystemDataDetails GetSystemDataDetails()
        {
            var systemData = GetSystemData();

            var systemDataDetails = systemData.ToSystemDataDetails();
            SetSystemDataDetailsSecurity(systemDataDetails);

            return systemDataDetails;
        }
        
        internal SystemData GetSystemData()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var systemData = context.SystemDatas.FirstOrDefault();

                return systemData;
            }
        }

        public string GetSystemSenderEmailAddress()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var systemSenderEmailAddress =
                    context.SystemDatas.Select(x => x.SystemSenderEmailAddress).FirstOrDefault();

                return systemSenderEmailAddress;
            }
        }

        public void UpdateSystemDataDetails(SystemDataDetails currentSystemDataDetails)
        {
            currentSystemDataDetails.ArgumentNotNull("currentSystemDataDetails");
            var original = GetSystemData();
            original.EntityNotNull("SystemData", currentSystemDataDetails.SystemDataId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.SystemDatas.Attach(original);
                currentSystemDataDetails.ToSystemData(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToSystemDataDetails(currentSystemDataDetails);
                }
            }
        }
        #endregion SystemData

        #region SystemLog

        public List<SystemLogOverview> GetSystemLogOverviews()
        {
            var dbEntityList = GetSystemLogs();

            var systemLogOverviewResult = dbEntityList.Select(systemLog => systemLog.ToSystemLogOverview()).ToList();

            return systemLogOverviewResult;
        }

        public SystemLogDetails GetSystemLogDetails(long logId)
        {
            var entity = GetSystemLog(logId);

            var systemLogDetails = entity.ToSystemLogDetails();

            return systemLogDetails;
        }

        internal List<SystemLog> GetSystemLogs()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.SystemLogs.OrderByDescending(c => c.LogId).Take(2000).ToList();

                return list;
            }
        }

        internal SystemLog GetSystemLog(long logId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var systemLog = context.SystemLogs.FirstOrDefault(q => q.LogId == logId);

                return systemLog;
            }
        }

        #endregion SystemLog

        #region SystemVersionInfo

        public SystemVersionInfoOverview GetSystemVersionInfoOverview()
        {
            var systemVersionInfoDetails = GetSystemVersionInfoDetails();

            return systemVersionInfoDetails.ToSystemVersionInfoOverview();
        }

        public SystemVersionInfoDetails GetSystemVersionInfoDetails()
        {
            var systemVersionInfo = new SystemVersionInfoDetails();
            using (var context = _dataAccessService.CreateDbContext())
            {
                var systemVersion = context.SystemVersions.OrderByDescending(c => c.UpgradeDateTime).FirstOrDefault();

                if (systemVersion != null)
                {
                    systemVersionInfo.DatabaseSchemaVersion =
                        $"{systemVersion.MajorVersion}.{systemVersion.MinorVersion}.{systemVersion.BuildVersion}.{systemVersion.RevisionVersion}";
                }
                else
                {
                    systemVersionInfo.DatabaseSchemaVersion = "n/a";
                }
            }

            var assemblyBuildInfo = AssemblyUtil.GetAssemblyVersion("FLS.Server.WebApi");
            systemVersionInfo.AssembliesInfo.Add(assemblyBuildInfo.ToAssemblyInfo());

            assemblyBuildInfo = AssemblyUtil.GetAssemblyVersion("FLS.Server.Service");
            systemVersionInfo.AssembliesInfo.Add(assemblyBuildInfo.ToAssemblyInfo());

            assemblyBuildInfo = AssemblyUtil.GetAssemblyVersion("FLS.Server.Data");
            systemVersionInfo.AssembliesInfo.Add(assemblyBuildInfo.ToAssemblyInfo());

            assemblyBuildInfo = AssemblyUtil.GetAssemblyVersion("FLS.Data.WebApi");
            systemVersionInfo.AssembliesInfo.Add(assemblyBuildInfo.ToAssemblyInfo());

            assemblyBuildInfo = AssemblyUtil.GetAssemblyVersion("FLS.Common");
            systemVersionInfo.AssembliesInfo.Add(assemblyBuildInfo.ToAssemblyInfo());

            return systemVersionInfo;
        }
        #endregion SystemVersionInfo

        #region Security
        private void SetSystemDataDetailsSecurity(SystemDataDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("SystemDataDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = false; //is false because delete is not possible system-wide
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
