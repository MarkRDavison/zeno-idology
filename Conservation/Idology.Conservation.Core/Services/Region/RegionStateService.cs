namespace Idology.Conservation.Core.Services.Region;

internal sealed class RegionStateService : IRegionStateService
{
    private readonly IConservationStateService _conservationStateService;

    public RegionStateService(IConservationStateService conservationStateService)
    {
        _conservationStateService = conservationStateService;
    }

    public void ActivateRegionScreen()
    {
        throw new NotImplementedException();
    }

    public void ClearActiveRegion()
    {
        throw new NotImplementedException();
    }

    public bool IsRegionCurrentlyActive()
    {
        throw new NotImplementedException();
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
