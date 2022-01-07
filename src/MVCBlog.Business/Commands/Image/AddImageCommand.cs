namespace MVCBlog.Business.Commands;

public class AddImageCommand
{
    public AddImageCommand(
        string fileName,
        byte[] data)
    {
        this.FileName = fileName;
        this.Data = data;
    }

    public string FileName { get; set; }

    public byte[] Data { get; set; }
}