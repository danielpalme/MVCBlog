using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Localization;

namespace MVCBlog.Data;

public class Image : EntityBase
{
    public Image(
       string name)
    {
        this.Name = name;
    }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

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