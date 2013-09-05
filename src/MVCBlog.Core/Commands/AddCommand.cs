namespace MVCBlog.Core.Commands
{
    public class AddCommand<T> where T : MVCBlog.Core.Entities.EntityBase
    {
        public T Entity { get; set; }
    }
}
