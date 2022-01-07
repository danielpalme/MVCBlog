using System;
using System.Threading.Tasks;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class IncrementBlogEntryVisitsCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly BlogEntry blogEntry;

    public IncrementBlogEntryVisitsCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();

        this.blogEntry = new BlogEntry("Test", "test", "Test");

        this.unitOfWork.BlogEntries.Add(this.blogEntry);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.BlogEntries);
    }

    [Fact]
    public async Task IncrementBlogEntryFileCounter()
    {
        var sut = new IncrementBlogEntryVisitsCommandHandler(this.unitOfWork);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.HandleAsync(new IncrementBlogEntryVisitsCommand(this.blogEntry.Id)));
    }
}
