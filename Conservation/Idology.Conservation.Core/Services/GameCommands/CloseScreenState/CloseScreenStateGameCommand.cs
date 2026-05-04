namespace Idology.Conservation.Core.Services.GameCommands.CloseScreenState;

public sealed record CloseScreenStateGameCommand(
    ScreenState ScreenState
) : IGameCommand;

internal sealed class CloseScreenStateGameCommandHandler : IGameCommandHandler<CloseScreenStateGameCommand>
{
    private readonly IScreenStateService _screenStateService;
    private readonly IEventRoutingService _eventRoutingService;

    public CloseScreenStateGameCommandHandler(
        IScreenStateService screenStateService,
        IEventRoutingService eventRoutingService)
    {
        _screenStateService = screenStateService;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(CloseScreenStateGameCommand command)
    {
        if (_screenStateService.CloseScreen(command.ScreenState))
        {
            _eventRoutingService.InvokeSetScreenState(new SetScreenStateGameCommand(ScreenState.Default));
            return true;
        }

        return false;
    }
}