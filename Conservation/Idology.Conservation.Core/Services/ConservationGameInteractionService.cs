namespace Idology.Conservation.Core.Services;

internal class ConservationGameInteractionService : IConservationGameInteractionService
{
    private readonly ConservationGameData _gameData;
    private readonly IInputManager _inputManager;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IGameCommandService _gameCommandService;
    private readonly ConservationGameCamera _conservationGameCamera;

    public ConservationGameInteractionService(
        ConservationGameData gameData,
        IInputManager inputManager,
        IGameDateTimeProvider gameDateTimeProvider,
        IGameCommandService gameCommandService,
        ConservationGameCamera conservationGameCamera)
    {
        _gameData = gameData;
        _inputManager = inputManager;
        _gameDateTimeProvider = gameDateTimeProvider;
        _gameCommandService = gameCommandService;
        _conservationGameCamera = conservationGameCamera;
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

        var shortcutsToScreenStates = new Dictionary<string, ScreenState>
        {
            { Constants.Action_Shortcut_Kakapo, ScreenState.Kakapo },
            { Constants.Action_Shortcut_Staff, ScreenState.Staff },
            { Constants.Action_Shortcut_Research, ScreenState.Research },
            { Constants.Action_Shortcut_Technology, ScreenState.Technology },
            { Constants.Action_Shortcut_Funding, ScreenState.Funding }
        };

        foreach (var (shortcut, state) in shortcutsToScreenStates)
        {
            if (_inputManager.HandleActionIfInvoked(shortcut))
            {
                if (_gameCommandService.EnqueueCommand(new SetScreenStateGameCommand { ScreenState = state }))
                {
                    _inputManager.HandleActionIfInvoked(shortcut);
                }
            }
        }

        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
            HandleDefaultInteraction();
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Region)
        {
            HandleRegionInteraction();
        }
        else if (subSceneStates.Contains(_gameData.InteractionData.ScreenState))
        {
            HandleSubStateInteraction();
        }
    }

    private void HandleDefaultInteraction()
    {
        if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
        {
            _gameData.InteractionData.DefaultScreenData.SelectedRegion = null;

            _gameCommandService.EnqueueCommand(new SetInfoScreenGameCommand
            {
                Open = false,
                State = InfoState.Hidden,
                Context = null
            });
        }
        else if (_inputManager.HandleActionIfInvoked(Constants.Action_PlayPause))
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

            if (_gameData.InteractionData.DefaultScreenData.SelectedRegion is not null)
            {
                _gameCommandService.EnqueueCommand(new SetInfoScreenGameCommand
                {
                    Open = true,
                    State = InfoState.RegionSummary,
                    Context = new RegionInfoScreenPayload(_gameData.InteractionData.DefaultScreenData.SelectedRegion.Value, true)
                });
            }
        }
        else if (_gameData.InteractionData.DefaultScreenData.SelectedRegion is not null &&
            _inputManager.HandleActionIfInvoked(Constants.Action_Enter))
        {
            // TODO: Push and pop camera/transform matrices or position/zoom levels???
            _gameData.InteractionData.ScreenState = ScreenState.Region;
            _gameData.ActiveRegion = _gameData.Regions[_gameData.InteractionData.DefaultScreenData.SelectedRegion.Value];

            _gameCommandService.EnqueueCommand(new SetInfoScreenGameCommand
            {
                Open = true,
                State = InfoState.Region,
                Context = new RegionInfoScreenPayload(_gameData.InteractionData.DefaultScreenData.SelectedRegion.Value, false)
            });
        }
        else if (_inputManager.IsActionInvoked(Constants.Action_Click))
        {
            var mousePosition = _inputManager.GetMousePosition(_conservationGameCamera.Camera);

            var tileX = (int)(mousePosition.X / Constants.TileSize);
            var tileY = (int)(mousePosition.Y / Constants.TileSize);

            var idx = 0;

            foreach (var r in _gameData.Regions)
            {
                if (r.RegionOffset.X <= tileX && tileX <= r.RegionOffset.X + r.Width &&
                    r.RegionOffset.Y <= tileY && tileY <= r.RegionOffset.Y + r.Height)
                {
                    _inputManager.MarkActionAsHandled(Constants.Action_Click);

                    if (_gameData.InteractionData.DefaultScreenData.SelectedRegion != idx)
                    {
                        _gameData.InteractionData.DefaultScreenData.SelectedRegion = idx;

                        _gameCommandService.EnqueueCommand(new SetInfoScreenGameCommand
                        {
                            Open = true,
                            State = InfoState.RegionSummary,
                            Context = new RegionInfoScreenPayload(idx, true)
                        });
                    }

                    break;
                }

                idx++;
            }
        }
    }

    private void HandleRegionInteraction()
    {
        if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
        {
            _gameData.InteractionData.ScreenState = ScreenState.Default;
            _gameData.InteractionData.DefaultScreenData.SelectedRegion = null;
            _gameData.ActiveRegion = null;

            _gameCommandService.EnqueueCommand(new SetInfoScreenGameCommand
            {
                Open = false,
                State = InfoState.Hidden,
                Context = null
            });
        }
    }

    private void HandleSubStateInteraction()
    {
        if (_inputManager.IsActionInvoked(Constants.Action_Escape))
        {
            if (_gameCommandService.EnqueueCommand(new SetScreenStateGameCommand { ScreenState = ScreenState.Default }))
            {
                _inputManager.HandleActionIfInvoked(Constants.Action_Escape);
            }
        }
    }
}
