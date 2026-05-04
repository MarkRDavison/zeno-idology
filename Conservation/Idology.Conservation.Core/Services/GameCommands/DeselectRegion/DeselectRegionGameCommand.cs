namespace Idology.Conservation.Core.Services.GameCommands.DeselectRegion;

public sealed record DeselectRegionGameCommand(
) : IDeferredGameCommand;

internal sealed class DeselectRegionGameCommandHandler : IDeferredGameCommandHandler<DeselectRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;

    public DeselectRegionGameCommandHandler(IRegionStateService regionStateService)
    {
        _regionStateService = regionStateService;
    }

    public bool CanHandleCommand(DeselectRegionGameCommand command)
    {
        return _regionStateService.IsRegionSummaryCurrentlyActive();
    }

    public void HandleCommand(DeselectRegionGameCommand command)
    {
        _regionStateService.ClearActiveRegion();
    }
}