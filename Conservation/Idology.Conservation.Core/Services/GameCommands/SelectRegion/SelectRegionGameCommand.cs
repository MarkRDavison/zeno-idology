namespace Idology.Conservation.Core.Services.GameCommands.SelectRegion;

public sealed record SelectRegionGameCommand(
    int RegionId
) : IDeferredGameCommand;

internal sealed class SelectRegionGameCommandHandler : IDeferredGameCommandHandler<SelectRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;

    public SelectRegionGameCommandHandler(
        IRegionStateService regionStateService,
        IInfoPanelStateService infoPanelStateService)
    {
        _regionStateService = regionStateService;
        _infoPanelStateService = infoPanelStateService;
    }

    public bool CanHandleCommand(SelectRegionGameCommand command)
    {
        if (!_regionStateService.IsRegionSummaryCurrentlyActive())
        {
            return true;
        }

        if (_regionStateService.GetSelectedRegion() != command.RegionId)
        {
            return true;
        }

        return false;
    }

    public void HandleCommand(SelectRegionGameCommand command)
    {
        _regionStateService.SetSelectedRegion(command.RegionId);
        _infoPanelStateService.PushInfoPanel(InfoState.RegionSummary, new RegionInfoPanelPayload(command.RegionId));

    }
}