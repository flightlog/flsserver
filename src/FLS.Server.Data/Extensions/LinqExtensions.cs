using System;
using System.Linq;
using System.Linq.Expressions;
using FLS.Data.WebApi;

namespace FLS.Server.Data.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Adds a where-clause if condition is true to a query. Usage: .WhereIf(batchNumber != null, s => s.Number == batchNumber)
        /// http://stackoverflow.com/questions/33153932/filter-search-using-multiple-fields-asp-net-mvc
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereDateTimeFilter<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, DateTimeFilter filter)
        {
            if (filter == null ||
                (filter.From.HasValue == false && filter.To.HasValue == false))
            {
                return source;
            }

            //var nullExpr = Expression.Constant(null, typeof(TKey));
            //var equalExpr = Expression.Equal(keySelector.Body, nullExpr);
            //var lambda = Expression.Lambda<Func<TSource, bool>>(equalExpr, keySelector.Parameters);
            //return source.Where(lambda);
            
            //(flight.ModifiedOn.HasValue && flight.ValidatedOn.HasValue && (flight.ModifiedOn >= flight.ValidatedOn))))

            var nullExpr = Expression.Constant(null, typeof(TKey));
            var from = filter.From.GetValueOrDefault(DateTime.MinValue);
            var to = filter.To.GetValueOrDefault(DateTime.MaxValue);

            var after = Expression.LessThanOrEqual(keySelector.Body,
                 Expression.Constant(from, typeof(DateTime)));

            var before = Expression.GreaterThanOrEqual(
                keySelector.Body, Expression.Constant(to, typeof(DateTime)));

            Expression body = Expression.And(after, before);

            var lambdaBetween = Expression.Lambda<Func<TSource, bool>>(body, keySelector.Parameters);
            return source.Where(lambdaBetween);
        }

        /// <summary>
        /// http://stackoverflow.com/questions/12496019/expression-tree-to-know-if-a-date-is-between-2-dates-in-c-sharp
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IQueryable<TElement> IsDateBetween<TElement>(this IQueryable<TElement> queryable,
                                                           Expression<Func<TElement, DateTime>> fromDate,
                                                           Expression<Func<TElement, DateTime>> toDate,
                                                           DateTime date)
        {
            var p = fromDate.Parameters.Single();
            Expression member = p;

            Expression fromExpression = Expression.Property(member, (fromDate.Body as MemberExpression).Member.Name);
            Expression toExpression = Expression.Property(member, (toDate.Body as MemberExpression).Member.Name);

            var after = Expression.LessThanOrEqual(fromExpression,
                 Expression.Constant(date, typeof(DateTime)));

            var before = Expression.GreaterThanOrEqual(
                toExpression, Expression.Constant(date, typeof(DateTime)));

            Expression body = Expression.And(after, before);

            var predicate = Expression.Lambda<Func<TElement, bool>>(body, p);
            return queryable.Where(predicate);
        }

    }
}
