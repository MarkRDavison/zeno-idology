namespace Idology.Conservation.Core.Services.Kakapo;

internal sealed class KakapoStateService : IKakapoStateService
{
    private readonly IConservationStateService _conservationStateService;

    public KakapoStateService(
        IConservationStateService conservationStateService)
    {
        _conservationStateService = conservationStateService;
    }

    public void SetActiveKakapoId(int kakapoId)
    {
        _conservationStateService
            .SetState(_ => _
                .WithActiveKakapo(kakapoId));
    }

    public void SetInfoPanelToKakapo()
    {
        _conservationStateService
            .SetState(_ => _
                .WithInfoScreenState(InfoState.KakapoSummary));
    }
}
