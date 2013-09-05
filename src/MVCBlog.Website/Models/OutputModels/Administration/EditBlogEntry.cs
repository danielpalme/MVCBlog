using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Administration
{
    /// <summary>
    /// Model for a editing a <see cref="BlogEntry"/>.
    /// </summary>
    public class EditBlogEntry
    {
        /// <summary>
        /// Gets or sets the blog entry.
        /// </summary>
        public BlogEntry BlogEntry { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        public IEnumerable<Image> Images { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public IEnumerable<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an existing <see cref="BlogEntry"/> is updated of a new one is created.
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Gets all <see cref="Tag">Tags</see> as comma separated <see cref="string"/>.
        /// </summary>
        public IHtmlString TagsAsJsonString
        {
            get
            {
                return MvcHtmlString.Create("[" + string.Join(",", this.Tags.Select(t => "\"" + t.Name + "\"").ToArray()) + "]");
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="Tag"/> at the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The name of the <see cref="Tag"/>.</returns>
        public string GetTagName(int index)
        {
            if (this.BlogEntry != null && this.BlogEntry.Tags != null && this.BlogEntry.Tags.Count > index)
            {
                return this.BlogEntry.Tags.ElementAt(index).Name;
            }
            else
            {
                return null;
            }
        }
    }
}
