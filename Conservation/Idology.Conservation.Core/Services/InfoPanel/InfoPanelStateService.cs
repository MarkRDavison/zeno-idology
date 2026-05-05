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
        _conservationStateService
            .SetState(_ => _
                .WithPopInfoScreenState(infoState));
        _eventRoutingService.InvokePopInfoState(new PopInfoPanelPayload(infoState));
    }

    public void PushInfoPanel(InfoState infoState, object? payload)
    {
        if (_conservationStateService.State.InteractionData.InfoState.Count == 0 ||
            _conservationStateService.State.InteractionData.InfoState.Last() != infoState)
        {
            _conservationStateService
                .SetState(_ => _
                    .WithPushInfoScreenState(infoState));
        }

        _eventRoutingService.InvokePushInfoState(new PushInfoPanelPayload(infoState, payload));
    }
}
