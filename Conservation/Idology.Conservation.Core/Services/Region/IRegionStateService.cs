namespace Idology.Conservation.Core.Services.Region;

public interface IRegionStateService
{
    void SetSelectedRegion(int regionId);
    void SetActiveRegion(int regionId);
    void ClearActiveRegion();
    void ActivateRegionScreen();
    void SetInfoPanelToRegion();
    bool IsRegionSummaryCurrentlyActive();
    bool IsRegionScreenOpen();
}
