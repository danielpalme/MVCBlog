using System.ServiceModel.Syndication;

namespace Palmmedia.Common.Net.Mvc.Feed
{
    /// <summary>
    /// <see cref="System.Web.Mvc.ActionResult"/> which provides an RSS feed.
    /// </summary>
    public class RssSyndicationFeedActionResult : SyndicationFeedActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssSyndicationFeedActionResult"/> class.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/>.</param>
        public RssSyndicationFeedActionResult(SyndicationFeed feed)
            : base(feed)
        {
        }

        /// <summary>
        /// Gets the syndication feed formatter.
        /// </summary>
        protected override SyndicationFeedFormatter SyndicationFeedFormatter
        {
            get
            {
                return new Rss20FeedFormatter(this.Feed);
            }
        }
    }
}
