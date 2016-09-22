using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using NLog;

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
        public string GetExtensionStringValue(string key)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == CurrentAuthenticatedFLSUserClubId);

                if (record == null) return string.Empty;

                return record.ExtensionStringValue;
            }
        }
        

        public void SaveExtensionStringValue(string key, string value)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var record = context.ExtensionValues.FirstOrDefault(x => x.ExtensionValueKeyName == key && x.ClubId == CurrentAuthenticatedFLSUserClubId);

                if (record == null)
                {
                    record = new ExtensionValue();
                    context.ExtensionValues.Add(record);
                }

                record.ClubId = CurrentAuthenticatedFLSUserClubId;
                record.ExtensionValueKeyName = key;
                record.ExtensionValueName = key;
                record.ExtensionStringValue = value;

                context.SaveChanges();
            }
        }
        
        public void DeleteExtensionValue(string key)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                if (context.ExtensionValues.Any(x => x.ExtensionValueKeyName == key && x.ClubId == CurrentAuthenticatedFLSUserClubId))
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
