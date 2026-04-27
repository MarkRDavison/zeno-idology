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
        var mainSubScenes = new Dictionary<string, ScreenState>
        {
            { Constants.SubScene_KakapoDetails, ScreenState.Kakapo },
            { Constants.SubScene_StaffDetails, ScreenState.Staff },
            { Constants.SubScene_ResearchDetails, ScreenState.Research },
            { Constants.SubScene_TechnologyDetails, ScreenState.Technology },
            { Constants.SubScene_FundingDetails, ScreenState.Funding }
        };

        var mainSubSceneScreenStates = new HashSet<ScreenState>
        {
            ScreenState.Kakapo,
            ScreenState.Staff,
            ScreenState.Research,
            ScreenState.Technology,
            ScreenState.Funding
        };

        if (mainSubScenes.ContainsKey(command.Id) &&
            mainSubSceneScreenStates.Contains(_gameData.InteractionData.ScreenState))
        {
            _eventRoutingService.InvokePopSubScene(new PopSubSceneGameCommand { Clear = true });
            _gameData.InteractionData.ScreenState = ScreenState.Default;
        }

        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
            // TODO: Handle pushing on top of current rather than only from default???
            if (mainSubScenes.TryGetValue(command.Id, out var newScreenState))
            {
                _gameData.InteractionData.ScreenState = newScreenState;
            }

            _eventRoutingService.InvokeSetSubScene(command);

            return true;
        }

        return false;
    }
}
