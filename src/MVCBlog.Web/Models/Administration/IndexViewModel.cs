using MVCBlog.Data;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Administration;

public class IndexViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<BlogEntry>? BlogEntries { get; set; }
}
