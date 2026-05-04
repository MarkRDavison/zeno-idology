namespace Idology.Conservation.Core.Services.GameCommands.PopInfoPanel;

public sealed record PopInfoPanelGameCommand(
    InfoState InfoState
) : IDeferredGameCommand;

internal sealed class PopInfoPanelGameCommandHandler : IDeferredGameCommandHandler<PopInfoPanelGameCommand>
{
    public bool CanHandleCommand(PopInfoPanelGameCommand command)
    {
        throw new NotImplementedException();
    }

    public void HandleCommand(PopInfoPanelGameCommand command)
    {
        throw new NotImplementedException();
    }
}