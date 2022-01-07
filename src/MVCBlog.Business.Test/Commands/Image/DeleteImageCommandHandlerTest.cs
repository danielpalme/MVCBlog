using System.Threading.Tasks;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands;

public class DeleteImageCommandHandlerTest
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly Mock<IImageFileProvider> imageFileProviderMock;

    private readonly Image image;

    public DeleteImageCommandHandlerTest()
    {
        this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
        this.imageFileProviderMock = new Mock<IImageFileProvider>();

        this.image = new Image("path\\test.png");

        this.unitOfWork.Images.Add(this.image);
        this.unitOfWork.SaveChanges();

        Assert.Single(this.unitOfWork.Images);
    }

    [Fact]
    public async Task DeleteImage()
    {
        var sut = new DeleteImageCommandHandler(this.unitOfWork, this.imageFileProviderMock.Object);
        await sut.HandleAsync(new DeleteImageCommand(this.image.Id));

        Assert.Empty(this.unitOfWork.Images);

        this.imageFileProviderMock.Verify(i => i.DeleteFileAsync($"{this.image.Id}.png"), Times.Once);
    }
}
