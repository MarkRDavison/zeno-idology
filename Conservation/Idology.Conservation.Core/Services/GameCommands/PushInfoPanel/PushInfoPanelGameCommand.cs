namespace Idology.Conservation.Core.Services.GameCommands.PushInfoPanel;

public sealed record PushInfoPanelGameCommand(
    InfoState InfoState,
    object? Context
) : IDeferredGameCommand;

internal sealed class PushInfoPanelGameCommandHandler : IDeferredGameCommandHandler<PushInfoPanelGameCommand>
{
    public bool CanHandleCommand(PushInfoPanelGameCommand command)
    {
        throw new NotImplementedException();
    }

    public void HandleCommand(PushInfoPanelGameCommand command)
    {
        throw new NotImplementedException();
    }
}