namespace Idology.Conservation.Core.Services.GameCommands.OpenRegion;

public sealed record OpenRegionGameCommand(
    int RegionId
) : IGameCommand;

internal sealed class OpenRegionGameCommandHandler : IGameCommandHandler<OpenRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;

    public OpenRegionGameCommandHandler(IRegionStateService regionStateService)
    {
        _regionStateService = regionStateService;
    }

    public bool HandleCommand(OpenRegionGameCommand command)
    {
        _regionStateService.SetActiveRegion(command.RegionId);
        _regionStateService.SetInfoPanelToRegion();

        return true;
    }
}