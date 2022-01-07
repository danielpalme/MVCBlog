using Markdig;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCBlog.Web.Infrastructure.Mvc;

[HtmlTargetElement("markdown")]
public class MarkdownTagHelper : TagHelper
{
    [HtmlAttributeName("markdown")]
    public ModelExpression? Markdown { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);

        string? content = null;
        if (this.Markdown != null)
        {
            content = this.Markdown.Model?.ToString();
        }

        if (content == null)
        {
            content = (await output.GetChildContentAsync(NullHtmlEncoder.Default))
                            .GetContent(NullHtmlEncoder.Default);
        }

        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        content = content.Trim('\n', '\r');

        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        string html = Markdig.Markdown.ToHtml(content, pipeline);

        output.TagName = null;  // Remove the <markdown> element
        output.Content.SetHtmlContent(html);
    }
}
