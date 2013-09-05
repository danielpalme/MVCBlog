using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace Palmmedia.Common.Net.Mvc.Feed
{
    /// <summary>
    /// <see cref="ActionResult"/> which provides a feed.
    /// </summary>
    public abstract class SyndicationFeedActionResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeedActionResult"/> class.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/>.</param>
        protected SyndicationFeedActionResult(SyndicationFeed feed)
        {
            this.Feed = feed;
        }

        /// <summary>
        /// Gets the feed.
        /// </summary>
        public SyndicationFeed Feed { get; private set; }

        /// <summary>
        /// Gets the syndication feed formatter.
        /// </summary>
        protected abstract SyndicationFeedFormatter SyndicationFeedFormatter { get; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                this.SyndicationFeedFormatter.WriteTo(writer);
            }
        }
    }
}
