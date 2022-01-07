using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Moq;
using MVCBlog.Business.IO;
using Xunit;

namespace MVCBlog.Business.Test.IO;

public class FileProviderBaseTest : IDisposable
{
    private readonly FileProviderBase fileProvider;

    public FileProviderBaseTest()
    {
        this.fileProvider = new SampleFileProvider();
    }

    public void Dispose()
    {
        string directory = Path.Combine(Path.GetTempPath(), "test");
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task GetFileAsync_DoesNotExist_FileNotFoundException()
    {
        await Assert.ThrowsAsync<FileNotFoundException>(() => this.fileProvider.GetFileAsync("test.txt"));
    }

    [Fact]
    public async Task GetFileAsync_Exists()
    {
        await this.fileProvider.AddFileAsync("test.txt", new byte[] { 0x00 });

        var data = await this.fileProvider.GetFileAsync("test.txt");
        Assert.True(data.Length > 0);
    }

    [Fact]
    public async Task DeleteFileAsync()
    {
        await this.fileProvider.AddFileAsync("test.txt", new byte[] { 0x00 });

        await this.fileProvider.DeleteFileAsync("test.txt");

        await Assert.ThrowsAsync<FileNotFoundException>(() => this.fileProvider.GetFileAsync("test.txt"));
    }

    private class SampleFileProvider : FileProviderBase
    {
        private const string FilesBaseDirectory = "test";

        public SampleFileProvider()
            : base(GetHostEnvironment(), FilesBaseDirectory)
        {
        }

        private static IHostEnvironment GetHostEnvironment()
        {
            var mock = new Mock<IHostEnvironment>();
            mock.Setup(m => m.ContentRootPath).Returns(Path.GetTempPath());

            return mock.Object;
        }
    }
}
