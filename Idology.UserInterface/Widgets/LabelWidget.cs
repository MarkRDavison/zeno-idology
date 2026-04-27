namespace Idology.UserInterface.Widgets;

public class LabelWidget : BaseWidget
{
    public override void Draw()
    {
        if (string.IsNullOrEmpty(TextContent))
        {
            return;
        }

        Raylib.DrawText(TextContent, (int)Layout.Rect.X, (int)Layout.Rect.Y, FontSize, Foreground);
    }

    public int FontSize { get; set; } = 32;
    public string TextContent { get; set; } = string.Empty;
}
