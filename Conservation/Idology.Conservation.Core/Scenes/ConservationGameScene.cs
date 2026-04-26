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
    private readonly ConservationGameRenderer _gameRenderer;
    private readonly IInputManager _inputManager;
    private readonly IConservationGameInteractionService _conservationGameInteractionService;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly ConservationGameCamera _camera;
    private readonly IServiceProvider _serviceProvider;

    public ConservationGameScene(
        ConservationGameData gameData,
        ConservationGame game,
        ConservationGameRenderer gameRenderer,
        IInputManager inputManager,
        IConservationGameInteractionService conservationGameInteractionService,
        IGameDateTimeProvider gameDateTimeProvider,
        IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider)
    {
        _gameData = gameData;
        _game = game;
        _gameRenderer = gameRenderer;
        _inputManager = inputManager;
        _conservationGameInteractionService = conservationGameInteractionService;
        _gameDateTimeProvider = gameDateTimeProvider;
        _userInterfaceRoot = userInterfaceRoot;
        _serviceProvider = serviceProvider;

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
        }

        _conservationGameInteractionService.Update(delta);

        ForEachSubScene(ss => ss.Update(delta));

        _game.Update(delta);

        _gameRenderer.Update(delta);

        _userInterfaceRoot.Update(delta);

        _camera.Update(delta);

        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        // TODO: Whats the relationship between the sub scene and the game renderer???
        _gameRenderer.Draw(_camera.Camera);

        ForEachSubSceneReverse(ss => ss.Draw());

        _userInterfaceRoot.RootWidget.Draw();

        Raylib.DrawFPS(10, 58);

        Raylib.EndDrawing();
    }
}
