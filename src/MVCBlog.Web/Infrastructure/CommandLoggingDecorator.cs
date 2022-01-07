using System.Text.Json;
using MVCBlog.Business;

namespace MVCBlog.Web.Infrastructure;

public class CommandLoggingDecorator<TCommand> : ICommandHandler<TCommand>
{
    private readonly ICommandHandler<TCommand> handler;

    private readonly ILogger<CommandLoggingDecorator<TCommand>> logger;

    private readonly IHttpContextAccessor httpContextAccessor;

    public CommandLoggingDecorator(
        ICommandHandler<TCommand> handler,
        ILogger<CommandLoggingDecorator<TCommand>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        this.handler = handler;
        this.logger = logger;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task HandleAsync(TCommand command)
    {
        this.logger.LogInformation(
            "Executing command '{0}' (User: '{1}', Data: '{2}')",
            command?.GetType().Name,
            this.httpContextAccessor.HttpContext?.User?.Identity?.Name,
            JsonSerializer.Serialize(command));

        await this.handler.HandleAsync(command);
    }
}
