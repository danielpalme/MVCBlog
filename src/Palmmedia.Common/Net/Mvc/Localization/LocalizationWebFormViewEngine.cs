using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// <see cref="WebFormViewEngine"/> that uses <see cref="LocalizationWebFormView"/>.
    /// </summary>
    /// <example>
    /// Use this snippet to register the <see cref="WebFormViewEngine"/> in Global.asax:
    /// <code>
    /// ViewEngines.Engines.Clear();
    /// ViewEngines.Engines.Add(new Palmmedia.Common.Net.Mvc.Localization.LocalizationWebFormViewEngine());
    /// </code>
    /// </example>
    public class LocalizationWebFormViewEngine : WebFormViewEngine
    {
        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="masterPath">The master path.</param>
        /// <returns>The view.</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new LocalizationWebFormView(controllerContext, viewPath, masterPath);
        }

        /// <summary>
        /// Creates the partial view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialPath">The partial path.</param>
        /// <returns>The partial view.</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new LocalizationWebFormView(controllerContext, partialPath, null);
        }
    }
}