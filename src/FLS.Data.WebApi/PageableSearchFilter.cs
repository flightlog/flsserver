using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace FLS.Data.WebApi
{
    public class PageableSearchFilter<T>
    {
        public PageableSearchFilter()
        {
            Sorting = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Sorting { get; set; }

        public T SearchFilter { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
                sb.Append($"{info.Name}: {info.GetValue(this)}, ");

            foreach (PropertyInfo info in type.GetProperties())
            {
                if (info.Name == "Sorting")
                {
                    sb.Append($"{info.Name}:");

                    if (Sorting == null)
                    {
                        sb.Append("No entries");
                    }
                    else
                    {
                        var sc = new StringBuilder();

                        foreach (var sortColumn in Sorting.Keys)
                        {
                            sc.Append($"{sortColumn}:{Sorting[sortColumn]},");
                        }
                        var sortExpression = sc.ToString().Replace(":asc", "").Replace(":desc", " descending").Trim(',');
                        sb.Append(sortExpression);
                    }

                    continue;
                }

                sb.Append($"{info.Name}: {info.GetValue(this, null)}, ");
            }
            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
