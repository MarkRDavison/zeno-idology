namespace Idology.UserInterface.Client.Scenes;

internal sealed class StartScene : Scene<StartScene>
{
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IInputManager _inputManager;

    public StartScene(
        IUserInterfaceRoot userInterfaceRoot,
        IInputManager inputManager)
    {
        _userInterfaceRoot = userInterfaceRoot;
        _inputManager = inputManager;
    }

    public override void Init(IScenePayload<StartScene>? payload)
    {
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        _userInterfaceRoot.RootWidget.Layout.Contain = ContainFlags.Layout;
        var root = _userInterfaceRoot.RootWidget;

        var background = root.AddChild(new PanelWidget
        {
            Foreground = Color.Gray,
            Layout = new LayoutItem
            {
                RequestedMargin = new LayoutEdges(32.0f),
                Behave = BehaveFlags.Fill,
                Contain = ContainFlags.Row
            }
        });

        var button = background.AddChild(new TextButtonWidget
        {
            Foreground = Color.Yellow,
            Background = Color.Green,
            Border = Color.DarkBlue,
            Layout = new LayoutItem
            {
                Behave = BehaveFlags.Center,
                RequestedSize = new LayoutVector(256, 96)
            }
        });

        button.OnClick += (s, e) => Console.WriteLine("BUTTON IS CLICKED!");
    }


    public override void Update(float delta)
    {
        if (Raylib.IsWindowResized())
        {
            _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        }

        _userInterfaceRoot.Update(delta);
        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.SkyBlue);

        _userInterfaceRoot.RootWidget.Draw();

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }

}
