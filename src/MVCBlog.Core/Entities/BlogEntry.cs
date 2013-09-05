using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MVCBlog.Core.Resources;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents a BlogEntry.
    /// </summary>
    public class BlogEntry : EntityBase
    {
        /// <summary>
        /// The header.
        /// </summary>
        private string header;

        /// <summary>
        /// The publish date.
        /// </summary>
        private DateTime publishDate;

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        [StringLength(150)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "HeaderLabel", ResourceType = typeof(Labels))]
        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
                this.UpdateHeaderUrl();
            }
        }

        /// <summary>
        /// Gets or sets the header URL.
        /// </summary>
        [StringLength(160)]
        [Required(ErrorMessage = "*")]
        public string HeaderUrl { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        [StringLength(100)]
        [Display(Name = "AuthorLabel", ResourceType = typeof(Labels))]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the short content.
        /// </summary>
        [StringLength(1500)]
        [AllowHtml]
        [Required(ErrorMessage = "*")]
        [Display(Name = "ShortContentLabel", ResourceType = typeof(Labels))]
        public string ShortContent { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [AllowHtml]
        /* TODO: SQL CE does not support nvarchar(max). Therefore 'ntext' is used
           Comment OUT the following line if you are NOT using SQL CE! */
        [Column(TypeName = "ntext")] // Otherwise nvarchar(4000) is used by default
        [Display(Name = "ContentLabel", ResourceType = typeof(Labels))]
        public string Content { get; set; }

        public int Visits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BlogEntry"/> is visible.
        /// </summary>
        [Display(Name = "VisibleLabel", ResourceType = typeof(Labels))]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>The publish date.</value>
        [Display(Name = "PublishDateLabel", ResourceType = typeof(Labels))]
        public DateTime PublishDate
        {
            get
            {
                return this.publishDate;
            }

            set
            {
                this.publishDate = value;
                this.UpdateHeaderUrl();
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the blog entry comments.
        /// </summary>
        public virtual ICollection<BlogEntryComment> BlogEntryComments { get; set; }

        /// <summary>
        /// Gets or sets the blog entry pingbacks.
        /// </summary>
        public virtual ICollection<BlogEntryPingback> BlogEntryPingbacks { get; set; }

        /// <summary>
        /// Gets or sets the blog entry files.
        /// </summary>
        public virtual ICollection<BlogEntryFile> BlogEntryFiles { get; set; }

        /// <summary>
        /// Gets the URL / permalink of the <see cref="BlogEntry"/>.
        /// </summary>
        [NotMapped]
        public string Url
        {
            get
            {
                return this.Created.Year + "/" + this.Created.Month + "/" + this.Created.Day + "/" + this.HeaderUrl;
            }
        }

        private void UpdateHeaderUrl()
        {
            if (this.Header != null)
            {
                this.HeaderUrl = Regex.Replace(
                        this.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                        "[^\\w^-]",
                        string.Empty);
            }
        }
    }
}
