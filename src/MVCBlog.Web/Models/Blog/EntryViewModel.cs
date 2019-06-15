using System.Collections.Generic;
using MVCBlog.Data;

namespace MVCBlog.Web.Models.Blog
{
    public class EntryViewModel
    {
        public BlogEntry BlogEntry { get; set; }

        public List<BlogEntry> RelatedBlogEntries { get; set; }

        public BlogEntryCommentViewModel Comment { get; set; }
    }
}
