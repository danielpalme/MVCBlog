using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Base class all entities derive from this class.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        public EntityBase()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        public DateTime? Modified { get; set; }
    }
}
