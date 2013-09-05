using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents a Tag.
    /// </summary>
    public class Tag : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(30)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the blog entries.
        /// </summary>
        public virtual ICollection<BlogEntry> BlogEntries { get; set; }
    }
}
