namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryCommand
{
    public DeleteBlogEntryCommand(Guid id)
    {
        this.Id = id;
    }

    public Guid Id { get; set; }
}