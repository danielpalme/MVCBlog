namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryFileCommand
{
    public DeleteBlogEntryFileCommand(Guid id)
    {
        this.Id = id;
    }

    public Guid Id { get; set; }
}