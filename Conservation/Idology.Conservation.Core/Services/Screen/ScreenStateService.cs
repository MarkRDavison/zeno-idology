namespace Idology.Conservation.Core.Services.Screen;

internal sealed class ScreenStateService : IScreenStateService
{
    private readonly IConservationStateService _conservationStateService;

    public ScreenStateService(
        IConservationStateService conservationStateService)
    {
        _conservationStateService = conservationStateService;
    }

    public bool CloseScreen(ScreenState screenState)
    {
        if (_conservationStateService.State.InteractionData.ScreenState != screenState)
        {
            return false;
        }

        _conservationStateService.SetState(_ => _.WithInteractionScreenState(ScreenState.Default));

        return true;
    }

    public bool OpenScreenState(ScreenState screenState)
    {
        if (_conservationStateService.State.InteractionData.ScreenState == screenState)
        {
            return false;
        }

        _conservationStateService.SetState(_ => _.WithInteractionScreenState(screenState));

        return true;
    }
}
