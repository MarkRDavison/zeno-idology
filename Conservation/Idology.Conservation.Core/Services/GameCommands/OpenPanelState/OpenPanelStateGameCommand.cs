namespace Idology.Conservation.Core.Services.GameCommands.OpenPanelState;

public sealed record OpenPanelStateGameCommand(
    ScreenPanelState PanelState,
    object? Payload
) : IGameCommand;

internal sealed class OpenPanelStateGameCommandHandler : IGameCommandHandler<OpenPanelStateGameCommand>
{
    private readonly IScreenPanelService _screenPanelService;
    private readonly IEventRoutingService _eventRoutingService;

    public OpenPanelStateGameCommandHandler(
        IScreenPanelService screenPanelService,
        IEventRoutingService eventRoutingService)
    {
        _screenPanelService = screenPanelService;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(OpenPanelStateGameCommand command)
    {
        if (!_screenPanelService.OpenPanel(command.PanelState))
        {
            return false;
        }

        _eventRoutingService.InvokeOpenScreenPanel(new OpenScreenPanelPayload(command.PanelState, command.Payload));

        return true;
    }
}