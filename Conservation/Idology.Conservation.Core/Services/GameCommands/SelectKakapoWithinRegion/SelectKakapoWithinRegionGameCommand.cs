namespace Idology.Conservation.Core.Services.GameCommands.SelectKakapoWithinRegion;

public sealed record SelectKakapoWithinRegionGameCommand(
    int RegionId,
    int KakapoId
) : IDeferredGameCommand;

internal sealed class SelectKakapoWithinRegionGameCommandHandler : IDeferredGameCommandHandler<SelectKakapoWithinRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;
    private readonly IKakapoStateService _kakapoStateService;

    public SelectKakapoWithinRegionGameCommandHandler(
        IRegionStateService regionStateService,
        IKakapoStateService kakapoStateService)
    {
        _regionStateService = regionStateService;
        _kakapoStateService = kakapoStateService;
    }

    public bool CanHandleCommand(SelectKakapoWithinRegionGameCommand command) => true;

    public void HandleCommand(SelectKakapoWithinRegionGameCommand command)
    {
        _regionStateService.SetActiveRegion(command.RegionId);
        _regionStateService.ActivateRegionScreen();
        _kakapoStateService.SetActiveKakapoId(command.KakapoId);
        _kakapoStateService.SetInfoPanelToKakapo();
    }
}