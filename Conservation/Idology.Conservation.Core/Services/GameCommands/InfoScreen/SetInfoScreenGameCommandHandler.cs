namespace Idology.Conservation.Core.Services.GameCommands.InfoScreen;

internal sealed class SetInfoScreenGameCommandHandler : IDeferredGameCommandHandler<SetInfoScreenGameCommand>
{
    private readonly IEventRoutingService _eventRoutingService;
    private readonly ConservationGameData _gameData;

    public SetInfoScreenGameCommandHandler(
        IEventRoutingService eventRoutingService,
        ConservationGameData gameData)
    {
        _eventRoutingService = eventRoutingService;
        _gameData = gameData;
    }

    public bool CanHandleCommand(SetInfoScreenGameCommand command)
    {
        if (!command.Open && _gameData.InteractionData.InfoState is InfoState.Hidden)
        {
            return false;
        }

        return true;
    }

    public void HandleCommand(SetInfoScreenGameCommand command)
    {
        _gameData.InteractionData.InfoState = command.State;

        _eventRoutingService.InvokeSetInfoState(command);
    }
}
