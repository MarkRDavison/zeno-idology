using Idology.Conservation.Core.Services.GameCommands.OpenPanelState;
using Idology.Conservation.Core.Services.GameCommands.SetTimeMode;

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
        var panelStates = new HashSet<ScreenPanelState>
        {
            ScreenPanelState.Kakapo,
            ScreenPanelState.Staff,
            ScreenPanelState.Research,
            ScreenPanelState.Technology,
            ScreenPanelState.Funding
        };

        var shortcutsToScreenStates = new Dictionary<string, ScreenPanelState>
        {
            { Constants.Action_Shortcut_Kakapo, ScreenPanelState.Kakapo },
            { Constants.Action_Shortcut_Staff, ScreenPanelState.Staff },
            { Constants.Action_Shortcut_Research, ScreenPanelState.Research },
            { Constants.Action_Shortcut_Technology, ScreenPanelState.Technology },
            { Constants.Action_Shortcut_Funding, ScreenPanelState.Funding }
        };

        foreach (var (shortcut, state) in shortcutsToScreenStates)
        {
            if (_inputManager.HandleActionIfInvoked(shortcut))
            {
                if (_gameCommandService.HandleCommand(new OpenPanelStateGameCommand(state, null)))
                {
                    _inputManager.HandleActionIfInvoked(shortcut);
                }
            }
        }

        if (panelStates.Contains(_gameState.State.InteractionData.PanelState))
        {
            HandleSubStateInteraction(_gameState.State.InteractionData.PanelState);
        }
        else if (_gameState.State.InteractionData.MainScreenState is MainScreenState.Default)
        {
            HandleDefaultInteraction();
        }
        else if (_gameState.State.InteractionData.MainScreenState is MainScreenState.Region)
        {
            HandleRegionInteraction();
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
            _gameCommandService.HandleCommand(new SetTimeModeGameCommand(TimeMode.Paused));
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

    private void HandleSubStateInteraction(ScreenPanelState panelState)
    {
        if (_inputManager.IsActionInvoked(Constants.Action_Escape))
        {
            switch (panelState)
            {
                default:
                    Console.Error.WriteLine("UNHANDLED escape panel action");
                    break;
            }
        }
    }
}
