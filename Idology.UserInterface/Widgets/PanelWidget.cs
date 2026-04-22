namespace Idology.UserInterface.Widgets;

public sealed class PanelWidget : BaseWidget
{

    public override void Draw()
    {
        Raylib.DrawRectangle((int)Layout.Rect.X, (int)Layout.Rect.Y, (int)Layout.Rect.Width, (int)Layout.Rect.Height, Foreground);

        DrawChildren();
    }
}
