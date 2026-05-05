namespace Idology.Conservation.Core.Services.GameCommands.PopInfoPanel;

public sealed record PopInfoPanelGameCommand(
    InfoState InfoState
) : IDeferredGameCommand;

internal sealed class PopInfoPanelGameCommandHandler : IDeferredGameCommandHandler<PopInfoPanelGameCommand>
{
    private readonly IConservationStateService _conservationStateService;
    private readonly IInfoPanelStateService _infoPanelStateService;

    public PopInfoPanelGameCommandHandler(
        IConservationStateService conservationStateService,
        IInfoPanelStateService infoPanelStateService)
    {
        _conservationStateService = conservationStateService;
        _infoPanelStateService = infoPanelStateService;
    }

    public bool CanHandleCommand(PopInfoPanelGameCommand command)
    {
        return
            _conservationStateService.State.InteractionData.InfoState.Any() &&
            _conservationStateService.State.InteractionData.InfoState.Last() == command.InfoState;
    }

    public void HandleCommand(PopInfoPanelGameCommand command)
    {
        _infoPanelStateService.PopInfoPanel(command.InfoState);
    }
}