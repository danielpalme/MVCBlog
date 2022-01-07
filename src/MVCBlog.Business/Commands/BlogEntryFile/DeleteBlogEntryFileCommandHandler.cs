using Microsoft.EntityFrameworkCore;
using MVCBlog.Business.IO;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class DeleteBlogEntryFileCommandHandler : ICommandHandler<DeleteBlogEntryFileCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly IBlogEntryFileFileProvider fileProvider;

    public DeleteBlogEntryFileCommandHandler(EFUnitOfWork unitOfWork, IBlogEntryFileFileProvider fileProvider)
    {
        this.unitOfWork = unitOfWork;
        this.fileProvider = fileProvider;
    }

    public async Task HandleAsync(DeleteBlogEntryFileCommand command)
    {
        var entity = await this.unitOfWork.BlogEntryFiles.SingleOrDefaultAsync(e => e.Id == command.Id);

        if (entity != null)
        {
            this.unitOfWork.BlogEntryFiles.Remove(entity);

            await this.unitOfWork.SaveChangesAsync();

            string extension = entity.Name.Substring(entity.Name.LastIndexOf('.') + 1);
            await this.fileProvider.DeleteFileAsync($"{entity.Id}.{extension}");
        }
    }
}