using System;
using System.Collections.Generic;
using System.Linq;

namespace FLS.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, Predicate<T> condition, Func<T, T> replaceAction)
        {
            return items.Select(item => condition(item) ? replaceAction(item) : item);
        }
    }
}
