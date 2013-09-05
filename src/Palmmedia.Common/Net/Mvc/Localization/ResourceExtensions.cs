using System.Globalization;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// Extends the <see cref="HtmlHelper"/> and <see cref="Controller">Controllers</see> by localization features.
    /// Local and global resources can be retrieved in Views and Controllers by using these extension methods.
    /// </summary>
    public static class ResourceExtensions
    {   
        /// <summary>
        /// The ViewPathKey.
        /// </summary>
        internal const string ViewPathKey = "__ViewPath__";

        /// <summary>
        /// Resolves the (global) resource with the given expression.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The resource.</returns>
        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            ResourceExpressionFields fields = GetResourceFields(expression, "~/");
            return GetGlobalResource(fields, args);
        }

        /// <summary>
        /// Resolves the resource with the given expression.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="HtmlHelper"/>.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The resource.</returns>
        public static string Resource(this HtmlHelper htmlHelper, string expression, params object[] args)
        {
            string path = (string)htmlHelper.ViewData[ViewPathKey];
            if (string.IsNullOrEmpty(path))
            {
                path = "~/";
            }

            ResourceExpressionFields fields = GetResourceFields(expression, path);
            if (!string.IsNullOrEmpty(fields.ClassKey))
            {
                return GetGlobalResource(fields, args);
            }

            return GetLocalResource(path, fields, args);
        }

        /// <summary>
        /// Gets a local resource.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The local resource.</returns>
        private static string GetLocalResource(string path, ResourceExpressionFields fields, object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, (string)HttpContext.GetLocalResourceObject(path, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        /// <summary>
        /// Gets a global resource.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The global resource.</returns>
        private static string GetGlobalResource(ResourceExpressionFields fields, object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, (string)HttpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        /// <summary>
        /// Gets a resource fields.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>The resource fields.</returns>
        private static ResourceExpressionFields GetResourceFields(string expression, string virtualPath)
        {
            var context = new ExpressionBuilderContext(virtualPath);
            var builder = new ResourceExpressionBuilder();
            return (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);
        }
    }
}