using System.Linq;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class AddOrUpdateBlogEntryFileCommandHandler : ICommandHandler<AddOrUpdateBlogEntryFileCommand>
    {
        private readonly IRepository repository;

        public AddOrUpdateBlogEntryFileCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(AddOrUpdateBlogEntryFileCommand command)
        {
            int indexOfLastDot = command.FileName.LastIndexOf('.');
            string name = command.FileName.Substring(0, indexOfLastDot);
            string extension = command.FileName.Substring(indexOfLastDot + 1, command.FileName.Length - indexOfLastDot - 1);

            BlogEntryFile blogEntryFile = this.repository.BlogEntryFiles
                .SingleOrDefault(f => f.BlogEntryId == command.BlogEntryId && f.Name == name && f.Extension == extension);

            if (blogEntryFile == null)
            {
                blogEntryFile = new BlogEntryFile() { BlogEntryId = command.BlogEntryId, Name = name, Extension = extension };
                this.repository.BlogEntryFiles.Add(blogEntryFile);
            }

            blogEntryFile.Data = command.Data;

            this.repository.SaveChanges();
        }
    }
}
