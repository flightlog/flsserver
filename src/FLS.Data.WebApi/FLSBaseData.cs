using System;
using System.Reflection;
using System.Text;

namespace FLS.Data.WebApi
{
    public abstract class FLSBaseData
    {
        public abstract Guid Id { get; set; }

        public bool CanUpdateRecord { get; set; }

        public bool CanDeleteRecord { get; set; }
        
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
