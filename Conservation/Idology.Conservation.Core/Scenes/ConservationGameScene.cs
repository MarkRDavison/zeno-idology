namespace Idology.Conservation.Core.Scenes;

public class ConservationGameScenePayload : IScenePayload<ConservationGameScene>
{
    public required bool Load { get; init; }
    public required bool Dev { get; init; }
}

public class ConservationGameScene : ConservationScene<ConservationGameScene>
{
    private readonly IConservationStateService _gameState;
    private readonly ConservationGame _game;
    private readonly IInputManager _inputManager;
    private readonly IConservationGameInteractionService _conservationGameInteractionService;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventRoutingService _eventRoutingService;
    private readonly IGameCommandService _gameCommandService;
    private readonly ConservationGameCamera _camera;

    private IWidget? _kakapoDetailsSubSceneWidget;
    private IWidget? _staffDetialsSubSceneWidget;
    private IWidget? _researchSubSceneWidget;
    private IWidget? _technologySubSceneWidget;
    private IWidget? _fundingSubSceneWidget;

    private IWidget? _lastActiveSubSceneWidget;

    public ConservationGameScene(
        IConservationStateService gameState,
        ConservationGame game,
        ConservationGameCamera camera,
        IInputManager inputManager,
        IConservationGameInteractionService conservationGameInteractionService,
        IGameDateTimeProvider gameDateTimeProvider,
        IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider,
        IEventRoutingService eventRoutingService,
        IGameCommandService gameCommandService)
    {
        _gameState = gameState;
        _game = game;
        _inputManager = inputManager;
        _conservationGameInteractionService = conservationGameInteractionService;
        _gameDateTimeProvider = gameDateTimeProvider;
        _userInterfaceRoot = userInterfaceRoot;
        _serviceProvider = serviceProvider;
        _eventRoutingService = eventRoutingService;
        _gameCommandService = gameCommandService;

        _camera = camera;
    }

    public override void Init(IScenePayload<ConservationGameScene>? payload)
    {
        // TODO: Dispose.
        _eventRoutingService.OpenScreenPanel += OnOpenScreenPanel;
        _eventRoutingService.CloseScreenPanel += OnCloseScreenPanel;

        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        {
            _gameState.SetState(ConservationStateInitializationMutations.CreateDefaultData());

            _gameDateTimeProvider.Set(new DateTime(2026, 01, 01));

            var kakapoData = new List<KakapoModel>();
            // TODO: Load from files....
            {
                // TODO: Validate data, that
                // FatherId -> Male
                // MotherId -> Female
                // Birth/death dates in order
                // Birth of parent comes before birth of child
                // Death of parent comes after birth of child ??? (Maybe not true if egg laid, then parent died, then baby hatched???)
                // https://encyclopedia.pub/entry/37611

                kakapoData.Add(new KakapoModel(1, "Flossie", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), null, 1));
                kakapoData.Add(new KakapoModel(2, "Solstice", Gender.Female, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered, "LAST_DISCOVERD_WILD"), null, 1));
                kakapoData.Add(new KakapoModel(3, "Nora", Gender.Female, null, null, new OriginInfo(new DateOnly(1980, 1, 1), OriginDateType.Discovered), null, 1));
                kakapoData.Add(new KakapoModel(4, "Rakiura", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 19), OriginDateType.KnownBirth), null, 1));
                kakapoData.Add(new KakapoModel(5, "Esperance", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 17), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(6, "Margaret-Maree", Gender.Female, null, null, new OriginInfo(new DateOnly(1986, 1, 1), OriginDateType.Discovered), null, 1));
                kakapoData.Add(new KakapoModel(7, "Marama", Gender.Female, 6, 24, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(8, "Stella", Gender.Female, 2, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(9, "Heather", Gender.Female, 1, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(10, "Ako", Gender.Female, 5, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(11, "Alice", Gender.Female, null, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(12, "Lisa", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(13, "Wendy", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(14, "Atareta", Gender.Female, 12, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(15, "Tia", Gender.Female, 4, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(16, "Toitiiti", Gender.Female, 4, null, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(17, "Yasmine", Gender.Female, 1, null, new OriginInfo(new DateOnly(2005, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(18, "Zephyr", Gender.Female, 3, 25, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(19, "Vori", Gender.Female, 11, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(20, "Tohu", Gender.Female, 9, null, new OriginInfo(new DateOnly(2014, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(21, "Aparima", Gender.Female, 13, null, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(22, "JEM", Gender.Female, 21, 23, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                kakapoData.Add(new KakapoModel(23, "Bill", Gender.Male, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), new DateOnly(2008, 3, 1), null));
                kakapoData.Add(new KakapoModel(24, "Nog", Gender.Male, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered), null, 1));
                kakapoData.Add(new KakapoModel(25, "Rangi", Gender.Male, null, null, new OriginInfo(new DateOnly(1987, 1, 1), OriginDateType.Discovered), null, 1));

            }

            var staffData = new List<StaffData>();
            // TODO: Load from files....
            {
                staffData.Add(new(1, "Tom"));
                staffData.Add(new(2, "Sarah"));
                staffData.Add(new(3, "Theo"));
                staffData.Add(new(4, "Danielle"));
                staffData.Add(new(5, "Vol-1"));
                staffData.Add(new(6, "Vol-2"));
                staffData.Add(new(7, "Vol-3"));
                staffData.Add(new(8, "Vol-4"));
            }

            List<string> regionNames = ["region-1", "region-2", "region-3", "region-4"];

            var kakapoLocationsByRegionId = new Dictionary<int, HashSet<Vector2>>();

            var regions = new List<RegionData>();
            var regionSimulations = new List<RegionSimulation>();

            foreach (var r in regionNames)
            {
                var regionData = RegionModel.Create(r);

                regions.Add(regionData.ToRegionData());
                regionSimulations.Add(new RegionSimulation(regionData.RegionModelData.Id, _gameState));
                kakapoLocationsByRegionId.Add(regionData.RegionModelData.Id, []);
            }

            var simulatedKakapo = new List<KakapoSimulationData>();

            foreach (var k in kakapoData)
            {
                if (k.RegionId is not null)
                {
                    var invalidLocations = kakapoLocationsByRegionId[k.RegionId.Value];
                    var region = regions.First(_ => _.Id == k.RegionId.Value);

                    while (true)
                    {
                        var attemptedLocation = new Vector2(Random.Shared.Next(0, region.Width), Random.Shared.Next(0, region.Height));

                        if (invalidLocations.Contains(attemptedLocation))
                        {
                            continue;
                        }

                        var tile = region.Tiles[(int)(attemptedLocation.Y * region.Width + attemptedLocation.X)];

                        // TODO: Add an exclusion zone???
                        invalidLocations.Add(attemptedLocation);

                        // TODO: Extension method for is valid location or something...
                        if (tile.TileType is TileType.Unset or TileType.Water or TileType.Cliff or TileType.Beach)
                        {
                            continue;
                        }

                        simulatedKakapo.Add(new KakapoSimulationData(
                            k.Id,
                            k.RegionId.Value,
                            attemptedLocation));

                        break;
                    }
                }
            }

            // TODO: Shuffle kakapo over the island when starting new game, need to persist so it can be loaded later.

            if (payload is ConservationGameScenePayload cgsp)
            {
                if (cgsp.Load)
                {
                    if (cgsp.Dev)
                    {

                    }
                    else
                    {

                    }
                }
            }

            _gameState.SetState(_ => _
                .WithSetStaffData(staffData)
                .WithSetKakapoData(kakapoData)
                .WithSetKakapoSimulations(simulatedKakapo)
                .WithSetRegionData(regions)
                .WithSetRegionSimulations(regionSimulations));
        }

        InitUserInterface();
    }

    private void OnCloseScreenPanel(object? sender, CloseScreenPanelPayload e)
    {
        _lastActiveSubSceneWidget?.Layout.Visibility = Visibility.Collapsed;
    }

    private void OnOpenScreenPanel(object? sender, OpenScreenPanelPayload e)
    {
        _lastActiveSubSceneWidget?.Layout.Visibility = Visibility.Collapsed;

        // TODO: Exhaustive static analysis?
        _lastActiveSubSceneWidget = _gameState.State.InteractionData.PanelState switch
        {
            ScreenPanelState.Kakapo => _kakapoDetailsSubSceneWidget,
            ScreenPanelState.Staff => _staffDetialsSubSceneWidget,
            ScreenPanelState.Research => _researchSubSceneWidget,
            ScreenPanelState.Technology => _technologySubSceneWidget,
            ScreenPanelState.Funding => _fundingSubSceneWidget,
            _ => null
        };

        _lastActiveSubSceneWidget?.Layout.Visibility = Visibility.Visible;
    }

    private void InitUserInterface()
    {
        var root = _userInterfaceRoot.RootWidget;
        root.Layout.Behave = BehaveFlags.Top | BehaveFlags.HFill;
        root.Layout.Contain = ContainFlags.Column;
        root.Layout.Align = AlignFlags.Start;

        root.AddChild(_serviceProvider.GetRequiredService<TopBarWidget>());

        var contentPanel = root.AddChild(new PanelWidget
        {
            Layout = new LayoutItem
            {
                Behave = BehaveFlags.Fill,
                Contain = ContainFlags.Layout
            }
        });

        contentPanel.AddChild(_serviceProvider.GetRequiredService<InfoContextPanelWidget>());

        {
            _kakapoDetailsSubSceneWidget = contentPanel.AddChild(_serviceProvider.GetRequiredService<KakapoDetailsUiSubScenePanelWidget>());
            _kakapoDetailsSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _staffDetialsSubSceneWidget = contentPanel.AddChild(_serviceProvider.GetRequiredService<StaffDetailsUiSubScenePanelWidget>());
            _staffDetialsSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _researchSubSceneWidget = contentPanel.AddChild(_serviceProvider.GetRequiredService<ResearchUiSubScenePanelWidget>());
            _researchSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _technologySubSceneWidget = contentPanel.AddChild(_serviceProvider.GetRequiredService<TechnologyUiSubScenePanelWidget>());
            _technologySubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _fundingSubSceneWidget = contentPanel.AddChild(_serviceProvider.GetRequiredService<FundingUiSubScenePanelWidget>());
            _fundingSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
    }

    public override void Update(float delta)
    {
        if (Raylib.IsWindowResized())
        {
            _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        }

        _game.Update(delta);

        _userInterfaceRoot.Update(delta);

        if (_gameState.State.InteractionData.PanelState is ScreenPanelState.None)
        {
            _camera.Update(delta);
        }

        _conservationGameInteractionService.Update(delta);

        _gameCommandService.HandleEnqueuedCommands();

        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Blue);

        DrawRootScene(_camera.Camera);

        _userInterfaceRoot.RootWidget.Draw();

        Raylib.EndDrawing();
    }

    private void DrawRootScene(Camera2D camera)
    {
        int TileSize = (int)Constants.TileSize;

        if (_gameState.State.InteractionData.MainScreenState is MainScreenState.Region)
        {
            if (_gameState.State.ActiveRegion is { } activeRegion)
            {
                Raylib.BeginMode2D(camera);

                for (int y = 0; y < activeRegion.Height; ++y)
                {
                    for (int x = 0; x < activeRegion.Width; ++x)
                    {
                        var tile = activeRegion.Tiles[y * activeRegion.Width + x];

                        if (tile.TileType is TileType.Water or TileType.Unset)
                        {
                            continue;
                        }

                        Raylib.DrawRectangle(
                            x * TileSize + (int)activeRegion.RegionOffset.X * TileSize,
                            y * TileSize + (int)activeRegion.RegionOffset.Y * TileSize,
                            TileSize,
                            TileSize,
                            tile.Color);
                    }
                }

                foreach (var k in _gameState.State.SimulatedKakapo.Where(_ => _.RegionId == activeRegion.Id))
                {
                    Raylib.DrawRectangle(
                        (int)k.CurrentLocation.X * TileSize + (int)activeRegion.RegionOffset.X * TileSize,
                        (int)k.CurrentLocation.Y * TileSize + (int)activeRegion.RegionOffset.Y * TileSize,
                        TileSize,
                        TileSize,
                        _gameState.State.InteractionData.RegionScreenData.SelectedKakapoId == k.KakapoId ? Color.Yellow : Color.Magenta);
                }

                Raylib.EndMode2D();
            }
        }
        else if (_gameState.State.InteractionData.MainScreenState is MainScreenState.Default)
        {
            Raylib.BeginMode2D(camera);

            int regionIdx = 0;

            foreach (var region in _gameState.State.Regions)
            {
                for (int y = 0; y < region.Height; ++y)
                {
                    for (int x = 0; x < region.Width; ++x)
                    {
                        var tile = region.Tiles[y * region.Width + x];

                        if (tile.TileType is TileType.Water or TileType.Unset)
                        {
                            continue;
                        }

                        Raylib.DrawRectangle(
                            x * TileSize + (int)region.RegionOffset.X * TileSize,
                            y * TileSize + (int)region.RegionOffset.Y * TileSize,
                            TileSize,
                            TileSize,
                            tile.Color);
                    }
                }

                if (_gameState.State.InteractionData.DefaultScreenData.SelectedRegion == regionIdx)
                {
                    // TODO: Width needs to be adjusted based on zoom level...
                    // Keeping it the same looks weird, so it can get smaller just needs a minimum width
                    Raylib.DrawRectangleLinesEx(
                        new Rectangle(
                            new Vector2(
                                (int)region.RegionOffset.X * TileSize,
                                (int)region.RegionOffset.Y * TileSize),
                            new Vector2(
                                TileSize * region.Width,
                                TileSize * region.Height)
                            ),
                        (float)(TileSize / 16.0f) / _camera.Zoom,
                        Color.White);
                }

                regionIdx++;
            }

            Raylib.EndMode2D();
        }
        else
        {
            Console.Error.WriteLine("INVALID MAIN SCENE RENDERING");
        }
    }
}
