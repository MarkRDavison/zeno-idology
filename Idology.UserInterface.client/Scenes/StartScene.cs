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
                Layout = new LayoutItem
                {
                    RequestedPadding = new LayoutEdges(4.0f),
                    RequestedSize = new LayoutVector(0, 48),
                    Contain = ContainFlags.Row,
                    Behave = BehaveFlags.HFill,
                    Align = AlignFlags.Start
                }
            });

            for (int i = 0; i < 6; ++i)
            {
                topBar.AddChild(new PanelWidget
                {
                    Background = Color.Gray,
                    Border = Color.DarkGray,
                    BorderThickness = 2.0f,
                    Layout = new LayoutItem
                    {
                        RequestedMargin = i == 0 ? new() : new LayoutEdges(2.0f, 0.0f, 0.0f, 0.0f),
                        RequestedSize = new LayoutVector(128, 0),
                        Contain = ContainFlags.Row,
                        Behave = BehaveFlags.VFill
                    }
                });
            }
            topBar.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
                Layout = new LayoutItem
                {
                    RequestedMargin = new LayoutEdges(2.0f, 0.0f, 0.0f, 0.0f),
                    RequestedSize = new LayoutVector(0, 0),
                    Contain = ContainFlags.Row,
                    Behave = BehaveFlags.Fill
                }
            });
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
                    Contain = ContainFlags.Column
                }
            });

            var button = background.AddChild(new TextButtonWidget
            {
                TextContent = "BUTTON 1",
                Foreground = Color.Yellow,
                Background = Color.Green,
                Border = Color.DarkBlue,
                BorderThickness = 4.0f,
                Layout = new LayoutItem
                {
                    Behave = BehaveFlags.Center,
                    RequestedSize = new LayoutVector(256, 96)
                }
            });

            button.OnClick += (s, e) => Console.WriteLine("BUTTON 1 IS CLICKED!");

            var button2 = background.AddChild(new DropdownWidget
            {
                Foreground = Color.Yellow,
                Background = Color.Green,
                Border = Color.DarkBlue,
                BorderThickness = 4.0f,
                Layout = new LayoutItem
                {
                    RequestedMargin = new LayoutEdges(0.0f, 8.0f, 0.0f, 0.0f),
                    Behave = BehaveFlags.Center,
                    RequestedSize = new LayoutVector(256, 64)
                },
                SelectedItemId = "3",
                Items =
                {
                    new DropdownItem("Item 1", "1"),
                    new DropdownItem("Item 2", "2"),
                    new DropdownItem("Item 3", "3"),
                    new DropdownItem("Item 4 with more text", "4"),
                    new DropdownItem("Item 5", "5"),
                }
            });

            button2.OnItemSelected += (s, e) =>
            {
                Console.WriteLine("Selected: '{0}' ({1})", e.Text, e.Id);
                button2.SelectedItemId = e.Id;
            };
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
