using System.IO;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// Extends the <see cref="WebFormView"/> to support localization.
    /// </summary>
    public class LocalizationWebFormView : WebFormView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationWebFormView"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        public LocalizationWebFormView(ControllerContext controllerContext, string viewPath)
            : base(controllerContext, viewPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationWebFormView"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="masterPath">The master path.</param>
        public LocalizationWebFormView(ControllerContext controllerContext, string viewPath, string masterPath)
            : base(controllerContext, viewPath, masterPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationWebFormView"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="masterPath">The master path.</param>
        /// <param name="viewPageActivator">The view page activator.</param>
        public LocalizationWebFormView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, masterPath, viewPageActivator)
        {
        }

        /// <summary>
        /// Renders the view to the response.
        /// </summary>
        /// <param name="viewContext">An object that encapsulates the information that is required in order to render the view, which includes the controller context, form context, the temporary data, and the view data for the associated view.</param>
        /// <param name="writer">The text writer object that is used to write HTML output.</param>
        /// <param name="instance">The view page instance.</param>
        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            viewContext.ViewData[ResourceExtensions.ViewPathKey] = this.ViewPath;

            base.RenderView(viewContext, writer, instance);
        }
    }
}