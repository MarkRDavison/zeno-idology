namespace Idology.Conservation.Core.Services.GameCommands.SelectKakapoWithinRegion;

public sealed record SelectKakapoWithinRegionGameCommand(
    int RegionId,
    int KakapoId
) : IDeferredGameCommand;

internal sealed class SelectKakapoWithinRegionGameCommandHandler : IDeferredGameCommandHandler<SelectKakapoWithinRegionGameCommand>
{
    private readonly IKakapoStateService _kakapoStateService;
    private readonly IEventRoutingService _eventRoutingService;

    public SelectKakapoWithinRegionGameCommandHandler(
        IKakapoStateService kakapoStateService,
        IEventRoutingService eventRoutingService)
    {
        _kakapoStateService = kakapoStateService;
        _eventRoutingService = eventRoutingService;
    }

    public bool CanHandleCommand(SelectKakapoWithinRegionGameCommand command) => true;

    public void HandleCommand(SelectKakapoWithinRegionGameCommand command)
    {
        _kakapoStateService.SetActiveKakapoId(command.KakapoId);
        _kakapoStateService.SetInfoPanelToKakapo();
        _eventRoutingService.InvokePushInfoState(new PushInfoPanelPayload(InfoState.KakapoSummary, new KakapoSummaryInfoPanelPayload(command.KakapoId)));
    }
}