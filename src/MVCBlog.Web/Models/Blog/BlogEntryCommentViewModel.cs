using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Web.Models.Blog;

public class BlogEntryCommentViewModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BlogEntryCommentViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public BlogEntryCommentViewModel(
        string name,
        string comment)
    {
        this.Name = name;
        this.Comment = comment;
    }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    public string Name { get; set; }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Homepage), ResourceType = typeof(Resources))]
    [DataType(DataType.Url)]
    public string? Homepage { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Comment), ResourceType = typeof(Resources))]
    [DataType(DataType.MultilineText)]
    public string Comment { get; set; }
}
