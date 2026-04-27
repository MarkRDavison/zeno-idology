namespace Idology.Conservation.Core.Services;

internal class ConservationGameInteractionService : IConservationGameInteractionService
{
    private readonly ConservationGameData _gameData;
    private readonly IInputManager _inputManager;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IGameCommandService _gameCommandService;

    public ConservationGameInteractionService(
        ConservationGameData gameData,
        IInputManager inputManager,
        IGameDateTimeProvider gameDateTimeProvider,
        IGameCommandService gameCommandService)
    {
        _gameData = gameData;
        _inputManager = inputManager;
        _gameDateTimeProvider = gameDateTimeProvider;
        _gameCommandService = gameCommandService;
    }

    public void Update(float delta)
    {
        var subSceneStates = new HashSet<ScreenState>
        {
            ScreenState.Kakapo,
            ScreenState.Staff,
            ScreenState.Research,
            ScreenState.Technology,
            ScreenState.Funding
        };
        var shortcutsToSceneIds = new Dictionary<string, string>
        {
            { Constants.Action_Shortcut_Kakapo, Constants.SubScene_KakapoDetails },
            { Constants.Action_Shortcut_Staff, Constants.SubScene_StaffDetails },
            { Constants.Action_Shortcut_Research, Constants.SubScene_ResearchDetails },
            { Constants.Action_Shortcut_Technology, Constants.SubScene_TechnologyDetails },
            { Constants.Action_Shortcut_Funding, Constants.SubScene_FundingDetails }
        };

        foreach (var (shortcut, sceneId) in shortcutsToSceneIds)
        {
            if (_inputManager.HandleActionIfInvoked(shortcut))
            {
                if (_gameCommandService.HandleCommand(new SetSubSceneGameCommand { Id = sceneId }))
                {
                    _inputManager.HandleActionIfInvoked(shortcut);
                }
            }
        }

        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
            if (_inputManager.HandleActionIfInvoked(Constants.Action_PlayPause))
            {
                _gameDateTimeProvider.SetPauseState(!_gameDateTimeProvider.IsPaused);
            }
            else if (_inputManager.HandleActionIfInvoked(Constants.Action_CycleRegion))
            {
                if (_gameData.InteractionData.DefaultScreenData.SelectedRegion is null)
                {
                    _gameData.InteractionData.DefaultScreenData.SelectedRegion = 0;
                }
                else
                {
                    _gameData.InteractionData.DefaultScreenData.SelectedRegion = (_gameData.InteractionData.DefaultScreenData.SelectedRegion + 1) % _gameData.Regions.Count;
                }
            }
            else if (_gameData.InteractionData.DefaultScreenData.SelectedRegion is not null &&
                _inputManager.HandleActionIfInvoked(Constants.Action_Enter))
            {
                // TODO: Push and pop camera/transform matrices???
                _gameData.InteractionData.ScreenState = ScreenState.Region;
                _gameData.ActiveRegion = _gameData.Regions[_gameData.InteractionData.DefaultScreenData.SelectedRegion.Value];
            }
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Region)
        {
            if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
            {
                _gameData.InteractionData.ScreenState = ScreenState.Default;
                _gameData.InteractionData.DefaultScreenData.SelectedRegion = null;
                _gameData.ActiveRegion = null;
            }
        }
        else if (subSceneStates.Contains(_gameData.InteractionData.ScreenState))
        {
            if (_inputManager.IsActionInvoked(Constants.Action_Escape))
            {
                if (_gameCommandService.HandleCommand(new PopSubSceneGameCommand()))
                {
                    _inputManager.HandleActionIfInvoked(Constants.Action_Escape);
                }
            }
        }
    }
}
