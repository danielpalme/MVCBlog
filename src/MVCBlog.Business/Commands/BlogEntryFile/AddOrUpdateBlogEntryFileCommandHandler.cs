using Microsoft.EntityFrameworkCore;
using MVCBlog.Business.IO;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class AddOrUpdateBlogEntryFileCommandHandler : ICommandHandler<AddOrUpdateBlogEntryFileCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly IBlogEntryFileFileProvider fileProvider;

    public AddOrUpdateBlogEntryFileCommandHandler(EFUnitOfWork unitOfWork, IBlogEntryFileFileProvider fileProvider)
    {
        this.unitOfWork = unitOfWork;
        this.fileProvider = fileProvider;
    }

    public async Task HandleAsync(AddOrUpdateBlogEntryFileCommand command)
    {
        string fileName = command.FileName.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

        BlogEntryFile? blogEntryFile = await this.unitOfWork.BlogEntryFiles
            .SingleOrDefaultAsync(f => f.BlogEntryId == command.BlogEntryId && f.Name == fileName);

        if (blogEntryFile == null)
        {
            blogEntryFile = new BlogEntryFile(fileName)
            {
                BlogEntryId = command.BlogEntryId,
                Name = fileName
            };

            this.unitOfWork.BlogEntryFiles.Add(blogEntryFile);
        }

        await this.fileProvider.AddFileAsync($"{blogEntryFile.Id}.{extension}", command.Data);

        await this.unitOfWork.SaveChangesAsync();
    }
}