using System;
using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            this.Id = Guid.NewGuid();
            this.CreatedOn = DateTimeOffset.UtcNow;
        }

        [Display(Name = "Id", ResourceType = typeof(Resources))]
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "CreatedOn", ResourceType = typeof(Resources))]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
