namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

internal sealed class SetScreenStateGameCommandHandler : IDeferredGameCommandHandler<SetScreenStateGameCommand>
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

    public bool CanHandleCommand(SetScreenStateGameCommand command)
    {
        if (_gameData.InteractionData.ScreenState != command.ScreenState)
        {
            return true;
        }

        return false;
    }

    public void HandleCommand(SetScreenStateGameCommand command)
    {
        _gameData.InteractionData.ScreenState = command.ScreenState;

        _eventRoutingService.InvokeSetScreenState(command);
    }
}
