namespace Idology.Engine.UiComponents;

public class Button : ComponentBase
{
    private bool _mouseContained = false;

    public Rectangle Bounds { get; set; }
    public Color FontColor { get; set; } = Color.RayWhite;
    public Color Color { get; set; } = Color.LightGray;
    public Color HoverColor { get; set; } = Color.DarkGray;
    public Color BorderColor { get; set; } = Color.Gray;
    public Action? OnClick { get; set; }
    public int BorderThickness { get; set; } = 4;
    public string? Label { get; set; }
    public int FontSize { get; set; } = 32;

    public override void Update(float delta)
    {
        var mousePos = Raylib.GetMousePosition();

        _mouseContained = Raylib.CheckCollisionPointRec(mousePos, Bounds);

        if (OnClick is not null && _mouseContained && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            OnClick();
        }
    }

    public override void Draw()
    {
        if (BorderThickness > 0)
        {
            var innerBounds = new Rectangle(Bounds.X + BorderThickness, Bounds.Y + BorderThickness, Bounds.Width - 2 * BorderThickness, Bounds.Height - 2 * BorderThickness);
            Raylib.DrawRectangleRec(Bounds, BorderColor);
            Raylib.DrawRectangleRec(innerBounds, _mouseContained ? HoverColor : Color);
        }
        else
        {
            Raylib.DrawRectangleRec(Bounds, Color);
        }

        if (!string.IsNullOrEmpty(Label))
        {
            var font = Services.GetRequiredService<IFontManager>().GetFont("DEBUG");
            var textBounds = Raylib.MeasureTextEx(font, Label, FontSize, 1);

            var position = Bounds.Position + Bounds.Size / 2 - new Vector2(textBounds.X, textBounds.Y) / 2;

            Raylib.DrawTextEx(font, Label, position, FontSize, 1, FontColor);
        }
    }
}
