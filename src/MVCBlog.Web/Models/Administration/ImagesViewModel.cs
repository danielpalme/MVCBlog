using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MVCBlog.Data;
using MVCBlog.Localization;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Administration
{
    public class ImagesViewModel
    {
        public string SearchTerm { get; set; }

        public PagedResult<Image> Images { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public IFormFile Image { get; set; }
    }
}
