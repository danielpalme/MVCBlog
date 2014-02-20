using System.Threading.Tasks;
namespace MVCBlog.Core.Commands
{
    public interface ICommandHandler<TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}
