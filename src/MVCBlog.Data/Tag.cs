using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Localization;

namespace MVCBlog.Data;

[Index(nameof(Name), IsUnique = true)]
public class Tag : EntityBase
{
    [StringLength(30, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public required string Name { get; set; }

    public virtual ICollection<BlogEntryTag>? BlogEntries { get; set; }
}