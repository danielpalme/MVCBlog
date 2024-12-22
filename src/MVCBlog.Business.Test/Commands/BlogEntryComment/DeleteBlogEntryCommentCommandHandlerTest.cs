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

        var blogEntry = new BlogEntry()
        {
            Header = "Test",
            Permalink = "Test",
            ShortContent = "Test"
        };

        this.comment = new BlogEntryComment()
        {
            BlogEntryId = blogEntry.Id,
            Name = "Test",
            Comment = "Test"
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
