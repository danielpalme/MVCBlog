using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Palmmedia.Common.Net.Mvc.LegacyRouting
{
    /// <summary>
    /// Handles a legacy request by redirecting to another route. 
    /// </summary>
    public class LegacyHandler : MvcHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyHandler"/> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        public LegacyHandler(RequestContext requestContext)
            : base(requestContext)
        {
        }

        /// <summary>
        /// Called by ASP.NET to begin asynchronous request processing.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="callback">The asynchronous callback method.</param>
        /// <param name="state">The state of the asynchronous object.</param>
        /// <returns>
        /// The status of the asynchronous call.
        /// </returns>
        protected override System.IAsyncResult BeginProcessRequest(HttpContext httpContext, System.AsyncCallback callback, object state)
        {
            var legacyRoute = RequestContext.RouteData.Route as LegacyRoute;

            httpContext.Response.Status = "301 Moved Permanently";
            httpContext.Response.RedirectLocation = legacyRoute.Target;
            httpContext.Response.End();

            return null;
        }
    }
}
