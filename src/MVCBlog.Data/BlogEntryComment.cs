using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data;

public class BlogEntryComment : EntityBase
{
    public BlogEntryComment(
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

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Comment), ResourceType = typeof(Resources))]
    public string Comment { get; set; }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Homepage), ResourceType = typeof(Resources))]
    public string? Homepage { get; set; }

    public bool AdminPost { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogEntryId { get; set; }

    public virtual BlogEntry? BlogEntry { get; set; }
}
