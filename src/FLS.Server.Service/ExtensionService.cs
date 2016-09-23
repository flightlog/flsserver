using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using NLog;
using System;

namespace FLS.Server.Service
{
    public class ExtensionService : BaseService, IExtensionService
    {
        private readonly DataAccessService _dataAccessService;

        public ExtensionService(DataAccessService dataAccessService, IdentityService identityService) 
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region ExtensionValue
        public string GetExtensionStringValue(string key, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == clubId);

                if (record == null) return string.Empty;

                return record.ExtensionStringValue;
            }
        }

        public byte[] GetExtensionBinaryValue(string key, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == clubId);

                if (record == null) return null;

                return record.ExtensionBinaryValue;
            }
        }

        public void SaveExtensionStringValue(string key, string value, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == clubId);

                if (record == null)
                {
                    record = new ExtensionValue();
                    context.ExtensionValues.Add(record);
                }

                record.ClubId = clubId;
                record.ExtensionValueKeyName = key;
                record.ExtensionValueName = key;
                record.ExtensionStringValue = value;

                context.SaveChanges();
            }
        }

        public void SaveExtensionBinaryValue(string key, byte[] value, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == clubId);

                if (record == null)
                {
                    record = new ExtensionValue();
                    context.ExtensionValues.Add(record);
                }

                record.ClubId = clubId;
                record.ExtensionValueKeyName = key;
                record.ExtensionValueName = key;
                record.ExtensionBinaryValue = value;

                context.SaveChanges();
            }
        }

        public void DeleteExtensionValue(string key, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                if (context.ExtensionValues.Any(x => x.ExtensionValueKeyName == key && x.ClubId == clubId))
                {
                    var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == CurrentAuthenticatedFLSUserClubId);

                    context.ExtensionValues.Remove(record);
                    context.SaveChanges();
                }
            }
        }
        #endregion ExtensionValue
        
    }
}
