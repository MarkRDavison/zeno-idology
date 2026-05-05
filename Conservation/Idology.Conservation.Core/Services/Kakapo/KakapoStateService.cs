namespace Idology.Conservation.Core.Services.Kakapo;

internal sealed class KakapoStateService : IKakapoStateService
{
    private readonly IConservationStateService _conservationStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;

    public KakapoStateService(
        IConservationStateService conservationStateService,
        IInfoPanelStateService infoPanelStateService)
    {
        _conservationStateService = conservationStateService;
        _infoPanelStateService = infoPanelStateService;
    }

    public void SetActiveKakapoIdAndInfoPanel(int kakapoId)
    {
        _conservationStateService
            .SetState(_ => _
                .WithActiveKakapo(kakapoId));

        _infoPanelStateService.PushInfoPanel(InfoState.KakapoSummary, new KakapoSummaryInfoPanelPayload(kakapoId));
    }
}
