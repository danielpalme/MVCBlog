using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Blog
{
    /// <summary>
    /// Model for a several <see cref="BlogEntry">BlogEntries</see>.
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        public BlogEntry[] Entries { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int? CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the currently selected tag.
        /// </summary>
        public string Tag { get; set; }
    }
}