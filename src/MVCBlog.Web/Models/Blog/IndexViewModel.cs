using System.Collections.Generic;
using MVCBlog.Data;
using MVCBlog.Web.Infrastructure.Paging;

namespace MVCBlog.Web.Models.Blog
{
    public class IndexViewModel
    {
        public PagedResult<BlogEntry> Entries { get; set; }

        public List<Tag> Tags { get; set; }

        public List<BlogEntry> PopularBlogEntries { get; set; }

        public string Search { get; set; }

        public string Tag { get; set; }
    }
}