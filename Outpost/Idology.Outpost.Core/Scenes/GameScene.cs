namespace Idology.Outpost.Core.Scenes;

public sealed class GameScene : Scene
{
    private Camera2D _camera;
    private readonly Game _game;
    private readonly GameRenderer _gameRenderer;
    private readonly GameData _gameData;

    public GameScene(
        Game game,
        GameRenderer gameRenderer,
        GameData gameData)
    {
        _game = game;
        _camera = new Camera2D();
        _gameRenderer = gameRenderer;
        _gameData = gameData;
    }

    public override void Init()
    {
        _camera.Zoom = 0.4f;
        _camera.Offset = new Vector2(
            Raylib.GetScreenWidth() / 3,
            Raylib.GetScreenHeight() / 2);
    }

    public override void Update(float delta)
    {

        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (_gameData.Town.TimeOfDay == TimeOfDay.Day)
            {
                _game.ApplyCommand(new DummyCommand("SUNSET"));
            }
            else
            {
                _game.ApplyCommand(new DummyCommand("SUNRISE"));
            }
        }

        _game.Update(delta);
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        Raylib.DrawFPS(10, 10);

        Raylib.BeginMode2D(_camera);

        _gameRenderer.Draw(_camera);

        Raylib.EndMode2D();

        Raylib.EndDrawing();
    }
}
