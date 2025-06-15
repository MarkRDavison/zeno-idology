namespace Idology.Space.Core.Scenes;

public sealed class GameScene : Scene
{
    private GameCamera _camera;
    private readonly Infrastructure.Game _game;
    private readonly GameRenderer _gameRenderer;
    private readonly GameData _gameData;

    public GameScene(
        GameCamera camera,
        Infrastructure.Game game,
        GameRenderer gameRenderer,
        GameData gameData)
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
