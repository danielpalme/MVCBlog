using System.Web.Routing;

namespace Palmmedia.Common.Net.Mvc.LegacyRouting
{
    /// <summary>
    /// <see cref="Route"/> that helps handling legacy requests.
    /// </summary>
    public class LegacyRoute : Route
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="target">The redirect target.</param>
        public LegacyRoute(string url, string target)
            : base(url, new LegacyRouteHandler())
        {
            this.Target = target;
        }

        /// <summary>
        /// Gets the redirect target.
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains information about the URL that is associated with the route.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
