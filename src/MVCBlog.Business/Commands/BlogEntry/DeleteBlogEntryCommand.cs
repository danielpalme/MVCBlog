using System;

namespace MVCBlog.Business.Commands
{
    public class DeleteBlogEntryCommand
    {
        public Guid Id { get; set; }
    }
}
