using System.Web.Mvc;

namespace Palmmedia.Common.Net.PingBack
{
    /// <summary>
    /// Adds the 'X-Pingback' header to each response.
    /// </summary>
    public class PingbackAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingbackAttribute"/> class.
        /// </summary>
        /// <param name="pingbackPath">The path of the Pingback handler.</param>
        public PingbackAttribute(string pingbackPath)
        {
            this.PingbackPath = pingbackPath;
        }

        /// <summary>
        /// Gets the path of the Pingback handler.
        /// </summary>
        public string PingbackPath { get; private set; }

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Url.GetLeftPart(System.UriPartial.Authority) + System.Web.VirtualPathUtility.ToAbsolute("~/") + this.PingbackPath;

            filterContext.HttpContext.Response.AddHeader(
                "X-Pingback",
                url);
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
