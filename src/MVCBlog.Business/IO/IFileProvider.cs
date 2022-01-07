namespace MVCBlog.Business.IO;

public interface IFileProvider
{
    Task<byte[]> GetFileAsync(string fileName);

    Task AddFileAsync(string fileName, byte[] file);

    Task DeleteFileAsync(string fileName);
}

public interface IImageFileProvider : IFileProvider
{
}

public interface IBlogEntryFileFileProvider : IFileProvider
{
}