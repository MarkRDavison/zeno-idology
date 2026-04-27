namespace Idology.Conservation.Core.Services.GameCommands;

internal sealed class GameCommandService : IGameCommandService
{
    private readonly IServiceProvider _serviceProvider;

    public GameCommandService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool HandleCommand<TCommand>(TCommand command) where TCommand : class, IGameCommand
    {
        var handler = _serviceProvider.GetRequiredService<IGameCommandHandler<TCommand>>();

        return handler.HandleCommand(command);
    }
}
