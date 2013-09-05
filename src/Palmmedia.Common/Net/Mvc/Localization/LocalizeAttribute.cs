using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// Adds possibility to manually switch the user's language.
    /// On the first request the browsers preferred language is used and stored in the session.
    /// On any further request the language from the session is used, unless a language is specified manually in the query string.
    /// To make the localization feature work, all <see cref="Controller">Controllers</see> should use this attribute.
    /// <example>
    /// To switch the language manually perform a request with the following query string:
    /// ?culture=TwoLetterISOLanguageName (e.g. ?culture=en).
    /// </example>
    /// </summary>
    public class LocalizeAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string culture = filterContext.HttpContext.Request.QueryString["culture"];

            /* First check if language has been set manually.
             * If not check if language is stored within the session.
             * If not (first request) use the browser's preferred language. */
            if (culture != null)
            {
                filterContext.HttpContext.Session["culture"] = culture;
                ApplyCulture(culture);
            }
            else if (filterContext.HttpContext.Session["culture"] != null)
            {
                ApplyCulture((string)filterContext.HttpContext.Session["culture"]);
            }
            else
            {
                try
                {
                    var userLanguages = filterContext.HttpContext.Request.UserLanguages;
                    if (userLanguages != null && userLanguages.Length > 0 && userLanguages[0].Length > 1)
                    {
                        var browserCulture = userLanguages[0].Substring(0, 2);
                        filterContext.HttpContext.Session["culture"] = browserCulture;
                    }
                    else
                    {
                        filterContext.HttpContext.Session["culture"] = "en";
                    }
                }
                catch (ArgumentException)
                {
                    filterContext.HttpContext.Session["culture"] = "en";
                }

                ApplyCulture((string)filterContext.HttpContext.Session["culture"]);
            }
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <summary>
        /// Applies the given culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        private static void ApplyCulture(string culture)
        {
            var cultureInfo = CultureInfo.InvariantCulture;

            if (culture.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                cultureInfo = CultureInfo.CreateSpecificCulture("de");
            }

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
