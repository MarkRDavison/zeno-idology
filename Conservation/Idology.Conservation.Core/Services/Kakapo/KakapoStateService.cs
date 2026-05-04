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
        throw new NotImplementedException();
    }

    public void SetInfoPanelToKakapo()
    {
        throw new NotImplementedException();
    }
}
