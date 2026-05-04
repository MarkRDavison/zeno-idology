namespace Idology.Conservation.Core.Services.GameCommands.CloseRegionScreen;

public sealed record CloseRegionScreenGameCommand(
) : IDeferredGameCommand;

internal sealed class CloseRegionScreenGameCommandHandler : IDeferredGameCommandHandler<CloseRegionScreenGameCommand>
{
    private readonly IRegionStateService _regionStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;

    public CloseRegionScreenGameCommandHandler(
        IRegionStateService regionStateService,
        IInfoPanelStateService infoPanelStateService)
    {
        _regionStateService = regionStateService;
        _infoPanelStateService = infoPanelStateService;
    }

    public bool CanHandleCommand(CloseRegionScreenGameCommand command)
    {
        return _regionStateService.IsRegionScreenOpen();
    }

    public void HandleCommand(CloseRegionScreenGameCommand command)
    {
        _regionStateService.CloseRegionScreenState();
        _infoPanelStateService.PopInfoPanel(InfoState.Region); // TODO: Might be kakapo summary or region
    }
}