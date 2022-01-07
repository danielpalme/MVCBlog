namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryCommentCommand
{
    public DeleteBlogEntryCommentCommand(Guid id)
    {
        this.Id = id;
    }

    public Guid Id { get; set; }
}