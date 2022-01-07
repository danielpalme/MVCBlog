using System.Threading.Tasks;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class DeleteBlogEntryCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly Mock<IBlogEntryFileFileProvider> blogEntryFileFileProvider;

    private readonly BlogEntry blogEntry;

    private readonly BlogEntryFile file;

    public DeleteBlogEntryCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
        this.blogEntryFileFileProvider = new Mock<IBlogEntryFileFileProvider>();

        this.blogEntry = new BlogEntry("test", "Test", "Test");

        this.file = new BlogEntryFile("test.pdf")
        {
            BlogEntryId = this.blogEntry.Id
        };
        this.unitOfWork.BlogEntries.Add(this.blogEntry);
        this.unitOfWork.BlogEntryFiles.Add(this.file);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.BlogEntryFiles);
    }

    [Fact]
    public async Task DeleteBlogEntry()
    {
        var sut = new DeleteBlogEntryCommandHandler(this.unitOfWork, this.blogEntryFileFileProvider.Object);
        await sut.HandleAsync(new DeleteBlogEntryCommand(this.blogEntry.Id));

        Assert.Empty(this.unitOfWork.BlogEntries);
        Assert.Empty(this.unitOfWork.BlogEntryFiles);

        this.blogEntryFileFileProvider.Verify(i => i.DeleteFileAsync($"{this.file.Id}.pdf"), Times.Once);
    }
}
