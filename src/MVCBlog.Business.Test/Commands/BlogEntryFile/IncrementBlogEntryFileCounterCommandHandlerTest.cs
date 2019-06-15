using System;
using System.Threading.Tasks;
using MVCBlog.Business.Commands;
using MVCBlog.Data;
using Xunit;

namespace MVCBlog.Business.Test.Commands
{
    public class IncrementBlogEntryFileCounterCommandHandlerTest
    {
        private readonly EFUnitOfWork unitOfWork;

        private readonly BlogEntryFile file;

        public IncrementBlogEntryFileCounterCommandHandlerTest()
        {
            this.unitOfWork = new InMemoryDatabaseFactory().CreateContext();

            var blogEntry = new BlogEntry()
            {
                ShortContent = "Test",
                Header = "Test"
            };

            this.file = new BlogEntryFile()
            {
                Name = "test.png",
                BlogEntryId = blogEntry.Id
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
             sut.HandleAsync(new IncrementBlogEntryFileCounterCommand()
             {
                 Id = this.file.Id
             }));
        }
    }
}
