namespace Idology.Conservation.Core.Services.GameCommands.ClosePanelState;

public sealed record ClosePanelStateGameCommand(
    ScreenPanelState PanelState
) : IGameCommand;

internal sealed class ClosePanelStateGameCommandHandler : IGameCommandHandler<ClosePanelStateGameCommand>
{
    private readonly IScreenPanelService _screenPanelService;
    private readonly IEventRoutingService _eventRoutingService;

    public ClosePanelStateGameCommandHandler(
        IScreenPanelService screenPanelService,
        IEventRoutingService eventRoutingService)
    {
        _screenPanelService = screenPanelService;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(ClosePanelStateGameCommand command)
    {
        if (!_screenPanelService.ClosePanel(command.PanelState))
        {
            return false;
        }

        _eventRoutingService.InvokeCloseScreenPanel(new CloseScreenPanelPayload(command.PanelState));

        return true;
    }
}