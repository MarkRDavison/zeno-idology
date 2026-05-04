namespace Idology.Conservation.Core.Services;

internal class ConservationGameInteractionService : IConservationGameInteractionService
{
    private readonly IConservationStateService _gameState;
    private readonly IInputManager _inputManager;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IGameCommandService _gameCommandService;
    private readonly ConservationGameCamera _conservationGameCamera;

    public ConservationGameInteractionService(
        IConservationStateService gameState,
        IInputManager inputManager,
        IGameDateTimeProvider gameDateTimeProvider,
        IGameCommandService gameCommandService,
        ConservationGameCamera conservationGameCamera)
    {
        _gameState = gameState;
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
                if (_gameCommandService.HandleCommand(new SetScreenStateGameCommand(state)))
                {
                    _inputManager.HandleActionIfInvoked(shortcut);
                }
            }
        }

        if (_gameState.State.InteractionData.ScreenState is ScreenState.Default)
        {
            HandleDefaultInteraction();
        }
        else if (_gameState.State.InteractionData.ScreenState is ScreenState.Region)
        {
            HandleRegionInteraction();
        }
        else if (subSceneStates.Contains(_gameState.State.InteractionData.ScreenState))
        {
            HandleSubStateInteraction(_gameState.State.InteractionData.ScreenState);
        }
    }

    private void HandleDefaultInteraction()
    {
        if (_inputManager.HandleActionIfInvoked(Constants.Action_Escape))
        {
            _gameCommandService.EnqueueCommand(new DeselectRegionGameCommand());
        }
        else if (_inputManager.HandleActionIfInvoked(Constants.Action_PlayPause))
        {
            // TODO: To game command...
            _gameDateTimeProvider.SetPauseState(!_gameDateTimeProvider.IsPaused);
        }
        else if (_inputManager.HandleActionIfInvoked(Constants.Action_CycleRegion))
        {
            int newRegionId;

            if (_gameState.State.InteractionData.DefaultScreenData.SelectedRegion is { } regionId)
            {
                newRegionId = (regionId + 1) % _gameState.State.Regions.Count;
            }
            else
            {
                newRegionId = 0;
            }

            _gameCommandService.EnqueueCommand(new SelectRegionGameCommand(newRegionId));
        }
        else if (_gameState.State.InteractionData.DefaultScreenData.SelectedRegion is { } regionId &&
            _inputManager.HandleActionIfInvoked(Constants.Action_Enter))
        {
            _gameCommandService.EnqueueCommand(new OpenRegionGameCommand(regionId));
        }
        else if (_inputManager.IsActionInvoked(Constants.Action_Click))
        {
            var mousePosition = _inputManager.GetMousePosition(_conservationGameCamera.Camera);

            var tileX = (int)(mousePosition.X / Constants.TileSize);
            var tileY = (int)(mousePosition.Y / Constants.TileSize);

            var idx = 0;

            foreach (var r in _gameState.State.Regions)
            {
                if (r.RegionOffset.X <= tileX && tileX <= r.RegionOffset.X + r.Width &&
                    r.RegionOffset.Y <= tileY && tileY <= r.RegionOffset.Y + r.Height)
                {
                    _inputManager.MarkActionAsHandled(Constants.Action_Click);

                    _gameCommandService.EnqueueCommand(new SelectRegionGameCommand(idx));

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
            // TODO: Maybe we want to check if the info panel is open before closing?
            // Fall back from the kakapo summary to the region info panel?
            _gameCommandService.EnqueueCommand(new CloseRegionScreenGameCommand());
        }
        else if (_inputManager.IsActionInvoked(Constants.Action_Click) && _gameState.State.ActiveRegion is { } activeRegion)
        {
            var mousePosition = _inputManager.GetMousePosition(_conservationGameCamera.Camera) / 64.0f;

            int? selectedKakapoId = null;

            // TODO: Maybe there are multiple, so gotta go through all of them to find the best match????
            foreach (var k in _gameState.State.SimulatedKakapo.Where(_ => _.RegionId == activeRegion.Id))
            {
                if (Vector2.DistanceSquared(mousePosition, k.CurrentLocation + activeRegion.RegionOffset) < 3.0f)
                {
                    selectedKakapoId = k.KakapoId;
                    break;
                }
            }

            if (selectedKakapoId is { } id)
            {
                _inputManager.MarkActionAsHandled(Constants.Action_Click);

                _gameCommandService.EnqueueCommand(new SelectKakapoWithinRegionGameCommand(activeRegion.Id, id));
            }
        }
    }

    private void HandleSubStateInteraction(ScreenState screenState)
    {
        if (_inputManager.IsActionInvoked(Constants.Action_Escape))
        {
            switch (screenState)
            {
                default:
                    Console.Error.WriteLine("UNHANDLED escape sub screen action");
                    break;
            }
            //if (_gameCommandService.EnqueueCommand(new ScreenStateGameCommand { ScreenState = ScreenState.Default }))
            //{
            //    _inputManager.HandleActionIfInvoked(Constants.Action_Escape);
            //}
        }
    }
}
