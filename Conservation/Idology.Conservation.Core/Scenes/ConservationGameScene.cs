namespace Idology.Conservation.Core.Scenes;

public class ConservationGameScenePayload : IScenePayload<ConservationGameScene>
{
    public required bool Load { get; init; }
    public required bool Dev { get; init; }
}

public class ConservationGameScene : Scene<ConservationGameScene>
{
    private readonly ConservationGameData _gameData;
    private readonly ConservationGame _game;
    private readonly ConservationGameRenderer _gameRenderer;
    private readonly IInputManager _inputManager;
    private readonly IConservationGameInteractionService _conservationGameInteractionService;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private ConservationGameCamera _camera;

    public ConservationGameScene(
        ConservationGameData gameData,
        ConservationGame game,
        ConservationGameRenderer gameRenderer,
        IInputManager inputManager,
        IConservationGameInteractionService conservationGameInteractionService,
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameData = gameData;
        _game = game;
        _gameRenderer = gameRenderer;
        _inputManager = inputManager;
        _conservationGameInteractionService = conservationGameInteractionService;
        _gameDateTimeProvider = gameDateTimeProvider;

        _camera = new ConservationGameCamera(inputManager);

        // TODO: Better way of applying defaults...
        _camera.Target = new Vector2(4500, 4000);
        _camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        _camera.Zoom = 0.10f;
    }

    public override void Init(IScenePayload<ConservationGameScene>? payload)
    {
        if (payload is ConservationGameScenePayload cgsp)
        {
            _gameData.ActiveRegion = null;
            _gameData.Regions.Clear();

            _gameDateTimeProvider.Set(new DateTime(2026, 01, 01));

            // TODO: Load from files....
            // https://encyclopedia.pub/entry/37611
            _gameData.KakapoData.Add(new(1, "Flossie", true, null, null, new DateOnly(1982, 1, 1), null));
            _gameData.KakapoData.Add(new(2, "Rakiura", true, 1, null, new DateOnly(2002, 2, 19), null));
            _gameData.KakapoData.Add(new(3, "Esperance", true, 1, null, new DateOnly(2002, 2, 17), null));

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

    public override void Update(float delta)
    {
        _conservationGameInteractionService.Update(delta);

        _game.Update(delta);

        _gameRenderer.Update(delta);

        _camera.Update(delta);

        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        _gameRenderer.Draw(_camera.Camera);

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
