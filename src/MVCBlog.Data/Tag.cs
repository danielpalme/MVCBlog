using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class Tag : EntityBase
    {
        [StringLength(30, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Name { get; set; }

        public virtual ICollection<BlogEntryTag> BlogEntries { get; set; }
    }
}
