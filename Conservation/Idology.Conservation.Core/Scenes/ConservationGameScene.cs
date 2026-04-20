using Idology.Conservation.Core.Models;

namespace Idology.Conservation.Core.Scenes;

public class ConservationGameScenePayload : IScenePayload<ConservationGameScene>
{
    public required bool Load { get; init; }
}

public class ConservationGameScene : Scene<ConservationGameScene>
{
    private readonly ConservationGameData _gameData;
    private readonly ConservationGame _game;
    private readonly ConservationGameRenderer _gameRenderer;
    private readonly IInputManager _inputManager;

    public ConservationGameScene(
        ConservationGameData gameData,
        ConservationGame game,
        ConservationGameRenderer gameRenderer,
        IInputManager inputManager)
    {
        _gameData = gameData;
        _game = game;
        _gameRenderer = gameRenderer;
        _inputManager = inputManager;
    }

    public override void Init(IScenePayload<ConservationGameScene>? payload)
    {
        if (payload is ConservationGameScenePayload cgsp)
        {

            _gameData.ActiveRegion = null;
            _gameData.Regions.Clear();

            List<string> regions = ["region-1", "region-2"];

            foreach (var r in regions)
            {
                var regionData = RegionModel.Create(r);

                _gameData.Regions.Add(regionData.ToRegionData());
            }

        }
    }

    public override void Update(float delta)
    {
        _game.Update(delta);
        _gameRenderer.Update(delta);
        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        _gameRenderer.Draw(new Camera2D());

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
