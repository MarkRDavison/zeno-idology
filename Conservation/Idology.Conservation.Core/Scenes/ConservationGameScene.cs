namespace Idology.Conservation.Core.Scenes;

public class ConservationGameScene : Scene
{
    private readonly ConservationGame _game;
    private readonly ConservationGameRenderer _gameRenderer;
    private readonly IInputManager _inputManager;

    public ConservationGameScene(
        ConservationGame game,
        ConservationGameRenderer gameRenderer,
        IInputManager inputManager)
    {
        _game = game;
        _gameRenderer = gameRenderer;
        _inputManager = inputManager;
    }

    public override void Init()
    {

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
