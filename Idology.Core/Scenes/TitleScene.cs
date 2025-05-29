namespace Idology.Core.Scenes;

public sealed class TitleScene : Scene
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ISceneService _sceneService;

    private readonly IList<ComponentBase> _components = [];

    public TitleScene(
        IHostApplicationLifetime hostApplicationLifetime,
        ISceneService sceneService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _sceneService = sceneService;

        _components.Add(new Button
        {
            Bounds = new Rectangle((Raylib.GetScreenWidth() / 2 - 100), Raylib.GetScreenHeight() / 2, 200, 50),
            OnClick = () => OnClick("START"),
            Label = "Start"
        });

        _components.Add(new Button
        {
            Bounds = new Rectangle((Raylib.GetScreenWidth() / 2 - 100), Raylib.GetScreenHeight() / 2 + 100, 200, 50),
            OnClick = () => OnClick("OPTIONS"),
            Label = "Options"
        });

        _components.Add(new Button
        {
            Bounds = new Rectangle((Raylib.GetScreenWidth() / 2 - 100), Raylib.GetScreenHeight() / 2 + 200, 200, 50),
            OnClick = () => OnClick("SHEET"),
            Label = "Sprites"
        });

        _components.Add(new Button
        {
            Bounds = new Rectangle((Raylib.GetScreenWidth() / 2 - 100), Raylib.GetScreenHeight() / 2 + 300, 200, 50),
            OnClick = () => OnClick("EXIT"),
            Label = "Exit"
        });
    }

    public override void Update(float delta)
    {
        foreach (var c in _components)
        {
            c.Update(delta);
        }
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        const string title = "Idology";

        Raylib.DrawText(title, Raylib.GetScreenWidth() / 2 - Raylib.MeasureText(title, 50) / 2, 100, 50, Color.Blue);

        foreach (var c in _components)
        {
            c.Draw();
        }

        Raylib.EndDrawing();
    }

    private void OnClick(string id)
    {
        switch (id)
        {
            case "START":
                _sceneService.SetScene<GameScene>();
                break;
            case "OPTIONS":
                break;
            case "SHEET":
                _sceneService.SetScene<MedievalSheetExplorerScene>();
                break;
            case "EXIT":
                _hostApplicationLifetime.StopApplication();
                break;
        }
    }
}
