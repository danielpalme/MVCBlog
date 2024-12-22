using System;
using System.Threading.Tasks;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class IncrementBlogEntryFileCounterCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly BlogEntryFile file;

    public IncrementBlogEntryFileCounterCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();

        var blogEntry = new BlogEntry()
        {
            Header = "Test",
            Permalink = "Test",
            ShortContent = "Test"
        };

        this.file = new BlogEntryFile()
        {
            BlogEntryId = blogEntry.Id,
            Name = "test.png"
        };
        this.unitOfWork.BlogEntries.Add(blogEntry);
        this.unitOfWork.BlogEntryFiles.Add(this.file);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.BlogEntryFiles);
    }

    [Fact]
    public async Task IncrementBlogEntryFileCounter()
    {
        var sut = new IncrementBlogEntryFileCounterCommandHandler(this.unitOfWork);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
         sut.HandleAsync(new IncrementBlogEntryFileCounterCommand(this.file.Id)));
    }
}
