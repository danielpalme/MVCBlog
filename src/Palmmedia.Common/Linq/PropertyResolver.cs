using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Palmmedia.Common.Linq
{
    /// <summary>
    /// Resolves the names of properties by supplying a lambda expression.
    /// This can be useful to create compiler validated <see cref="string">strings</see>.
    /// Usecase: INotifyPropertyChanged
    /// </summary>
    public static class PropertyResolver
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="type">The current type.</param>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <returns>The name of the property if property exists, otherwise <c>null</c>.</returns>
        public static string GetPropertyName<T>(this T type, Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T>(expression);
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <returns>The name of the property if property exists, otherwise <c>null</c>.</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }
    }
}
