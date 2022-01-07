using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCBlog.Web.Infrastructure.Paging;

[HtmlTargetElement("th", Attributes = "sort-column,paged-result")]
public class SortLinkTagHelper : TagHelper
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public SortLinkTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public dynamic? PagedResult { get; set; }

    public string? SortColumn { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (this.PagedResult == null)
        {
            throw new InvalidOperationException("'PagedResult' must be not null.");
        }

        if (this.SortColumn == null)
        {
            throw new InvalidOperationException("'SortColumn' must be not null.");
        }

        string currentContent = (await output.GetChildContentAsync()).GetContent();

        string? url = this.httpContextAccessor.HttpContext?.Request.QueryString.Value;
        url = url.SetParameters(KeyValuePair.Create("skip", "0"));
        url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortColumn), this.SortColumn));

        if (this.PagedResult.Paging.SortColumn == this.SortColumn)
        {
            if (this.PagedResult.Paging.SortDirection == SortDirection.Ascending)
            {
                url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Descending.ToString()));
                output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</a><i class=\"fa fa-caret-down text-danger d-print-none ml-1\"></i>");
            }
            else
            {
                url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Ascending.ToString()));
                output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</a><i class=\"fa fa-caret-up text-danger d-print-none ml-1\"></i>");
            }
        }
        else
        {
            url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Ascending.ToString()));
            output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</i></a><i class=\"fa fa-caret-down d-print-none ml-1\"></i>");
        }
    }
}