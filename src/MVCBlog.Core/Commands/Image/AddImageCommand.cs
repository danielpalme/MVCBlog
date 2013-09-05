namespace MVCBlog.Core.Commands
{
    public class AddImageCommand
    {
        public string FileName { get; set; }

        public byte[] Data { get; set; }
    }
}
