namespace Idology.Conservation.Core.Services.Panel;

internal sealed class ScreenPanelService : IScreenPanelService
{
    private readonly IConservationStateService _conservationStateService;

    public ScreenPanelService(IConservationStateService conservationStateService)
    {
        _conservationStateService = conservationStateService;
    }

    public bool ClosePanel(ScreenPanelState panelState)
    {
        if (_conservationStateService.State.InteractionData.PanelState != panelState)
        {
            return false;
        }

        _conservationStateService
            .SetState(_ => _.WithScreenPanelState(ScreenPanelState.None));

        return true;
    }

    public bool OpenPanel(ScreenPanelState panelState)
    {
        _conservationStateService
            .SetState(_ => _.WithScreenPanelState(panelState));

        return true;
    }
}
