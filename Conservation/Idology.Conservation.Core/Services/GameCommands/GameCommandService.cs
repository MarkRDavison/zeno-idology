namespace Idology.Conservation.Core.Services.GameCommands;

internal sealed class GameCommandService : IGameCommandService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GameCommandService> _logger;

    private readonly List<Action> _commands = [];

    public GameCommandService(
        IServiceProvider serviceProvider,
        ILogger<GameCommandService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public bool HandleCommand<TCommand>(TCommand command) where TCommand : class, IGameCommand
    {
        _logger.LogInformation("Handling GameCommand - {0} - Begin", typeof(TCommand));

        var handler = _serviceProvider.GetRequiredService<IGameCommandHandler<TCommand>>();

        var response = handler.HandleCommand(command);

        _logger.LogInformation("Handling GameCommand - {0} - End: {1}", typeof(TCommand), response);

        return response;
    }

    public bool EnqueueCommand<TCommand>(TCommand command) where TCommand : class, IDeferredGameCommand
    {
        var handler = _serviceProvider.GetRequiredService<IDeferredGameCommandHandler<TCommand>>();

        var canHandle = handler.CanHandleCommand(command);

        if (canHandle)
        {
            _logger.LogInformation("Enqueue GameCommand - {0}", typeof(TCommand));

            _commands.Add(() =>
            {
                _logger.LogInformation("Handling deferred GameCommand - {0} - Begin", typeof(TCommand));

                handler.HandleCommand(command);

                _logger.LogInformation("Handling deferred GameCommand - {0} - End", typeof(TCommand));
            });

            return true;
        }

        return false;
    }

    public void HandleEnqueuedCommands()
    {
        if (_commands.Count > 0)
        {
            _logger.LogInformation("Handling enqueued {0} commands", _commands.Count);
            _commands.ForEach(c =>
            {
                c.Invoke();
            });

            _commands.Clear();
        }
    }
}
