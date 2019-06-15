using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Web.Models.Blog
{
    public class BlogEntryCommentViewModel
    {
        [StringLength(50, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Name", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        [StringLength(50, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Email", ResourceType = typeof(Resources))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(100, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Homepage", ResourceType = typeof(Resources))]
        [DataType(DataType.Url)]
        public string Homepage { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Comment", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}