using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCBlog.Web.Infrastructure.Mvc;

[HtmlTargetElement("form")]
public class SpamProtectionTagHelper : TagHelper
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? Method { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (this.Method != null && !string.Equals(this.Method, "get", StringComparison.OrdinalIgnoreCase))
        {
            output.PostContent.AppendHtml("<input type=\"text\" style=\"display:inline;height:1px;left:-10000px;overflow:hidden;position:absolute;width:1px;\" name=\"Website\" value=\"\" />");
            output.PostContent.AppendHtml($"<input type=\"hidden\" name=\"SpamProtectionTimeStamp\" value=\"{DateTimeOffset.Now.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)}\" />");
        }

        return Task.CompletedTask;
    }
}
