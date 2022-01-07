namespace MVCBlog.Business.Commands;

public class AddOrUpdateBlogEntryFileCommand
{
    public AddOrUpdateBlogEntryFileCommand(
        string fileName,
        byte[] data,
        Guid blogEntryId)
    {
        this.FileName = fileName;
        this.Data = data;
        this.BlogEntryId = blogEntryId;
    }

    public string FileName { get; set; }

    public byte[] Data { get; set; }

    public Guid BlogEntryId { get; set; }
}