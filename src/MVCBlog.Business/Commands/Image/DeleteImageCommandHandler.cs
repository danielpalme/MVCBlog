using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Business.IO;
using MVCBlog.Data;

namespace MVCBlog.Business.Commands;

public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand>
{
    private readonly EFUnitOfWork unitOfWork;

    private readonly IImageFileProvider fileProvider;

    public DeleteImageCommandHandler(EFUnitOfWork unitOfWork, IImageFileProvider fileProvider)
    {
        this.unitOfWork = unitOfWork;
        this.fileProvider = fileProvider;
    }

    public async Task HandleAsync(DeleteImageCommand command)
    {
        var entity = await this.unitOfWork.Images.SingleOrDefaultAsync(e => e.Id == command.Id);

        if (entity != null)
        {
            this.unitOfWork.Images.Remove(entity);

            await this.unitOfWork.SaveChangesAsync();

            await this.fileProvider.DeleteFileAsync(entity.Path);
        }
    }
}
