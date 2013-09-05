using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// <see cref="RazorViewEngine"/> that uses <see cref="LocalizationRazorView"/>.
    /// </summary>
    /// <example>
    /// Use this snippet to register the <see cref="LocalizationRazorViewEngine"/> in Global.asax:
    /// <code>
    /// ViewEngines.Engines.Clear();
    /// ViewEngines.Engines.Add(new Palmmedia.Common.Net.Mvc.Localization.LocalizationRazorViewEngine());
    /// </code>
    /// </example>
    public class LocalizationRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRazorViewEngine"/> class.
        /// </summary>
        public LocalizationRazorViewEngine()
            : base()
        {
            this.ViewStartFileExtensions = new[]
            {
                "cshtml",
                "vbhtml",
            };
        }

        /// <summary>
        /// Gets or sets the view start file extensions.
        /// </summary>
        public string[] ViewStartFileExtensions
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a partial view using the specified controller context and partial path.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialPath">The path to the partial view.</param>
        /// <returns>The partial view.</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new LocalizationRazorView(
                controllerContext,
                partialPath,
                layoutPath: null,
                runViewStartPages: false,
                viewStartFileExtensions: this.ViewStartFileExtensions);
        }

        /// <summary>
        /// Creates a view by using the specified controller context and the paths of the view and master view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The path to the view.</param>
        /// <param name="masterPath">The path to the master view.</param>
        /// <returns>The view.</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var view = new LocalizationRazorView(
                controllerContext,
                viewPath,
                layoutPath: masterPath,
                runViewStartPages: true,
                viewStartFileExtensions: this.ViewStartFileExtensions);

            return view;
        }
    }
}