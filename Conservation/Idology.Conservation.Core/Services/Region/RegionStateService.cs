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
        _conservationStateService.SetState(_ => _.WithCloseRegionSummary());
        _infoPanelStateService.PopInfoPanel(InfoState.RegionSummary);
    }

    public bool IsRegionSummaryCurrentlyActive()
    {
        return
            _conservationStateService.State.InteractionData.ScreenState is ScreenState.Default &&
            _conservationStateService.State.InteractionData.InfoState is InfoState.RegionSummary;
    }

    public bool IsRegionScreenOpen()
    {
        return _conservationStateService.State.InteractionData.ScreenState is ScreenState.Region;
    }

    public void SetActiveRegion(int regionId)
    {
        _conservationStateService
            .SetState(_ => _
                .WithActiveRegion(regionId));
    }

    public void SetInfoPanelToRegion()
    {
        _conservationStateService
            .SetState(_ => _
                .WithInfoScreenState(InfoState.Region));
    }

    public void SetSelectedRegion(int regionId)
    {
        _conservationStateService.SetState(_ => _.WithSetSelectedRegion(regionId));
    }

    public void CloseRegionScreenState()
    {
        _conservationStateService
            .SetState(_ => _
                .WithInteractionScreenState(ScreenState.Default)
                .WithInfoScreenState(InfoState.Hidden)
                .WithResetRegionState());
    }
}
