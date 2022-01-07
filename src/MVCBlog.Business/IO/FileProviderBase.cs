using Microsoft.Extensions.Hosting;

namespace MVCBlog.Business.IO;

public abstract class FileProviderBase : IFileProvider
{
    private readonly IHostEnvironment hostEnvironment;

    private readonly string baseDirectory;

    public FileProviderBase(IHostEnvironment hostEnvironment, string baseDirectory)
    {
        this.hostEnvironment = hostEnvironment;
        this.baseDirectory = baseDirectory;
    }

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        fileName = this.GetFilePath(fileName);

        if (File.Exists(fileName))
        {
            return await File.ReadAllBytesAsync(fileName);
        }
        else
        {
            throw new FileNotFoundException();
        }
    }

    public async Task AddFileAsync(string fileName, byte[] file)
    {
        string directory = this.GetDirectory();

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        fileName = this.GetFilePath(fileName);

        await File.WriteAllBytesAsync(fileName, file);
    }

    public Task DeleteFileAsync(string fileName)
    {
        fileName = this.GetFilePath(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        return Task.CompletedTask;
    }

    private string GetDirectory()
    {
        return Path.Combine(
            this.hostEnvironment.ContentRootPath,
            this.baseDirectory);
    }

    private string GetFilePath(string fileName)
    {
        if (fileName.Contains("../")
            || fileName.Contains("..\\")
            || fileName.IndexOfAny(Path.GetInvalidPathChars()) > -1)
        {
            throw new ArgumentException("Filename contains invalid path characters.", nameof(fileName));
        }

        return Path.Combine(
            this.GetDirectory(),
            fileName);
    }
}
