using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Localization;

namespace MVCBlog.Data;

[Index(nameof(Permalink), IsUnique = true)]
public class BlogEntry : EntityBase
{
    [StringLength(150, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Header), ResourceType = typeof(Resources))]
    public required string Header { get; set; }

    [StringLength(160, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public required string Permalink { get; set; }

    [StringLength(1500, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.ShortContent), ResourceType = typeof(Resources))]
    public required string ShortContent { get; set; }

    [Display(Name = nameof(Resources.Content), ResourceType = typeof(Resources))]
    public string? Content { get; set; }

    [Display(Name = nameof(Resources.Visible), ResourceType = typeof(Resources))]
    public bool Visible { get; set; }

    [Display(Name = nameof(Resources.PublishDate), ResourceType = typeof(Resources))]
    public DateTimeOffset PublishDate { get; set; }

    [Display(Name = nameof(Resources.UpdateDate), ResourceType = typeof(Resources))]
    public DateTimeOffset UpdateDate { get; set; }

    [Display(Name = nameof(Resources.Visits), ResourceType = typeof(Resources))]
    public int Visits { get; set; }

    [Display(Name = nameof(Resources.Author), ResourceType = typeof(Resources))]
    public string? AuthorId { get; set; }

    [Display(Name = nameof(Resources.Author), ResourceType = typeof(Resources))]
    public User? Author { get; set; }

    public virtual ICollection<BlogEntryTag>? Tags { get; set; }

    public virtual ICollection<BlogEntryComment>? BlogEntryComments { get; set; }

    public virtual ICollection<BlogEntryFile>? BlogEntryFiles { get; set; }

    /// <summary>
    /// Gets the full URL of the <see cref="BlogEntry"/>.
    /// </summary>
    [NotMapped]
    public string Url
    {
        get
        {
            return $"{this.PublishDate.Year}/{this.PublishDate.Month}/{this.PublishDate.Day}/{this.Permalink}";
        }
    }
}
