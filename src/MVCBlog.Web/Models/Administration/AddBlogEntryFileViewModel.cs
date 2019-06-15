using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MVCBlog.Localization;

namespace MVCBlog.Web.Models.Administration
{
    public class AddBlogEntryFileViewModel
    {
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public Guid? BlogEntryId { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public IFormFile File { get; set; }
    }
}
