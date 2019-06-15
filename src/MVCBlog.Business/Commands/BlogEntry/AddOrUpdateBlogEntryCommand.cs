using System.Collections.Generic;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands
{
    public class AddOrUpdateBlogEntryCommand
    {
        public BlogEntry Entity { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
