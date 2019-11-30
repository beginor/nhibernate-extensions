using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.NetCore {

    public static class QueryableExtensions {

        private static MethodInfo orderByMethod;
        private static MethodInfo orderByDescMethod;

        private static MethodInfo OrderBy(System.Type source, System.Type key) {
            if (orderByMethod == null) {
                orderByMethod = new Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(
                    Queryable.OrderBy<object, object>
                ).GetMethodInfo().GetGenericMethodDefinition();
            }
            return orderByMethod.MakeGenericMethod(source, key);
        }

        private static MethodInfo OrderByDescending(System.Type source, System.Type key) {
            if (orderByDescMethod == null) {
                orderByDescMethod = new Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(
                    Queryable.OrderByDescending<object, object>
                ).GetMethodInfo().GetGenericMethodDefinition();
            }
            return orderByDescMethod.MakeGenericMethod(source, key);
        }

        public static IOrderedQueryable<TSource> AddOrderBy<TSource>(
            this IQueryable<TSource> queryable,
            string propertyName,
            bool isAsc
        ) {
            return isAsc ? OrderBy(queryable, propertyName)
                : OrderByDescending(queryable, propertyName);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(
            this IQueryable<TSource> queryable,
            string propertyName
        ) {
            var expr = CreatePropertyAccessExpression<TSource>(propertyName);
            var result = queryable.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    OrderBy(typeof(TSource), expr.ReturnType),
                    queryable.Expression,
                    Expression.Quote(expr)
                )
            );
            return result as IOrderedQueryable<TSource>;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(
            this IQueryable<TSource> queryable,
            string propertyName
        ) {
            var expr = CreatePropertyAccessExpression<TSource>(propertyName);
            var result = queryable.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    OrderByDescending(typeof(TSource), expr.ReturnType),
                    queryable.Expression,
                    Expression.Quote(expr)
                )
            );
            return result as IOrderedQueryable<TSource>;
        }

        /// <summary>
        /// create lambda : x => x.PropertyName
        /// </summary>
        private static LambdaExpression CreatePropertyAccessExpression<TSource>(
            string propertyName
        ) {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            var propertyInfo = typeof(TSource).GetProperty(propertyName);
            var funcType = typeof(Func<,>).MakeGenericType(typeof(TSource), propertyInfo.PropertyType);
            var access = Expression.MakeMemberAccess(parameter, propertyInfo);
            var result = Expression.Lambda(funcType, access, parameter);
            return result;
        }

    }

}
