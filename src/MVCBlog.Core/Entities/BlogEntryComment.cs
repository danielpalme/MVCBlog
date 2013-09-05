using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCBlog.Core.Resources;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents a BlogEntryComment.
    /// </summary>
    public class BlogEntryComment : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "Name", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "NameLabel", ResourceType = typeof(Labels))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [StringLength(2500)]
        [Required(ErrorMessageResourceName = "Comment", ErrorMessageResourceType = typeof(Validation))]
        [AllowHtml]
        [Display(Name = "CommentLabel", ResourceType = typeof(Labels))]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(50)]
        [Display(Name = "EmailLabel", ResourceType = typeof(Labels))]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        [RegularExpression(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessageResourceName = "Homepage", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(100)]
        [Display(Name = "HomepageLabel", ResourceType = typeof(Labels))]
        public string Homepage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [admin post].
        /// </summary>
        public bool AdminPost { get; set; }

        /// <summary>
        /// Gets or sets the blog entry id.
        /// </summary>
        public Guid BlogEntryId { get; set; }

        /// <summary>
        /// Gets or sets the blog entry.
        /// </summary>
        public virtual BlogEntry BlogEntry { get; set; }
    }
}
