using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
using log4net;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// Extends <see cref="System.Web.Mvc.HandleErrorAttribute"/> to work properly with ELMAH.
    /// ELMAH can only handle exceptions that are not handled yet.
    /// See: http://stackoverflow.com/questions/766610/how-to-get-elmah-to-work-with-asp-net-mvc-handleerror-attribute
    /// </summary>
    public sealed class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandleErrorAttribute));

        /// <summary>
        /// Called when exception occurs.
        /// </summary>
        /// <param name="filterContext">The <see cref="ExceptionContext"/>.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            // Give base implementation the chance to handle exception
            base.OnException(filterContext);

            var e = filterContext.Exception;

            // If unhandled, will be logged anyhow, prefer signaling if possible
            if (!filterContext.ExceptionHandled
                || RaiseErrorSignal(e) 
                || IsFiltered(filterContext))
            {
                return;
            }

            LogException(e);
        }

        /// <summary>
        /// Raises the error signal if in HttpContext.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns>True if exception could be raised, otherwise false.</returns>
        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return false;
            }

            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
            {
                return false;
            }

            signal.Raise(e, context);
            return true;
        }

        /// <summary>
        /// Determines whether the exception is filtered.
        /// </summary>
        /// <param name="filterContext">The <see cref="ExceptionContext"/>.</param>
        /// <returns>True if exception is filtered, otherwise false.</returns>
        private static bool IsFiltered(ExceptionContext filterContext) 
        {
            var config = filterContext.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

            if (config == null)
            {
                return false;
            }

            var testContext = new ErrorFilterModule.AssertionHelperContext(filterContext.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }

        /// <summary>
        /// Logs the given exception.
        /// </summary>
        /// <param name="e">The exception to log.</param>
        private static void LogException(Exception e)
        {
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(e, context));

            var request = context.Request;
            Logger.Error(string.Format("Unhandled exception (Referrer: {0}, URL: {1}, Parameters: {2}).", request.UrlReferrer, request.Url, request.Params.ToString()), e);
        }
    }
}