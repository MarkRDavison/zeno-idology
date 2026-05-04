namespace Idology.Conservation.Core.Services.GameCommands.FocusRegion;

public sealed record FocusRegionGameCommand(
    int RegionId
) : IGameCommand;

internal sealed class FocusRegionGameCommandHandler : IGameCommandHandler<FocusRegionGameCommand>
{
    private readonly ICameraService _cameraService;

    public FocusRegionGameCommandHandler(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }

    public bool HandleCommand(FocusRegionGameCommand command)
    {
        return _cameraService.FocusOnRegion(command.RegionId);
    }
}