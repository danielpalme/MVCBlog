using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Administration
{
    /// <summary>
    /// Model for downloads.
    /// </summary>
    public class Downloads
    {
        /// <summary>
        /// Gets or sets the blog entries.
        /// </summary>
        public IEnumerable<BlogEntry> BlogEntries { get; set; }

        /// <summary>
        /// Gets or sets the feed statistics.
        /// </summary>
        /// <value>
        /// The feed statistics.
        /// </value>
        public IEnumerable<IGrouping<DateTime, FeedStatistic>> FeedStatistics { get; set; }
    }
}
