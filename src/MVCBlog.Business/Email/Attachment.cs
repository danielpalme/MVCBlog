namespace MVCBlog.Business.Email;

public class Attachment
{
    public Attachment(string fileName, byte[] data)
    {
        this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        this.Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public string FileName { get; }

    public byte[] Data { get; }

    public override string ToString()
    {
        return this.FileName;
    }
}