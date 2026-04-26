namespace Idology.Conservation.Core.Services;

internal class ConservationGameInteractionService : IConservationGameInteractionService
{
    private readonly ConservationGameData _gameData;
    private readonly IInputManager _inputManager;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public ConservationGameInteractionService(
        ConservationGameData gameData,
        IInputManager inputManager,
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameData = gameData;
        _inputManager = inputManager;
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public void Update(float delta)
    {
        // TODO: MOVE THIS TO THE SUB SCENE?
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
            else if (_inputManager.HandleActionIfInvoked(Constants.Action_Shortcut_Kakapo))
            {
                _gameData.InteractionData.ScreenState = ScreenState.Kakapo;
            }
            else if (_inputManager.HandleActionIfInvoked(Constants.Action_Shortcut_Staff))
            {
                _gameData.InteractionData.ScreenState = ScreenState.Staff;
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
        else if (_gameData.InteractionData.ScreenState is ScreenState.Kakapo)
        {
            if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
            {
                _gameData.InteractionData.ScreenState = ScreenState.Default;
            }
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Staff)
        {
            if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
            {
                _gameData.InteractionData.ScreenState = ScreenState.Default;
            }
        }
    }
}
