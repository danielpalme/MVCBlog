using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// Extends the <see cref="RazorView"/> to support localization.
    /// </summary>
    public class LocalizationRazorView : RazorView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRazorView"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="layoutPath">The layout path.</param>
        /// <param name="runViewStartPages">if set to <c>true</c> [run view start pages].</param>
        /// <param name="viewStartFileExtensions">The view start file extensions.</param>
        public LocalizationRazorView(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRazorView"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="layoutPath">The layout or master page.</param>
        /// <param name="runViewStartPages">A value that indicates whether view start files should be executed before the view.</param>
        /// <param name="viewStartFileExtensions">The set of extensions that will be used when looking up view start files.</param>
        /// <param name="viewPageActivator">The view page activator.</param>
        public LocalizationRazorView(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        /// <summary>
        /// Renders the specified view context by using the specified writer and <see cref="T:System.Web.Mvc.WebViewPage"/> instance.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="writer">The writer that is used to render the view to the response.</param>
        /// <param name="instance">The <see cref="T:System.Web.Mvc.WebViewPage"/> instance.</param>
        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            viewContext.ViewData[ResourceExtensions.ViewPathKey] = this.ViewPath;

            base.RenderView(viewContext, writer, instance);
        }
    }
}