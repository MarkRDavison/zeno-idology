using Idology.UserInterface.Layout;

namespace Idology.UserInterface.Client.Scenes;

internal sealed class StartScene : Scene<StartScene>
{
    private readonly Application _application;

    private LayoutItem _root;

    public StartScene(Application application)
    {
        _application = application;

        _root = new LayoutItem
        {
            RequestedSize = new LayoutVector(
                Raylib.GetScreenWidth(),
                Raylib.GetScreenHeight()),
            Behave = BehaveFlags.Fill,
            RequestedMargin = new LayoutEdges(16.0f),
            Contain = ContainFlags.Row,
        };

        var masterList = new LayoutItem
        {
            RequestedSize = new(400, 0),
            Behave = BehaveFlags.VFill,
            RequestedMargin = new LayoutEdges(16.0f),
            Contain = ContainFlags.Column,
        };
        _root.AddChild(masterList);

        var contentView = new LayoutItem
        {
            Behave = BehaveFlags.Fill,
            RequestedMargin = new LayoutEdges(16.0f),
        };
        _root.AddChild(contentView);
    }

    public override void Update(float delta)
    {
        _root.RequestedSize = new LayoutVector(
            Raylib.GetScreenWidth() - _root.RequestedMargin.Left - _root.RequestedMargin.Right,
            Raylib.GetScreenHeight() - _root.RequestedMargin.Top - _root.RequestedMargin.Bottom);

        _root.Run();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.SkyBlue);

        List<Color> Colours = [Color.Beige, Color.RayWhite, Color.RayWhite, Color.Blue, Color.Red, Color.Green];

        int idx = 0;

        Action<LayoutItem> drawFunc = null!;

        drawFunc = (LayoutItem item) =>
        {
            Raylib.DrawRectangle((int)item.Rect.X, (int)item.Rect.Y, (int)item.Rect.Width, (int)item.Rect.Height, Colours[idx++]);

            foreach (var c in item.Children)
            {
                drawFunc(c);
            }
        };

        drawFunc(_root);

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }

}
