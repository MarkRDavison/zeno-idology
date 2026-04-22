namespace Idology.UserInterface.Widgets;

internal sealed class RootWidget : BaseWidget
{
    public override void Draw()
    {
        Raylib.DrawRectangle((int)Layout.Rect.X, (int)Layout.Rect.Y, (int)Layout.Rect.Width, (int)Layout.Rect.Height, Color.Beige);

        DrawChildren();
    }
}
