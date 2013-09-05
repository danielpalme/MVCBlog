using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents an Image.
    /// </summary>
    public class Image : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        [StringLength(4)]
        [Required]
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [NotMapped]
        public byte[] Data
        {
            get
            {
                return File.ReadAllBytes(this.FullPath);
            }

            set
            {
                File.WriteAllBytes(this.FullPath, value);
            }
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        [NotMapped]
        public string RelativePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ImagesPath"] + this.Id.ToString() + "." + this.Extension;
            }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        private string FullPath
        {
            get
            {
                var applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                return Path.Combine(applicationPath, this.RelativePath);
            }
        }

        /// <summary>
        /// Deletes the corresponding file.
        /// </summary>
        internal void DeleteData()
        {
            File.Delete(this.FullPath);
        }
    }
}
