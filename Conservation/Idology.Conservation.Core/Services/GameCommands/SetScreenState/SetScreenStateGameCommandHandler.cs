namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

internal sealed class SetScreenStateGameCommandHandler : IGameCommandHandler<SetScreenStateGameCommand>
{
    private readonly ConservationGameData _gameData;
    private readonly IEventRoutingService _eventRoutingService;

    public SetScreenStateGameCommandHandler(
        ConservationGameData gameData,
        IEventRoutingService eventRoutingService)
    {
        _gameData = gameData;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(SetScreenStateGameCommand command)
    {
        if (_gameData.InteractionData.ScreenState != command.ScreenState)
        {
            _gameData.InteractionData.ScreenState = command.ScreenState;

            _eventRoutingService.InvokeSetScreenState(command);

            return true;
        }

        return false;
    }
}
