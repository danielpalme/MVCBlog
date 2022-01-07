using System.Threading.Tasks;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class DeleteBlogEntryCommentCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly BlogEntryComment comment;

    public DeleteBlogEntryCommentCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();

        var blogEntry = new BlogEntry("Test", "test", "Test");

        this.comment = new BlogEntryComment("Test", "Test")
        {
            BlogEntryId = blogEntry.Id
        };
        this.unitOfWork.BlogEntries.Add(blogEntry);
        this.unitOfWork.BlogEntryComments.Add(this.comment);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.BlogEntryComments);
    }

    [Fact]
    public async Task DeleteBlogEntryComment()
    {
        var sut = new DeleteBlogEntryCommentCommandHandler(this.unitOfWork);
        await sut.HandleAsync(new DeleteBlogEntryCommentCommand(this.comment.Id));

        Assert.Empty(this.unitOfWork.BlogEntryComments);
    }
}
