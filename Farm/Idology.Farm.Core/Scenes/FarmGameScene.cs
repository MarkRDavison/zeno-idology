using Idology.Engine.Core;
using Idology.Farm.Core.Infrastructure;

namespace Idology.Farm.Core.Scenes;

public class FarmGameScene : Scene
{
    private GameCamera _camera;
    private readonly FarmGame _game;
    private readonly FarmGameRenderer _gameRenderer;
    private readonly FarmGameData _gameData;

    public FarmGameScene(
        GameCamera camera,
        FarmGame game,
        FarmGameRenderer gameRenderer,
        FarmGameData gameData)
    {
        _camera = camera;
        _game = game;
        _gameRenderer = gameRenderer;
        _gameData = gameData;
    }

    public override void Init()
    {

    }

    public override void Update(float delta)
    {

        _game.Update(delta);
        _gameRenderer.Update(delta);
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
