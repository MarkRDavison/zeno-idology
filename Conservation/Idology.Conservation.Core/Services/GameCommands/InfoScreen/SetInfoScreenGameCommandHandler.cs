namespace Idology.Conservation.Core.Services.GameCommands.InfoScreen;

internal sealed class SetInfoScreenGameCommandHandler : IGameCommandHandler<SetInfoScreenGameCommand>
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

    public bool HandleCommand(SetInfoScreenGameCommand command)
    {
        if (!command.Open && _gameData.InteractionData.InfoState is InfoState.Hidden)
        {
            return false;
        }

        _gameData.InteractionData.InfoState = command.State;

        _eventRoutingService.InvokeSetInfoState(command);

        return true;
    }
}
