using System.Threading.Tasks;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class DeleteBlogEntryFileCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly Mock<IBlogEntryFileFileProvider> blogEntryFileFileProvider;

    private readonly BlogEntryFile file;

    public DeleteBlogEntryFileCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
        this.blogEntryFileFileProvider = new Mock<IBlogEntryFileFileProvider>();

        var blogEntry = new BlogEntry("Test", "test", "Test");

        this.file = new BlogEntryFile("test.pdf")
        {
            BlogEntryId = blogEntry.Id
        };
        this.unitOfWork.BlogEntries.Add(blogEntry);
        this.unitOfWork.BlogEntryFiles.Add(this.file);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.BlogEntryFiles);
    }

    [Fact]
    public async Task DeleteBlogEntryFile()
    {
        var sut = new DeleteBlogEntryFileCommandHandler(this.unitOfWork, this.blogEntryFileFileProvider.Object);
        await sut.HandleAsync(new DeleteBlogEntryFileCommand(this.file.Id));

        Assert.Empty(this.unitOfWork.BlogEntryFiles);

        this.blogEntryFileFileProvider.Verify(i => i.DeleteFileAsync($"{this.file.Id}.pdf"), Times.Once);
    }
}
