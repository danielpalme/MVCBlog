using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.InputModels.Login
{
    /// <summary>
    /// Inputs in login form.
    /// </summary>
    public class LoginFormInput
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Required(ErrorMessage = "*")]
        [Display(Name = "Username", ResourceType = typeof(Properties.Common))]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Properties.Common))]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        [Display(Name = "RememberMe", ResourceType = typeof(Properties.Common))]
        public bool RememberMe { get; set; }
    }
}