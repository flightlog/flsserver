using System;

namespace FLS.Common.Extensions
{
    public static class GuidExtensions
    {
        public static Nullable<Guid> GetNullableGuid(this Nullable<Guid> guid)
        {
            if (guid == null || Equals(guid, Guid.Empty)) return null;

            return guid;
        }

        public static bool IsValid(this Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}