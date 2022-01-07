using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Localization;

namespace MVCBlog.Data;

public class BlogEntryFile : EntityBase
{
    public BlogEntryFile(
       string name)
    {
        this.Name = name;
    }

    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

    [Display(Name = nameof(Resources.Counter), ResourceType = typeof(Resources))]
    public int Counter { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogEntryId { get; set; }

    public virtual BlogEntry? BlogEntry { get; set; }

    [NotMapped]
    public string Path
    {
        get
        {
            string extension = this.Name.Substring(this.Name.LastIndexOf('.') + 1);
            return $"{this.Id}.{extension}";
        }
    }
}
