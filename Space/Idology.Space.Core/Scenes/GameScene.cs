namespace Idology.Space.Core.Scenes;

public sealed class GameScene : Scene
{
    private Camera2D _camera;
    private readonly Infrastructure.Game _game;
    private readonly GameRenderer _gameRenderer;
    private readonly GameData _gameData;

    public GameScene(
        Infrastructure.Game game,
        GameRenderer gameRenderer,
        GameData gameData)
    {
        _camera = new Camera2D();
        _game = game;
        _gameRenderer = gameRenderer;
        _gameData = gameData;
    }

    public override void Init()
    {
        _camera.Zoom = 0.5f;
        _camera.Offset = new Vector2(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2);
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

        Raylib.DrawFPS(10, 10);

        _gameRenderer.Draw(_camera);

        Raylib.EndDrawing();
    }
}
