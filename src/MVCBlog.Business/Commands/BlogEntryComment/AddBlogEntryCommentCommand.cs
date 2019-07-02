using MVCBlog.Data;

namespace MVCBlog.Business.Commands
{
    public class AddBlogEntryCommentCommand
    {
        public BlogEntryComment Entity { get; set; }

        public string Referer { get; set; }
    }
}
