using System;
using System.Text;
using System.Reflection;

namespace FLS.Data.WebApi
{
    public abstract class FLSMetaData : FLSBaseData
    {
        public Nullable<DateTime> DeletedOn { get; set; }

        public Nullable<Guid> DeletedByUserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public Nullable<int> RecordState { get; set; }

        public Nullable<DateTime> ModifiedOn { get; set; }

        public Nullable<Guid> ModifiedByUserId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));

            foreach (PropertyInfo info in type.GetProperties())
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
