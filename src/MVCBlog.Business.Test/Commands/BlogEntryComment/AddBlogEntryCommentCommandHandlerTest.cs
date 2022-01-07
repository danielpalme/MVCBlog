using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.Email;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class AddBlogEntryCommentCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly Mock<INotificationService> notificationsServiceMock;

    private readonly BlogEntry blogEntry;

    public AddBlogEntryCommentCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
        this.notificationsServiceMock = new Mock<INotificationService>();

        this.blogEntry = new BlogEntry("Test", "test", "Test");

        this.unitOfWork.BlogEntries.Add(this.blogEntry);
        this.unitOfWork.SaveChanges();
    }

    [Fact]
    public async Task AddBlogEntryComment()
    {
        var sut = new AddBlogEntryCommentCommandHandler(
            this.unitOfWork,
            this.notificationsServiceMock.Object,
            Options.Create(new BlogSettings()
            {
                NotifyOnNewComments = true,
                NotifyOnNewCommentsEmail = "test@test.de"
            }));

        await sut.HandleAsync(new AddBlogEntryCommentCommand(new BlogEntryComment("Test", "Test")
        {
            BlogEntryId = this.blogEntry.Id
        }));

        Assert.Single(this.unitOfWork.BlogEntryComments);

        this.notificationsServiceMock.Verify(i => i.SendNotificationAsync(It.IsAny<Message>()), Times.Once);
    }
}
