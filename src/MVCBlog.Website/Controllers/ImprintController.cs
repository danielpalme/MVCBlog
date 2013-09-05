using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    /// <summary>
    /// Controller for the imprint.
    /// </summary>
    public partial class ImprintController : Controller
    {
        /// <summary>
        /// Shows the imprint.
        /// </summary>
        /// <returns>The imprint view.</returns>
        public virtual ActionResult Index()
        {
            return this.View();
        }
    }
}
