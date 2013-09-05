using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents a BlogEntryPingback.
    /// </summary>
    public class BlogEntryPingback : EntityBase
    {
        [Required]
        public string Homepage { get; set; }

        /// <summary>
        /// Gets or sets the blog entry id.
        /// </summary>
        [Required]
        public Guid? BlogEntryId { get; set; }

        /// <summary>
        /// Gets or sets the blog entry.
        /// </summary>
        public virtual BlogEntry BlogEntry { get; set; }
    }
}
