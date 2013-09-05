using System.ServiceModel.Syndication;

namespace Palmmedia.Common.Net.Mvc.Feed
{
    /// <summary>
    /// <see cref="System.Web.Mvc.ActionResult"/> which provides an ATOM feed.
    /// </summary>
    public class AtomSyndicationFeedActionResult : SyndicationFeedActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomSyndicationFeedActionResult"/> class.
        /// </summary>
        /// <param name="feed">The <see cref="SyndicationFeed"/>.</param>
        public AtomSyndicationFeedActionResult(SyndicationFeed feed)
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
                return new Atom10FeedFormatter(this.Feed);
            }
        }
    }
}
