namespace Idology.Conservation.Core.Services.GameCommands.SetSubScene;

internal sealed class SetSubSceneGameCommandHandler : IGameCommandHandler<SetSubSceneGameCommand>
{
    private readonly ConservationGameData _gameData;
    private readonly IEventRoutingService _eventRoutingService;

    public SetSubSceneGameCommandHandler(
        ConservationGameData gameData,
        IEventRoutingService eventRoutingService)
    {
        _gameData = gameData;
        _eventRoutingService = eventRoutingService;
    }

    public bool HandleCommand(SetSubSceneGameCommand command)
    {
        var mainSubScenes = new HashSet<string>
        {
            Constants.SubScene_KakapoDetails,
            Constants.SubScene_StaffDetails
        };
        var mainSubSceneScreenStates = new HashSet<ScreenState>
        {
            ScreenState.Kakapo,
            ScreenState.Staff
        };

        if (mainSubScenes.Contains(command.Id) &&
            mainSubSceneScreenStates.Contains(_gameData.InteractionData.ScreenState))
        {
            _eventRoutingService.InvokePopSubScene(new PopSubSceneGameCommand { Clear = true });
            _gameData.InteractionData.ScreenState = ScreenState.Default;
        }

        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
            // TODO: Handle pushing on top of current rather than only from default???
            if (command.Id is Constants.SubScene_KakapoDetails)
            {
                _gameData.InteractionData.ScreenState = ScreenState.Kakapo;
            }
            else if (command.Id is Constants.SubScene_StaffDetails)
            {
                _gameData.InteractionData.ScreenState = ScreenState.Staff;
            }

            _eventRoutingService.InvokeSetSubScene(command);

            return true;
        }

        return false;
    }
}
