using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Palmmedia.Common.Net
{
    /// <summary>
    /// Helper methods to modify URLs.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Replaces a parameter within an URL.
        /// If <c>null</c> is supplied as new value, the parameter gets removed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The new URL.</returns>
        public static string SetParameter(this string url, string param, string value)
        {
            int questionMarkIndex = url.IndexOf('?');
            NameValueCollection parameters;
            var result = new StringBuilder();

            if (questionMarkIndex == -1)
            {
                parameters = new NameValueCollection();
                result.Append(url);
            }
            else
            {
                parameters = HttpUtility.ParseQueryString(url.Substring(questionMarkIndex));
                result.Append(url.Substring(0, questionMarkIndex));
            }

            if (string.IsNullOrEmpty(value))
            {
                parameters.Remove(param);
            }
            else
            {
                parameters[param] = value;
            }

            if (parameters.Count > 0)
            {
                result.Append('?');

                foreach (string parameterName in parameters)
                {
                    result.AppendFormat("{0}={1}&", parameterName, parameters[parameterName]);
                }

                result.Remove(result.Length - 1, 1);
            }

            return result.ToString();
        }
    }
}
