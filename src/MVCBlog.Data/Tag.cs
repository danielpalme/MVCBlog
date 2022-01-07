using System.ComponentModel.DataAnnotations;
using MVCBlog.Localization;

namespace MVCBlog.Data;

public class Tag : EntityBase
{
    public Tag(
       string name)
    {
        this.Name = name;
    }

    [StringLength(30, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

    public virtual ICollection<BlogEntryTag>? BlogEntries { get; set; }
}