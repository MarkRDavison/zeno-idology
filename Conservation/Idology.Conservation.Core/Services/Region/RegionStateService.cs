namespace Idology.Conservation.Core.Services.Region;

internal sealed class RegionStateService : IRegionStateService
{
    private readonly IConservationStateService _conservationStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;

    public RegionStateService(
        IConservationStateService conservationStateService,
        IInfoPanelStateService infoPanelStateService)
    {
        _conservationStateService = conservationStateService;
        _infoPanelStateService = infoPanelStateService;
    }

    public void ActivateRegionScreen()
    {
        throw new NotImplementedException();
    }

    public void ClearSelectedRegion()
    {
        _infoPanelStateService.PopInfoPanel(InfoState.RegionSummary);
    }

    public bool IsRegionSummaryCurrentlyActive()
    {
        return
            _conservationStateService.State.InteractionData.MainScreenState is MainScreenState.Default &&
            _conservationStateService.State.InteractionData.InfoState.Last() is InfoState.RegionSummary;
    }

    public bool IsRegionScreenOpen()
    {
        return
            _conservationStateService.State.InteractionData.MainScreenState is MainScreenState.Region &&
            _conservationStateService.State.InteractionData.PanelState is ScreenPanelState.None;
    }

    public void SetActiveRegion(int regionId)
    {
        _conservationStateService
            .SetState(_ => _
                .WithActiveRegion(regionId));
    }

    public void SetSelectedRegion(int regionId)
    {
        _conservationStateService.SetState(_ => _.WithSetSelectedRegion(regionId));
    }

    public void CloseRegionScreenState()
    {
        _conservationStateService
            .SetState(_ => _
                .WithInteractionScreenState(MainScreenState.Default)
                .WithResetRegionState());
    }
}
