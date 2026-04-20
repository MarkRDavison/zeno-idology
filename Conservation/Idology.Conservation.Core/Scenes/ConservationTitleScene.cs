namespace Idology.Conservation.Core.Scenes;

using Idology.Engine.Infrastructure;

public sealed class ConservationTitleScene : Scene
{
    private readonly ISceneService _sceneService;
    private readonly IFontManager _fontManager;
    private readonly IInputManager _inputManager;
    private readonly Application _application;

    private readonly string[] _buttons = { "Start", "Load", "Quit" };
    private int _hoveredIndex = -1;

    public ConservationTitleScene(
        ISceneService sceneService,
        IFontManager fontManager,
        IInputManager inputManager
,
        Application application)
    {
        _sceneService = sceneService;
        _fontManager = fontManager;
        _inputManager = inputManager;
        _application = application;
    }

    public override void Init()
    {

    }

    public override void Update(float delta)
    {
        _hoveredIndex = -1;

        var font = _fontManager.GetFont("CALIBRIB");
        var w = Raylib.GetScreenWidth();
        var h = Raylib.GetScreenHeight();
        var y = h - 300;
        var mouse = _inputManager.GetMousePosition();

        for (int i = 0; i < _buttons.Length; i++)
        {
            var text = _buttons[i];
            var bounds = Raylib.MeasureTextEx(font, text, 48, 0.0f);
            var offset = w / 2.0f - bounds.X / 2.0f;
            var rect = new Rectangle(offset, y + i * 70, bounds.X, bounds.Y);

            if (Raylib.CheckCollisionPointRec(mouse, rect))
            {
                _hoveredIndex = i;
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    switch (i)
                    {
                        case 0: _sceneService.SetScene<ConservationGameScene>(); break;
                        case 1: /* Load logic */ break;
                        case 2: _application.Stop(); break;
                    }
                }
            }
        }
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.SkyBlue);

        var font = _fontManager.GetFont("CALIBRIB");

        var w = Raylib.GetScreenWidth();
        var h = Raylib.GetScreenHeight();
        var bounds = Raylib.MeasureTextEx(font, Constants.Title, 96, 0.0f);
        var offset = w / 2.0f - bounds.X / 2.0f;

        Raylib.DrawTextEx(font, Constants.Title, new System.Numerics.Vector2(offset, 128), 96, 0.0f, Color.DarkGreen);

        var y = h - 300;
        for (int i = 0; i < _buttons.Length; i++)
        {
            var text = _buttons[i];
            var textBounds = Raylib.MeasureTextEx(font, text, 48, 0.0f);
            var x = w / 2.0f - textBounds.X / 2.0f;
            var color = (i == _hoveredIndex) ? Color.Yellow : Color.White;
            Raylib.DrawTextEx(font, text, new System.Numerics.Vector2(x, y + i * 70), 48, 0.0f, color);
        }

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
