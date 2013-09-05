using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Palmmedia.Common.Linq
{
    /// <summary>
    /// Extensions for EntityFramework.
    /// </summary>
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// Checks if a value is contained in the given collection.
        /// Extension method since the Contains()-method in not supported in EF.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>An IQueryable.</returns>
        public static IQueryable<TEntity> WhereIn<TEntity, TValue>(
            this ObjectQuery<TEntity> query,
            Expression<Func<TEntity, TValue>> selector, 
            IEnumerable<TValue> collection)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            var parameterExpression = selector.Parameters.Single();

            if (!collection.Any())
            {
                return query;
            }

            var equals = collection.Select(value => (Expression)Expression.Equal(selector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate((accumulate, equal) => Expression.Or(accumulate, equal));
            return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, parameterExpression));
        }
    }
}
