namespace Idology.Conservation.Core.Services.MainScreen;

internal sealed class MainScreenStateService : IMainScreenStateService
{
    private readonly IConservationStateService _conservationStateService;

    public MainScreenStateService(
        IConservationStateService conservationStateService)
    {
        _conservationStateService = conservationStateService;
    }

    public bool OpenMainScreenState(MainScreenState screenState)
    {
        if (_conservationStateService.State.InteractionData.MainScreenState == screenState)
        {
            return false;
        }

        _conservationStateService.SetState(_ => _.WithInteractionScreenState(screenState));

        return true;
    }
}
