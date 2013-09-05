using System;

namespace MVCBlog.Core.Commands
{
    public class DeleteCommand<T> where T : MVCBlog.Core.Entities.EntityBase
    {
        public Guid Id { get; set; }
    }
}
