using System;

namespace FLS.Server.Data
{
    public interface IOwnershipMetaData 
    {
        Guid OwnerId { get; }

        int OwnershipType { get; }
    }
}
