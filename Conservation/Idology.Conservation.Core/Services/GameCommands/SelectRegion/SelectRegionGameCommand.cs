namespace Idology.Conservation.Core.Services.GameCommands.SelectRegion;

public sealed record SelectRegionGameCommand(
    int RegionId
) : IDeferredGameCommand;

internal sealed class SelectRegionGameCommandHandler : IDeferredGameCommandHandler<SelectRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;

    public SelectRegionGameCommandHandler(IRegionStateService regionStateService)
    {
        _regionStateService = regionStateService;
    }

    public bool CanHandleCommand(SelectRegionGameCommand command) => true;

    public void HandleCommand(SelectRegionGameCommand command)
    {
        _regionStateService.SetSelectedRegion(command.RegionId);
    }
}