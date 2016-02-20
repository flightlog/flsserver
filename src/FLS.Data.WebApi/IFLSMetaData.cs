using System;
using FLS.Server.Data;

namespace FLS.Data.WebApi
{
    public interface IFLSMetaData : IOwnershipMetaData
    {
        Guid Id { get; }

        Nullable<DateTime> DeletedOn { get; }

        Nullable<Guid> DeletedByUserId { get; }

        DateTime CreatedOn { get; }

        Guid CreatedByUserId { get; }

        Nullable<int> RecordState { get; }

        Nullable<DateTime> ModifiedOn { get; }

        Nullable<Guid> ModifiedByUserId { get; }
    }
}
