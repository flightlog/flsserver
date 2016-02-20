using System;
using FLS.Common.Extensions;
using FLS.Data.WebApi;

namespace FLS.Server.Tests.Extensions
{
    public static class FLSMetaDataExtensions
    {
        public static void RemoveMetadataInfo(this IFLSMetaData flsEntity)
        {
            if (flsEntity == null) return;

            flsEntity.SetPropertyValue("Id", Guid.Empty);
            flsEntity.SetPropertyValue("CreatedOn", null);
            flsEntity.SetPropertyValue("CreatedByUserId", Guid.Empty);
            flsEntity.SetPropertyValue("ModifiedOn", null);
            flsEntity.SetPropertyValue("ModifiedByUserId", null);
            flsEntity.SetPropertyValue("DeletedOn", null);
            flsEntity.SetPropertyValue("DeletedByUserId", null);
            flsEntity.SetPropertyValue("OwnerId", Guid.Empty);
            flsEntity.SetPropertyValue("OwnershipType", 0);
            flsEntity.SetPropertyValue("RecordState", null);
        }
    }
}
