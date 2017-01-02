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

        public static TResult? MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TResult : struct
        {
            return source
                .Select(selector)
                .Cast<TResult?>()
                .Max();
        }
    }
}
