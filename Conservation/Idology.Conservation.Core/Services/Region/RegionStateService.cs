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

    public void ClearActiveRegion()
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
        throw new NotImplementedException();
    }

    public void SetActiveRegion(int regionId)
    {
        throw new NotImplementedException();
    }

    public void SetInfoPanelToRegion()
    {
        throw new NotImplementedException();
    }

    public void SetSelectedRegion(int regionId)
    {
        _conservationStateService.SetState(_ => _.WithSetSelectedRegion(regionId));
    }
}
