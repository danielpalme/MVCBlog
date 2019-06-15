using System.Linq;
using System.Threading.Tasks;
using Moq;
using MVCBlog.Business.Commands;
using MVCBlog.Business.IO;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands
{
    public class AddImageCommandHandlerTest
    {
        private readonly EFUnitOfWork unitOfWork;

        private readonly Mock<IImageFileProvider> imageFileProviderMock;

        public AddImageCommandHandlerTest()
        {
            this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();
            this.imageFileProviderMock = new Mock<IImageFileProvider>();
        }

        [Fact]
        public async Task AddImage()
        {
            var sut = new AddImageCommandHandler(this.unitOfWork, this.imageFileProviderMock.Object);
            await sut.HandleAsync(new AddImageCommand()
            {
                Data = new byte[0],
                FileName = "path\\test.png"
            });

            var images = this.unitOfWork.Images.Where(i => i.Name == "test.png").ToList();

            Assert.Single(images);
            Assert.Equal($"{images[0].Id}.png", images[0].Path);

            this.imageFileProviderMock.Verify(i => i.AddFileAsync($"{images[0].Id}.png", It.IsAny<byte[]>()), Times.Once);
        }
    }
}
