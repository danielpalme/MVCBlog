using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using MVCBlog.Localization;

namespace MVCBlog.Data
{
    public class BlogEntry : EntityBase
    {
        private string header;

        [StringLength(150, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Header", ResourceType = typeof(Resources))]
        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
                this.UpdatePermalink();
            }
        }

        [StringLength(160, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Permalink { get; set; }

        [StringLength(1500, ErrorMessageResourceName = "Validation_MaxLength", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Validation_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ShortContent", ResourceType = typeof(Resources))]
        public string ShortContent { get; set; }

        [Display(Name = "Content", ResourceType = typeof(Resources))]
        public string Content { get; set; }

        [Display(Name = "Visible", ResourceType = typeof(Resources))]
        public bool Visible { get; set; }

        [Display(Name = "PublishDate", ResourceType = typeof(Resources))]
        public DateTimeOffset PublishDate { get; set; }

        [Display(Name = "UpdateDate", ResourceType = typeof(Resources))]
        public DateTimeOffset UpdateDate { get; set; }

        [Display(Name = "Visits", ResourceType = typeof(Resources))]
        public int Visits { get; set; }

        [Display(Name = "Author", ResourceType = typeof(Resources))]
        public string AuthorId { get; set; }

        [Display(Name = "Author", ResourceType = typeof(Resources))]
        public User Author { get; set; }

        public virtual ICollection<BlogEntryTag> Tags { get; set; }

        public virtual ICollection<BlogEntryComment> BlogEntryComments { get; set; }

        public virtual ICollection<BlogEntryFile> BlogEntryFiles { get; set; }

        /// <summary>
        /// Gets the full URL of the <see cref="BlogEntry"/>.
        /// </summary>
        [NotMapped]
        public string Url
        {
            get
            {
                return this.PublishDate.Year + "/" + this.PublishDate.Month + "/" + this.PublishDate.Day + "/" + this.Permalink;
            }
        }

        private void UpdatePermalink()
        {
            if (this.Header != null && this.Permalink == null)
            {
                this.Permalink = Regex.Replace(
                        this.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                        "[^\\w^-]",
                        string.Empty);
            }
        }
    }
}
