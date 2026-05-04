namespace Idology.Conservation.Core.Services.GameCommands.CloseRegionScreen;

public sealed record CloseRegionScreenGameCommand(
) : IDeferredGameCommand;

internal sealed class CloseRegionScreenGameCommandHandler : IDeferredGameCommandHandler<CloseRegionScreenGameCommand>
{
    private readonly IRegionStateService _regionStateService;

    public CloseRegionScreenGameCommandHandler(IRegionStateService regionStateService)
    {
        _regionStateService = regionStateService;
    }

    public bool CanHandleCommand(CloseRegionScreenGameCommand command)
    {
        return _regionStateService.IsRegionScreenOpen();
    }

    public void HandleCommand(CloseRegionScreenGameCommand command)
    {
        throw new NotImplementedException();
    }
}