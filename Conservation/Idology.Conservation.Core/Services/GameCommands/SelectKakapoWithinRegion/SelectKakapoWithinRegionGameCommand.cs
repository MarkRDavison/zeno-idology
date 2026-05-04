namespace Idology.Conservation.Core.Services.GameCommands.SelectKakapoWithinRegion;

public sealed record SelectKakapoWithinRegionGameCommand(
    int RegionId,
    int KakapoId
) : IDeferredGameCommand;

internal sealed class SelectKakapoWithinRegionGameCommandHandler : IDeferredGameCommandHandler<SelectKakapoWithinRegionGameCommand>
{
    private readonly IKakapoStateService _kakapoStateService;

    public SelectKakapoWithinRegionGameCommandHandler(
        IKakapoStateService kakapoStateService)
    {
        _kakapoStateService = kakapoStateService;
    }

    public bool CanHandleCommand(SelectKakapoWithinRegionGameCommand command) => true;

    public void HandleCommand(SelectKakapoWithinRegionGameCommand command)
    {
        _kakapoStateService.SetActiveKakapoId(command.KakapoId);
        _kakapoStateService.SetInfoPanelToKakapo();
    }
}