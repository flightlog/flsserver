using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace FLS.Common.Extensions
{
    /// <summary>
    /// <see cref="http://stackoverflow.com/questions/24597120/automapper-how-to-ignore-all-properties-that-are-marked-as-virtual"/>
    /// </summary>
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination>
                   IgnoreAllVirtual<TSource, TDestination>(
                       this IMappingExpression<TSource, TDestination> expression)
        {
            var desType = typeof(TDestination);
            foreach (var property in desType.GetProperties().Where(p =>
                                     p.GetGetMethod().IsVirtual))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            return expression;
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
