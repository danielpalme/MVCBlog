using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MVCBlog.Business.IO
{
    public abstract class FileProviderBase : IFileProvider
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private readonly string baseDirectory;

        public FileProviderBase(IHostingEnvironment hostingEnvironment, string baseDirectory)
        {
            this.hostingEnvironment = hostingEnvironment;
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
                this.hostingEnvironment.ContentRootPath,
                this.baseDirectory);
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(
                this.GetDirectory(),
                fileName);
        }
    }
}