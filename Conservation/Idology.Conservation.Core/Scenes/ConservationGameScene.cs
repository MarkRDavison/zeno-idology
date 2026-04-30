namespace Idology.Conservation.Core.Scenes;

public class ConservationGameScenePayload : IScenePayload<ConservationGameScene>
{
    public required bool Load { get; init; }
    public required bool Dev { get; init; }
}

public class ConservationGameScene : ConservationScene<ConservationGameScene>
{
    private readonly ConservationGameData _gameData;
    private readonly ConservationGame _game;
    private readonly IInputManager _inputManager;
    private readonly IConservationGameInteractionService _conservationGameInteractionService;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventRoutingService _eventRoutingService;
    private readonly ConservationGameCamera _camera;

    private IWidget? _kakapoDetailsSubSceneWidget;
    private IWidget? _staffDetialsSubSceneWidget;
    private IWidget? _researchSubSceneWidget;
    private IWidget? _technologySubSceneWidget;
    private IWidget? _fundingSubSceneWidget;

    private IWidget? _lastActiveSubSceneWidget;

    public ConservationGameScene(
        ConservationGameData gameData,
        ConservationGame game,
        IInputManager inputManager,
        IConservationGameInteractionService conservationGameInteractionService,
        IGameDateTimeProvider gameDateTimeProvider,
        IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider,
        IEventRoutingService eventRoutingService)
    {
        _gameData = gameData;
        _game = game;
        _inputManager = inputManager;
        _conservationGameInteractionService = conservationGameInteractionService;
        _gameDateTimeProvider = gameDateTimeProvider;
        _userInterfaceRoot = userInterfaceRoot;
        _serviceProvider = serviceProvider;
        _eventRoutingService = eventRoutingService;

        _camera = new ConservationGameCamera(inputManager)
        {
            // TODO: Better way of applying defaults...
            Target = new Vector2(4500, 4000),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
            Zoom = 0.10f
        };
    }

    public override void Init(IScenePayload<ConservationGameScene>? payload)
    {
        // TODO: Dispose.
        _eventRoutingService.SetScreenState += OnSetScreenState;

        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        {
            _gameData.ActiveRegion = null;
            _gameData.Regions.Clear();

            _gameDateTimeProvider.Set(new DateTime(2026, 01, 01));

            // TODO: Load from files....
            {
                // TODO: Validate data, that
                // FatherId -> Male
                // MotherId -> Female
                // Birth/death dates in order
                // Birth of parent comes before birth of child
                // Death of parent comes after birth of child ??? (Maybe not true if egg laid, then parent died, then baby hatched???)
                // https://encyclopedia.pub/entry/37611
                _gameData.KakapoData.Add(new KakapoModel(1, "Flossie", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(2, "Solstice", Gender.Female, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered, "LAST_DISCOVERD_WILD"), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(3, "Nora", Gender.Female, null, null, new OriginInfo(new DateOnly(1980, 1, 1), OriginDateType.Discovered), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(4, "Rakiura", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 19), OriginDateType.KnownBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(5, "Esperance", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 17), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(6, "Margaret-Maree", Gender.Female, null, null, new OriginInfo(new DateOnly(1986, 1, 1), OriginDateType.Discovered), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(7, "Marama", Gender.Female, 6, 24, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(8, "Stella", Gender.Female, 2, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(9, "Heather", Gender.Female, 1, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(10, "Ako", Gender.Female, 5, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(11, "Alice", Gender.Female, null, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(12, "Lisa", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(13, "Wendy", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(14, "Atareta", Gender.Female, 12, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(15, "Tia", Gender.Female, 4, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(16, "Toitiiti", Gender.Female, 4, null, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(17, "Yasmine", Gender.Female, 1, null, new OriginInfo(new DateOnly(2005, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(18, "Zephyr", Gender.Female, 3, 25, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(19, "Vori", Gender.Female, 11, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(20, "Tohu", Gender.Female, 9, null, new OriginInfo(new DateOnly(2014, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(21, "Aparima", Gender.Female, 13, null, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(22, "JEM", Gender.Female, 21, 23, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(23, "Bill", Gender.Male, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), new DateOnly(2008, 3, 1), null));
                _gameData.KakapoData.Add(new KakapoModel(24, "Nog", Gender.Male, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered), null, 1));
                _gameData.KakapoData.Add(new KakapoModel(25, "Rangi", Gender.Male, null, null, new OriginInfo(new DateOnly(1987, 1, 1), OriginDateType.Discovered), null, 1));
            }

            // TODO: Load from files....
            {
                _gameData.StaffData.Add(new(1, "Tom"));
                _gameData.StaffData.Add(new(2, "Sarah"));
                _gameData.StaffData.Add(new(3, "Theo"));
                _gameData.StaffData.Add(new(4, "Danielle"));
                _gameData.StaffData.Add(new(5, "Vol-1"));
                _gameData.StaffData.Add(new(6, "Vol-2"));
                _gameData.StaffData.Add(new(7, "Vol-3"));
                _gameData.StaffData.Add(new(8, "Vol-4"));
            }

            List<string> regions = ["region-1", "region-2", "region-3", "region-4"];

            var kakapoLocationsByRegionId = new Dictionary<int, HashSet<Vector2>>();

            foreach (var r in regions)
            {
                var regionData = RegionModel.Create(r);

                _gameData.Regions.Add(regionData.ToRegionData());
                _gameData.RegionSimulations.Add(new RegionSimulation(regionData.RegionModelData.Id, _gameData));
                kakapoLocationsByRegionId.Add(regionData.RegionModelData.Id, []);
            }

            foreach (var k in _gameData.KakapoData)
            {
                if (k.RegionId is not null)
                {
                    var invalidLocations = kakapoLocationsByRegionId[k.RegionId.Value];
                    var region = _gameData.Regions.First(_ => _.Id == k.RegionId.Value);

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

                        if (tile.TileType is TileType.Unset or TileType.Water)
                        {
                            continue;
                        }

                        _gameData.SimulatedKakapo.Add(new KakapoSimulationData(
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
        }

        InitUserInterface();
    }

    private void OnSetScreenState(object? sender, SetScreenStateGameCommand e)
    {
        _lastActiveSubSceneWidget?.Layout.Visibility = Visibility.Collapsed;

        _lastActiveSubSceneWidget = e.ScreenState switch
        {
            ScreenState.Kakapo => _kakapoDetailsSubSceneWidget,
            ScreenState.Staff => _staffDetialsSubSceneWidget,
            ScreenState.Research => _researchSubSceneWidget,
            ScreenState.Technology => _technologySubSceneWidget,
            ScreenState.Funding => _fundingSubSceneWidget,
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

        _conservationGameInteractionService.Update(delta);

        _game.Update(delta);

        _userInterfaceRoot.Update(delta);

        if (_gameData.InteractionData.ScreenState is ScreenState.Default or ScreenState.Region)
        {
            _camera.Update(delta);
        }

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
        const int TileSize = 64;

        if (_gameData.InteractionData.ScreenState is ScreenState.Region)
        {
            if (_gameData.ActiveRegion is { } activeRegion)
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
                            x * TileSize,
                            y * TileSize,
                            TileSize,
                            TileSize,
                            tile.Color);
                    }
                }

                foreach (var k in _gameData.SimulatedKakapo.Where(_ => _.RegionId == activeRegion.Id))
                {
                    Raylib.DrawRectangle(
                        (int)k.CurrentLocation.X * TileSize,
                        (int)k.CurrentLocation.Y * TileSize,
                        TileSize,
                        TileSize,
                        Color.DarkGreen);
                }

                Raylib.EndMode2D();
            }
        }
        else
        {
            Raylib.BeginMode2D(camera);
            int regionIdx = 0;
            foreach (var region in _gameData.Regions)
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

                if (_gameData.InteractionData.DefaultScreenData.SelectedRegion == regionIdx)
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
    }
}
