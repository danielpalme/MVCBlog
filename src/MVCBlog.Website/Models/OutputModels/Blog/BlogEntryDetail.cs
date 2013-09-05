using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Blog
{
    /// <summary>
    /// Model for a single <see cref="BlogEntry"/>.
    /// </summary>
    public class BlogEntryDetail
    {
        /// <summary>
        /// Gets or sets the blog entry.
        /// </summary>
        public BlogEntry BlogEntry { get; set; }

        /// <summary>
        /// Gets or sets the blog entry.
        /// </summary>
        public BlogEntry[] RelatedBlogEntries { get; set; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return this.BlogEntry.Header;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the comments form should be visible.
        /// </summary>
        public bool HideNewCommentsForm { get; set; }
    }
}
