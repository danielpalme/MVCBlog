
using System.Threading.Tasks;
namespace MVCBlog.Core.Commands
{
    public class CommandLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(CommandLoggingDecorator<TCommand>));

        private readonly ICommandHandler<TCommand> handler;

        public CommandLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            this.handler = handler;
        }

        public async Task HandleAsync(TCommand command)
        {
            Logger.Info("Executing command " + command.GetType().Name);

            await this.handler.HandleAsync(command);
        }
    }
}
