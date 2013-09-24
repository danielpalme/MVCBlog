using System.Web.Mvc;
using System.Web.Security;
using MVCBlog.Website.Models.InputModels.Login;

namespace MVCBlog.Website.Controllers
{
    /// <summary>
    /// Controller for user authentication tasks.
    /// </summary>
    public partial class LoginController : Controller
    {
        /// <summary>
        /// Shows the login form.
        /// </summary>
        /// <returns>View showing the login form.</returns>
        public virtual ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.RedirectToAction(MVC.Blog.Index());
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Validates the login and redirects to the return URL.
        /// </summary>
        /// <param name="loginFormInput">The form input.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Redirect to the return URL.</returns>
        [Palmmedia.Common.Net.Mvc.ReferrerAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Index(LoginFormInput loginFormInput, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            if (!FormsAuthentication.Authenticate(loginFormInput.Username, loginFormInput.Password))
            {
                ModelState.AddModelError("login", Properties.Common.LoginFailure);
                return this.View();
            }

            FormsAuthentication.SetAuthCookie(loginFormInput.Username, loginFormInput.RememberMe);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(MVC.Login.Index());
            }
        }

        /// <summary>
        /// Shows the logout form.
        /// </summary>
        /// <returns>View showing the logout form.</returns>
        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction(MVC.Login.Index());
        }
    }
}
