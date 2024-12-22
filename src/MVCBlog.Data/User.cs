using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MVCBlog.Localization;

namespace MVCBlog.Data;

public class User : IdentityUser
{
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.FirstName), ResourceType = typeof(Resources))]
    public required string FirstName { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.LastName), ResourceType = typeof(Resources))]
    public required string LastName { get; set; }

    public virtual ICollection<BlogEntry>? BlogEntries { get; set; }

    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName}";
    }
}