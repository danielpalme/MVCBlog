using System;
using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class BlogEntryTag
    {
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public Guid? BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public Guid? TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
