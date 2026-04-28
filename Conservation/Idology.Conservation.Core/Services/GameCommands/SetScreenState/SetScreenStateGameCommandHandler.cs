namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

internal sealed class SetScreenStateGameCommandHandler : IGameCommandHandler<SetScreenStateGameCommand>
{
    private readonly ConservationGameData _gameData;
    private readonly IEventRoutingService _eventRoutingService;
    private readonly IGameCommandService _gameCommandService;

    public SetScreenStateGameCommandHandler(
        ConservationGameData gameData,
        IEventRoutingService eventRoutingService,
        IGameCommandService gameCommandService)
    {
        _gameData = gameData;
        _eventRoutingService = eventRoutingService;
        _gameCommandService = gameCommandService;
    }

    public bool HandleCommand(SetScreenStateGameCommand command)
    {
        if (_gameData.InteractionData.ScreenState != command.ScreenState)
        {
            _gameData.InteractionData.ScreenState = command.ScreenState;

            _gameCommandService.HandleCommand(new SetInfoScreenGameCommand
            {
                Open = false,
                State = InfoState.Hidden,
                Context = null
            });
            _eventRoutingService.InvokeSetScreenState(command);

            return true;
        }

        return false;
    }
}
