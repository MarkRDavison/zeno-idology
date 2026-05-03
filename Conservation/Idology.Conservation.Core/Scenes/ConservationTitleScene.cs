namespace Idology.Conservation.Core.Scenes;

public sealed class ConservationTitleScene : Scene<ConservationTitleScene>
{
    private readonly ISceneService _sceneService;
    private readonly IFontManager _fontManager;
    private readonly IInputManager _inputManager;
    private readonly ITranslationService _translationService;
    private readonly Application _application;

    private readonly string[] _buttons;
    private int _hoveredIndex = -1;

    public ConservationTitleScene(
        ISceneService sceneService,
        IFontManager fontManager,
        IInputManager inputManager,
        ITranslationService translationService,
        Application application)
    {
        _sceneService = sceneService;
        _fontManager = fontManager;
        _inputManager = inputManager;
        _translationService = translationService;
        _application = application;

        _buttons = [
            _translationService["TITLE_SCREEN_START"],
            _translationService["TITLE_SCREEN_SIMULATE"],
            _translationService["TITLE_SCREEN_LOAD"],
            _translationService["TITLE_SCREEN_LOAD_DEV"],
            _translationService["TITLE_SCREEN_QUIT"],
        ];
    }

    public override void Init(IScenePayload<ConservationTitleScene>? payload)
    {

    }

    public override void Update(float delta)
    {
        _hoveredIndex = -1;

        var font = _fontManager.GetFont("CALIBRIB");
        var w = Raylib.GetScreenWidth();
        var h = Raylib.GetScreenHeight();
        var y = h - 400;
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
                // TODO: DO INPUT MANAGER CHECKS
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    _inputManager.MarkActionAsHandled(Constants.Action_Click);
                    switch (i)
                    {
                        case 0:
                            _sceneService.SetScene(new ConservationGameScenePayload { Load = false, Dev = false });
                            break;
                        case 1:
                            _sceneService.SetScene(new ConservationSimulationTestScenePayload { });
                            break;
                        case 2:
                            _sceneService.SetScene(new ConservationGameScenePayload { Load = true, Dev = false });
                            break;
                        case 3:
                            _sceneService.SetScene(new ConservationGameScenePayload { Load = true, Dev = true });
                            break;
                        case 4:
                            _application.Stop();
                            break;
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

        var title = _translationService["TITLE_SCREEN_TITLE"];

        var w = Raylib.GetScreenWidth();
        var h = Raylib.GetScreenHeight();
        var bounds = Raylib.MeasureTextEx(font, title, 96, 0.0f);
        var offset = w / 2.0f - bounds.X / 2.0f;

        Raylib.DrawTextEx(font, title, new System.Numerics.Vector2(offset, 128), 96, 0.0f, Color.DarkGreen);

        var y = h - 400;
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
