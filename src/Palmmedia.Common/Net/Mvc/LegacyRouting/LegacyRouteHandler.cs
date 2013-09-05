using System.Web;
using System.Web.Routing;

namespace Palmmedia.Common.Net.Mvc.LegacyRouting
{
    /// <summary>
    /// <see cref="IRouteHandler"/> that creates instances of <see cref="LegacyHandler"/>
    /// </summary>
    public class LegacyRouteHandler : IRouteHandler
    {
        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext) 
        {
            return new LegacyHandler(requestContext);
        }
    }
}
