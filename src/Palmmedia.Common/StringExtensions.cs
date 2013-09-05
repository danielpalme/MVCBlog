using System.Collections.Generic;
using System.Linq;

namespace Palmmedia.Common
{
    /// <summary>
    /// Contains extension methods for processing <see cref="string">strings</see>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Shortens the given <see cref="string" />.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to shorten.</param>
        /// <param name="maxlength">The maximum length of the <see cref="string"/>.</param>
        /// <returns>The shortened <see cref="string"/>.</returns>
        public static string Trim(this string value, int maxlength)
        {
            if (string.IsNullOrEmpty(value) || maxlength > value.Length)
            {
                return value;
            }

            return value.Substring(0, maxlength - 4) + " ...";
        }

        /// <summary>
        /// Returns <c>null</c> if <see cref="string"/> is empty otherwise the <see cref="string"/> itself.
        /// Gets the text or null if empty.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns><c>null</c> if <see cref="string"/> is empty otherwise the <see cref="string"/> itself.</returns>
        public static string GetTextOrNullIfEmpty(this string value)
        {
            if (value == null || value.Trim().Length == 0)
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> containing all values separated by the given <see cref="string"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing all values separated by the given <see cref="string"/>.
        /// </returns>
        public static string ToString<T>(this IEnumerable<T> values, string separator)
        {
            if (values == null)
            {
                return string.Empty;
            }

            var valueArray = values.ToArray();

            return string.Join(separator, values.Select(v => v.ToString()).ToArray());
        }
    }
}
