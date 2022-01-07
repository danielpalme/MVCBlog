namespace MVCBlog.Business.Email;

public class EmbeddedImage
{
    public EmbeddedImage(string fileName, byte[] data, string contentId)
    {
        this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        this.Data = data ?? throw new ArgumentNullException(nameof(data));
        this.ContentId = contentId ?? throw new ArgumentNullException(nameof(contentId));
    }

    public string FileName { get; }

    public byte[] Data { get; }

    public string ContentId { get; }

    public override string ToString()
    {
        return this.FileName;
    }
}