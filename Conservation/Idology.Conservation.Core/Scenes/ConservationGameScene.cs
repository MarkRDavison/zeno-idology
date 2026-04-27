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

    private KakapoDetailsSubScene? _kakapoDetailsSubScene;
    private StaffDetailsSubScene? _staffDetialsSubScene;
    private ResearchSubScene? _researchSubScene;
    private TechnologySubScene? _technologySubScene;
    private FundingSubScene? _fundingSubScene;

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
        _eventRoutingService.SetSubScene += OnSetSubScene;
        _eventRoutingService.PopSubScene += OnPopSubScene;

        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        InitUserInterface();

        {
            _gameData.ActiveRegion = null;
            _gameData.Regions.Clear();

            _gameDateTimeProvider.Set(new DateTime(2026, 01, 01));

            // TODO: Load from files....
            // https://encyclopedia.pub/entry/37611
            _gameData.KakapoData.Add(new(1, "Flossie", true, null, null, new DateOnly(1982, 1, 1), null));
            _gameData.KakapoData.Add(new(2, "Solstice", true, null, null, new DateOnly(1989, 1, 1), null));
            _gameData.KakapoData.Add(new(3, "Nora", true, null, null, new DateOnly(1980, 1, 1), null));
            _gameData.KakapoData.Add(new(4, "Rakiura", true, 1, null, new DateOnly(2002, 2, 19), null));
            _gameData.KakapoData.Add(new(5, "Esperance", true, 1, null, new DateOnly(2002, 2, 17), null));
            _gameData.KakapoData.Add(new(6, "Margaret-Maree", true, null, null, new DateOnly(1986, 1, 1), null));
            _gameData.KakapoData.Add(new(7, "Marama", true, null, null, new DateOnly(2002, 1, 1), null));
            _gameData.KakapoData.Add(new(8, "Stella", true, 2, null, new DateOnly(2011, 1, 1), null));
            _gameData.KakapoData.Add(new(9, "Heather", true, 1, null, new DateOnly(1981, 1, 1), null));
            _gameData.KakapoData.Add(new(10, "Ako", true, 5, null, new DateOnly(2019, 1, 1), null));
            _gameData.KakapoData.Add(new(11, "Alice", true, null, null, new DateOnly(1981, 1, 1), null));
            _gameData.KakapoData.Add(new(12, "Lisa", true, null, null, new DateOnly(1982, 1, 1), null));
            _gameData.KakapoData.Add(new(13, "Wendy", true, null, null, new DateOnly(1982, 1, 1), null));
            _gameData.KakapoData.Add(new(14, "Atareta", true, 12, null, new DateOnly(2011, 1, 1), null));
            _gameData.KakapoData.Add(new(15, "Tia", true, 4, null, new DateOnly(2011, 1, 1), null));
            _gameData.KakapoData.Add(new(16, "Toitiiti", true, 4, null, new DateOnly(2011, 1, 1), null));
            _gameData.KakapoData.Add(new(17, "Yasmine", true, 1, null, new DateOnly(2005, 1, 1), null));
            _gameData.KakapoData.Add(new(18, "Zephyr", true, 3, null, new DateOnly(1981, 1, 1), null));
            _gameData.KakapoData.Add(new(19, "Vori", true, 11, null, new DateOnly(2019, 1, 1), null));
            _gameData.KakapoData.Add(new(20, "Tohu", true, 9, null, new DateOnly(2014, 1, 1), null));
            _gameData.KakapoData.Add(new(21, "Aparima", true, 13, null, new DateOnly(2002, 1, 1), null));
            _gameData.KakapoData.Add(new(22, "JEM", true, 21, null, new DateOnly(2008, 1, 1), null));

            _gameData.StaffData.Add(new(1, "Tom"));
            _gameData.StaffData.Add(new(2, "Sarah"));
            _gameData.StaffData.Add(new(3, "Theo"));
            _gameData.StaffData.Add(new(4, "Danielle"));

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
    }

    private void OnSetSubScene(object? sender, SetSubSceneGameCommand e)
    {
        void handleSetSubScene<TSubScene, TSubScenePayload>(ref TSubScene? subScene)
            where TSubScene : SubScene<TSubScene, TSubScenePayload>
            where TSubScenePayload : class, new()
        {
            if (subScene is null)
            {
                subScene = _serviceProvider.GetRequiredService<TSubScene>();
                subScene.Init(new TSubScenePayload());
            }

            PushSubScene(subScene);
        }

        if (e.Id is Constants.SubScene_KakapoDetails)
        {
            handleSetSubScene<KakapoDetailsSubScene, KakapoDetailsSubScenePayload>(ref _kakapoDetailsSubScene);
        }
        else if (e.Id is Constants.SubScene_StaffDetails)
        {
            handleSetSubScene<StaffDetailsSubScene, StaffDetailsSubScenePayload>(ref _staffDetialsSubScene);
        }
        else if (e.Id is Constants.SubScene_ResearchDetails)
        {
            handleSetSubScene<ResearchSubScene, ResearchSubScenePayload>(ref _researchSubScene);
        }
        else if (e.Id is Constants.SubScene_TechnologyDetails)
        {
            handleSetSubScene<TechnologySubScene, TechnologySubScenePayload>(ref _technologySubScene);
        }
        else if (e.Id is Constants.SubScene_FundingDetails)
        {
            handleSetSubScene<FundingSubScene, FundingSubScenePayload>(ref _fundingSubScene);
        }
    }

    private void OnPopSubScene(object? sender, PopSubSceneGameCommand e)
    {
        if (e.Clear)
        {
            PopAllScenes();
        }
        else
        {
            PopSubScene();
        }
    }

    private void InitUserInterface()
    {
        var root = _userInterfaceRoot.RootWidget;
        root.Layout.Contain = ContainFlags.Flex;
        {
            var topBar = root.AddChild(_serviceProvider.GetRequiredService<TopBarWidget>());

            topBar.Layout.Contain = ContainFlags.Row;
            topBar.Layout.Behave = BehaveFlags.HFill | BehaveFlags.Top;
            topBar.Layout.Align = AlignFlags.Start;
        }
    }

    public override void Update(float delta)
    {
        if (Raylib.IsWindowResized())
        {
            _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

            ForEachSubScene(ss => ss.OnWindowResize(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        }

        _conservationGameInteractionService.Update(delta);

        ForEachSubScene(ss => ss.Update(delta));

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

        var subSceneCamera = new Camera2D(new Vector2(0.0f, TopBarWidget.Height), new(), 0.0f, 1.0f);

        ForEachSubSceneReverse(ss => ss.Draw(subSceneCamera));

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
