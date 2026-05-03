namespace Idology.Conservation.Core.Services.GameCommands;

internal sealed class GameCommandService : IGameCommandService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly List<Action> _commands = [];

    public GameCommandService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool HandleCommand<TCommand>(TCommand command) where TCommand : class, IGameCommand
    {
        var handler = _serviceProvider.GetRequiredService<IGameCommandHandler<TCommand>>();

        return handler.HandleCommand(command);
    }

    public bool EnqueueCommand<TCommand>(TCommand command) where TCommand : class, IDeferredGameCommand
    {
        var handler = _serviceProvider.GetRequiredService<IDeferredGameCommandHandler<TCommand>>();

        if (handler.CanHandleCommand(command))
        {
            _commands.Add(() =>
            {

                handler.HandleCommand(command);
            });

            return true;
        }

        return false;
    }

    public void HandleEnqueuedCommands()
    {
        foreach (var c in _commands)
        {
            c();
        }
        _commands.Clear();
    }
}
