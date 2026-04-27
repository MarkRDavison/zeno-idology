namespace Idology.Conservation.Core.Services.GameCommands.PopSubScene;

internal sealed class PopSubSceneGameCommandHandler : IGameCommandHandler<PopSubSceneGameCommand>
{
    private readonly ConservationGameData _gameData;
    private readonly IEventRoutingService _eventRoutingService;

    public PopSubSceneGameCommandHandler(
        ConservationGameData gameData,
        IEventRoutingService eventRoutingService)
    {
        _gameData = gameData;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(PopSubSceneGameCommand command)
    {
        if (_gameData.InteractionData.ScreenState is not ScreenState.Default)
        {
            _gameData.InteractionData.ScreenState = ScreenState.Default;

            _eventRoutingService.InvokePopSubScene(command);
        }

        return false;
    }
}
