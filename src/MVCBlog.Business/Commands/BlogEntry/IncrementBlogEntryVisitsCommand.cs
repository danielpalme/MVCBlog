using System;

namespace MVCBlog.Business.Commands
{
    public class IncrementBlogEntryVisitsCommand
    {
        public Guid Id { get; set; }
    }
}
