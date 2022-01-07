using System.Linq;
using System.Threading.Tasks;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class AddOrUpdateBlogEntryFileCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly Mock<IBlogEntryFileFileProvider> blogEntryFileFileProvider;

    private readonly BlogEntry blogEntry;

    public AddOrUpdateBlogEntryFileCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
        this.blogEntryFileFileProvider = new Mock<IBlogEntryFileFileProvider>();

        this.blogEntry = new BlogEntry("Test", "test", "Test");

        this.unitOfWork.BlogEntries.Add(this.blogEntry);
        this.unitOfWork.SaveChanges();
    }

    [Fact]
    public async Task AddBlogEntryFile()
    {
        var sut = new AddOrUpdateBlogEntryFileCommandHandler(this.unitOfWork, this.blogEntryFileFileProvider.Object);
        await sut.HandleAsync(new AddOrUpdateBlogEntryFileCommand("path\\test.pdf", new byte[0], this.blogEntry.Id));

        var files = this.unitOfWork.BlogEntryFiles.Where(i => i.Name == "test.pdf").ToList();

        Assert.Single(files);
        Assert.Equal($"{files[0].Id}.pdf", files[0].Path);

        this.blogEntryFileFileProvider.Verify(i => i.AddFileAsync($"{files[0].Id}.pdf", It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBlogEntryFile()
    {
        var sut = new AddOrUpdateBlogEntryFileCommandHandler(this.unitOfWork, this.blogEntryFileFileProvider.Object);
        await sut.HandleAsync(new AddOrUpdateBlogEntryFileCommand("path\\test.pdf", new byte[0], this.blogEntry.Id));

        await sut.HandleAsync(new AddOrUpdateBlogEntryFileCommand("path\\test.pdf", new byte[0], this.blogEntry.Id));

        var files = this.unitOfWork.BlogEntryFiles.Where(i => i.Name == "test.pdf").ToList();

        Assert.Single(files);
        Assert.Equal($"{files[0].Id}.pdf", files[0].Path);

        this.blogEntryFileFileProvider.Verify(i => i.AddFileAsync($"{files[0].Id}.pdf", It.IsAny<byte[]>()), Times.Exactly(2));
    }
}
