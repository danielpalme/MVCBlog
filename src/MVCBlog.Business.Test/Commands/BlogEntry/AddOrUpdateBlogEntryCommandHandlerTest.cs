using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class AddOrUpdateBlogEntryCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    public AddOrUpdateBlogEntryCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
    }

    [Fact]
    public async Task AddBlogEntryCommand()
    {
        var sut = new AddOrUpdateBlogEntryCommandHandler(this.unitOfWork);

        await sut.HandleAsync(new AddOrUpdateBlogEntryCommand(
            new BlogEntry("Test", "test", "Test"),
            new[] { "Tag1" }));

        Assert.Single(this.unitOfWork.BlogEntries);
        Assert.Single(this.unitOfWork.Tags);
        Assert.Single(this.unitOfWork.BlogEntryTags);
    }

    [Fact]
    public async Task UpdateBlogEntryCommand()
    {
        var sut = new AddOrUpdateBlogEntryCommandHandler(this.unitOfWork);

        var blogEntry = new BlogEntry("Test - Test", "test", "Test");

        await sut.HandleAsync(new AddOrUpdateBlogEntryCommand(
            blogEntry,
            new[] { "Tag1", "Tag2" }));

        Assert.Single(this.unitOfWork.BlogEntries);
        Assert.Equal(2, this.unitOfWork.Tags.Count());
        Assert.Equal(2, this.unitOfWork.BlogEntryTags.Count());
        Assert.Equal("test-test", this.unitOfWork.BlogEntries.Single().Permalink);

        await sut.HandleAsync(new AddOrUpdateBlogEntryCommand(
            new BlogEntry("Test2", "test2", "Test2")
            {
                Id = blogEntry.Id
            },
            new[] { "Tag2", "Tag3" }));

        Assert.Single(this.unitOfWork.BlogEntries);
        Assert.Equal(3, this.unitOfWork.Tags.Count());
        Assert.Equal(2, this.unitOfWork.BlogEntryTags.Count());
        Assert.Equal("test2", this.unitOfWork.BlogEntries.Single().Permalink);
    }
}