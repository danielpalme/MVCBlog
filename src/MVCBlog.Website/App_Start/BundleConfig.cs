using System.Web.Optimization;

namespace MVCBlog.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/jquery.lightbox.js",
                    "~/Scripts/jquery.autocomplete.pack.js",
                    "~/Scripts/SyntaxHighlighter/shCore.js",
                    "~/Scripts/SyntaxHighlighter/shAutoloader.js",
                    "~/Scripts/custom.js",
                    "~/Scripts/jquery.lightbox.js"));

            bundles.Add(new StyleBundle("~/css/combined")
                .Include(
                    "~/Content/bootstrap/bootstrap.css",
                    "~/Content/custom.css",
                    "~/Content/lightbox.css",
                    "~/Content/jquery.autocomplete.css",
                    "~/Content/shCoreDefault.css"));
        }
    }
}