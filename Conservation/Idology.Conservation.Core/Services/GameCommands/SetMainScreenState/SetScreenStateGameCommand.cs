namespace Idology.Conservation.Core.Services.GameCommands.SetMainScreenState;

public sealed record SetMainScreenStateGameCommand(
    MainScreenState MainScreenState,
    object? Payload
) : IGameCommand;

internal sealed class SetMainScreenStateGameCommandHandler : IGameCommandHandler<SetMainScreenStateGameCommand>
{
    private readonly IMainScreenStateService _mainScreenStateService;
    private readonly IEventRoutingService _eventRoutingService;

    public SetMainScreenStateGameCommandHandler(
        IMainScreenStateService mainScreenStateService,
        IEventRoutingService eventRoutingService)
    {
        _mainScreenStateService = mainScreenStateService;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(SetMainScreenStateGameCommand command)
    {
        if (_mainScreenStateService.OpenMainScreenState(command.MainScreenState))
        {
            _eventRoutingService.InvokeSetMainScreenState(new SetMainScreenPanelPayload(command.MainScreenState, command.Payload));
            return true;
        }

        return false;
    }
}