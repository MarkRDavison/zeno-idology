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
                // TODO: Validate data, that FatherId -> Male, MotherId -> Female etc
                // https://encyclopedia.pub/entry/37611
                _gameData.KakapoData.Add(new KakapoModel(1, "Flossie", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), null));
                _gameData.KakapoData.Add(new KakapoModel(2, "Solstice", Gender.Female, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered, "LAST_DISCOVERD_WILD"), null));
                _gameData.KakapoData.Add(new KakapoModel(3, "Nora", Gender.Female, null, null, new OriginInfo(new DateOnly(1980, 1, 1), OriginDateType.Discovered), null));
                _gameData.KakapoData.Add(new KakapoModel(4, "Rakiura", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 19), OriginDateType.KnownBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(5, "Esperance", Gender.Female, 1, 23, new OriginInfo(new DateOnly(2002, 2, 17), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(6, "Margaret-Maree", Gender.Female, null, null, new OriginInfo(new DateOnly(1986, 1, 1), OriginDateType.Discovered), null));
                _gameData.KakapoData.Add(new KakapoModel(7, "Marama", Gender.Female, 6, 24, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(8, "Stella", Gender.Female, 2, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(9, "Heather", Gender.Female, 1, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(10, "Ako", Gender.Female, 5, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(11, "Alice", Gender.Female, null, null, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(12, "Lisa", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(13, "Wendy", Gender.Female, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(14, "Atareta", Gender.Female, 12, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(15, "Tia", Gender.Female, 4, null, new OriginInfo(new DateOnly(2011, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(16, "Toitiiti", Gender.Female, 4, null, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(17, "Yasmine", Gender.Female, 1, null, new OriginInfo(new DateOnly(2005, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(18, "Zephyr", Gender.Female, 3, 25, new OriginInfo(new DateOnly(1981, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(19, "Vori", Gender.Female, 11, null, new OriginInfo(new DateOnly(2019, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(20, "Tohu", Gender.Female, 9, null, new OriginInfo(new DateOnly(2014, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(21, "Aparima", Gender.Female, 13, null, new OriginInfo(new DateOnly(2002, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(22, "JEM", Gender.Female, 21, 23, new OriginInfo(new DateOnly(2008, 1, 1), OriginDateType.EstimatedBirth), null));
                _gameData.KakapoData.Add(new KakapoModel(23, "Bill", Gender.Male, null, null, new OriginInfo(new DateOnly(1982, 1, 1), OriginDateType.Discovered), new DateOnly(2008, 3, 1)));
                _gameData.KakapoData.Add(new KakapoModel(24, "Nog", Gender.Male, null, null, new OriginInfo(new DateOnly(1989, 1, 1), OriginDateType.Discovered), null));
                _gameData.KakapoData.Add(new KakapoModel(25, "Rangi", Gender.Male, null, null, new OriginInfo(new DateOnly(1987, 1, 1), OriginDateType.Discovered), null));
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

            List<string> regions = ["region-1", "region-2", "region-3"];

            foreach (var r in regions)
            {
                var regionData = RegionModel.Create(r);

                _gameData.Regions.Add(regionData.ToRegionData());
            }

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

        {
            _kakapoDetailsSubSceneWidget = root.AddChild(_serviceProvider.GetRequiredService<KakapoDetailsUiSubScenePanelWidget>());
            _kakapoDetailsSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _staffDetialsSubSceneWidget = root.AddChild(_serviceProvider.GetRequiredService<StaffDetailsUiSubScenePanelWidget>());
            _staffDetialsSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _researchSubSceneWidget = root.AddChild(_serviceProvider.GetRequiredService<ResearchUiSubScenePanelWidget>());
            _researchSubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _technologySubSceneWidget = root.AddChild(_serviceProvider.GetRequiredService<TechnologyUiSubScenePanelWidget>());
            _technologySubSceneWidget.Layout.Visibility = Visibility.Collapsed;
        }
        {
            _fundingSubSceneWidget = root.AddChild(_serviceProvider.GetRequiredService<FundingUiSubScenePanelWidget>());
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
        Raylib.ClearBackground(Color.SkyBlue);

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

                        Raylib.DrawRectangle(
                            x * TileSize,
                            y * TileSize,
                            TileSize,
                            TileSize,
                            tile.Color);
                    }
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
                    Raylib.DrawRectangleLinesEx(
                        new Rectangle(
                            new Vector2(
                                (int)region.RegionOffset.X * TileSize,
                                (int)region.RegionOffset.Y * TileSize),
                            new Vector2(
                                TileSize * region.Width,
                                TileSize * region.Height)
                            ),
                        (float)(TileSize / 4.0f),
                        Color.White);
                }

                regionIdx++;
            }
            Raylib.EndMode2D();
        }
    }
}
