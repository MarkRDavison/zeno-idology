namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

public sealed record SetScreenStateGameCommand(
    ScreenState State
) : IGameCommand;

internal sealed class SetScreenStateGameCommandHandler : IGameCommandHandler<SetScreenStateGameCommand>
{
    private readonly IScreenStateService _screenStateService;
    private readonly IEventRoutingService _eventRoutingService;

    public SetScreenStateGameCommandHandler(
        IScreenStateService screenStateService,
        IEventRoutingService eventRoutingService)
    {
        _screenStateService = screenStateService;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(SetScreenStateGameCommand command)
    {
        if (_screenStateService.OpenScreenState(command.State))
        {
            _eventRoutingService.InvokeSetScreenState(command);
            return true;
        }

        return false;
    }
}