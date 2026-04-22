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

        var root = _userInterfaceRoot.RootWidget;
        root.Layout.Contain = ContainFlags.Column;

        {
            var topBar = root.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
                // Gap = 2.0f, // TODO: Need gap for parents...
                Layout = new LayoutItem
                {
                    RequestedSize = new LayoutVector(0, 48),
                    Contain = ContainFlags.Row,
                    Behave = BehaveFlags.HFill,
                    Align = AlignFlags.Start
                }
            });

            for (int i = 0; i < 4; ++i)
            {
                topBar.AddChild(new PanelWidget
                {
                    Background = Color.Gray,
                    Border = Color.DarkGray,
                    BorderThickness = 2.0f,
                    Layout = new LayoutItem
                    {
                        RequestedSize = new LayoutVector(128, 0),
                        Contain = ContainFlags.Row,
                        Behave = BehaveFlags.VFill
                    }
                });
            }
        }

        {
            var background = root.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderRoundness = 0.025f,
                BorderThickness = 8.0f,
                Layout = new LayoutItem
                {
                    RequestedMargin = new LayoutEdges(32.0f),
                    Behave = BehaveFlags.Fill,
                    Contain = ContainFlags.Row
                }
            });

            var button = background.AddChild(new TextButtonWidget
            {
                TextContent = "Hello World!",
                Foreground = Color.Yellow,
                Background = Color.Green,
                Border = Color.DarkBlue,
                BorderThickness = 8.0f,
                Layout = new LayoutItem
                {
                    Behave = BehaveFlags.Center,
                    RequestedSize = new LayoutVector(256, 96)
                }
            });

            button.OnClick += (s, e) => Console.WriteLine("BUTTON IS CLICKED!");
        }
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

        Raylib.DrawFPS(10, 52);

        Raylib.EndDrawing();
    }

}
