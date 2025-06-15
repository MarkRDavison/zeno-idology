namespace Idology.Space.Core.Commands;

public sealed class SpaceCommandHandler
{
    private readonly List<ISpaceCommandHandlerBase> _handlers;
    public SpaceCommandHandler(IEnumerable<ISpaceCommandHandlerBase> handlers)
    {
        _handlers = handlers.ToList();
    }

    public void HandleCommand<TCommand>(TCommand command) where TCommand : class, ISpaceCommand
    {
        if (_handlers.FirstOrDefault(_ => _ is ISpaceCommandHandler<TCommand>) is ISpaceCommandHandler<TCommand> handler)
        {
            handler.HandleCommand(command);
        }
    }
}
