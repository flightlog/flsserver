using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace FLS.Common.Extensions
{
    public static class ListExtensions
    {
        public static List<U> FindDuplicates<T, U>(this List<T> list, Func<T, U> keySelector)
        {
            return list.GroupBy(keySelector)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();
        }
    }
}
