using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class Image : EntityBase
    {
        [StringLength(50, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
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
}
