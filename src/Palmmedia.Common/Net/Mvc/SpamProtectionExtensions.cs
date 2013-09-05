using System;
using System.Web;
using System.Web.Mvc;

namespace Palmmedia.Common.Net.Mvc
{
    /// <summary>
    /// Extends the <see cref="HtmlHelper"/> to create a hidden field containing the 'SpamProtectionTimeStamp' used by <see cref="SpamProtectionAttribute"/>.
    /// </summary>
    public static class SpamProtectionExtensions
    {
        /// <summary>
        /// Create a hidden field containing the 'SpamProtectionTimeStamp'.
        /// </summary>
        /// <param name="helper">The <see cref="HtmlHelper"/>.</param>
        /// <returns>The hidden field containing the 'SpamProtectionTimeStamp'.</returns>
        public static IHtmlString SpamProtectionTimeStamp(this HtmlHelper helper)
        {
            var builder1 = new TagBuilder("input");
            builder1.MergeAttribute("name", "SpamProtectionTimeStamp");
            builder1.MergeAttribute("type", "hidden");
            builder1.MergeAttribute("value", ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString());

            var builder2 = new TagBuilder("input");
            builder2.MergeAttribute("name", "website");
            builder2.MergeAttribute("type", "text");
            builder2.MergeAttribute("style", "display:inline;height:1px;left:-10000px;overflow:hidden;position:absolute;width:1px;");
            builder2.MergeAttribute("value", string.Empty);
            return MvcHtmlString.Create(builder1.ToString(TagRenderMode.SelfClosing) + builder2.ToString(TagRenderMode.SelfClosing));
        }
    }
}