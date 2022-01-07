using System.ComponentModel.DataAnnotations;
using MVCBlog.Data;
using MVCBlog.Localization;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Administration;

public class ImagesViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<Image>? Images { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public IFormFile? Image { get; set; }
}
