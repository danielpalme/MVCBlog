using System;
using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class BlogEntryComment : EntityBase
    {
        [StringLength(50, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Name", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Comment", ResourceType = typeof(Resources))]
        public string Comment { get; set; }

        [StringLength(50, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Email", ResourceType = typeof(Resources))]
        public string Email { get; set; }

        [StringLength(100, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Homepage", ResourceType = typeof(Resources))]
        public string Homepage { get; set; }

        public bool AdminPost { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public Guid? BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }
    }
}
