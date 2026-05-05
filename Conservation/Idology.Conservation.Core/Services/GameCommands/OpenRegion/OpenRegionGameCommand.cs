namespace Idology.Conservation.Core.Services.GameCommands.OpenRegion;

public sealed record OpenRegionGameCommand(
    int RegionId
) : IDeferredGameCommand;

internal sealed class OpenRegionGameCommandHandler : IDeferredGameCommandHandler<OpenRegionGameCommand>
{
    private readonly IRegionStateService _regionStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;
    private readonly ICameraService _cameraService;

    public OpenRegionGameCommandHandler(
        IRegionStateService regionStateService,
        IInfoPanelStateService infoPanelStateService,
        ICameraService cameraService)
    {
        _regionStateService = regionStateService;
        _infoPanelStateService = infoPanelStateService;
        _cameraService = cameraService;
    }

    public bool CanHandleCommand(OpenRegionGameCommand command) => true;

    public void HandleCommand(OpenRegionGameCommand command)
    {
        _regionStateService.SetActiveRegion(command.RegionId);
        _cameraService.FocusOnRegion(command.RegionId);
        _infoPanelStateService.PushInfoPanel(InfoState.Region, new RegionInfoPanelPayload(command.RegionId));
    }
}