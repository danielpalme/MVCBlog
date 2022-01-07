namespace MVCBlog.Business.Commands;

public class IncrementBlogEntryFileCounterCommand
{
    public IncrementBlogEntryFileCounterCommand(Guid id)
    {
        this.Id = id;
    }

    public Guid Id { get; set; }
}