namespace MVCBlog.Core.Commands
{
    public class UpdateCommand<T> where T : MVCBlog.Core.Entities.EntityBase
    {
        public T Entity { get; set; }
    }
}
