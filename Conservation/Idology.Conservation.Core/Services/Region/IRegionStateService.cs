namespace Idology.Conservation.Core.Services.Region;

public interface IRegionStateService
{
    void SetSelectedRegion(int regionId);
    void SetActiveRegion(int regionId);
    void ClearSelectedRegion();
    void ActivateRegionScreen();
    bool IsRegionSummaryCurrentlyActive();
    bool IsRegionScreenOpen();
    void CloseRegionScreenState();
    int? GetSelectedRegion();
}
