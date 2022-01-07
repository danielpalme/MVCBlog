using MVCBlog.Data;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Blog;

public class IndexViewModel
{
    public IndexViewModel(
        PagedResult<BlogEntry> entries,
        List<Tag> tags,
        List<BlogEntry> popularBlogEntries)
    {
        this.Entries = entries;
        this.Tags = tags;
        this.PopularBlogEntries = popularBlogEntries;
    }

    public PagedResult<BlogEntry> Entries { get; set; }

    public List<Tag> Tags { get; set; }

    public List<BlogEntry> PopularBlogEntries { get; set; }

    public string? Search { get; set; }

    public string? Tag { get; set; }
}
