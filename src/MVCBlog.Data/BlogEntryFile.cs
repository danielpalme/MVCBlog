using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class BlogEntryFile : EntityBase
    {
        [Display(Name = "Name", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Display(Name = "Counter", ResourceType = typeof(Resources))]
        public int Counter { get; set; }

        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public Guid? BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }

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
