using System.Collections.Generic;
using MVCBlog.Data;

namespace MVCBlog.Web.Models.Administration
{
    public class EditBlogEntryViewModel
    {
        public BlogEntry BlogEntry { get; set; }

        public List<string> SelectedTagNames { get; set; }

        public List<Tag> AllTags { get; set; }

        public List<User> Authors { get; set; }
    }
}
