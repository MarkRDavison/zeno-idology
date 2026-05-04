namespace Idology.Conservation.Core.Services.InfoPanel;

internal sealed class InfoPanelStateService : IInfoPanelStateService
{
    private readonly IConservationStateService _conservationStateService;
    private readonly IEventRoutingService _eventRoutingService;

    public InfoPanelStateService(
        IConservationStateService conservationStateService,
        IEventRoutingService eventRoutingService)
    {
        _conservationStateService = conservationStateService;
        _eventRoutingService = eventRoutingService;
    }

    public void PopInfoPanel(InfoState infoState)
    {
        _eventRoutingService.InvokePopInfoState(new PopInfoPanelPayload(infoState));
    }

    public void PushInfoPanel(InfoState infoState, object? payload)
    {
        _conservationStateService
            .SetState(_ => _
                .WithInfoScreenState(infoState));

        _eventRoutingService.InvokePushInfoState(new PushInfoPanelPayload(infoState, payload));
    }
}
